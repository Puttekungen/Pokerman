using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask blockedLayer;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private LayerMask stairsLayer;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stairMusic;

    public LayerMask interactableLayer;

    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterTrainersView;

    private float tileSize = 1f;

    // Separate timings for walk vs run
    [SerializeField] private float walkMoveTime = 0.35f;
    [SerializeField] private float runMoveTime = 0.18f;

    private bool isMoving = false;
    private bool isRunning = false; // toggled with X

    private Vector2 moveDir = Vector2.down; // default facing


    private enum MovementState { idleDown, idleUp, idleLeft, idleRight, walkDown, walkUp, walkLeft, walkRight, runDown, runUp, runLeft, runRight, cycleDown, cycleUp, cycleLeft, cycleRight }
    private MovementState state = MovementState.idleDown;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    bool IsBlocked(Vector3 targetPos)
    {
        return Physics2D.OverlapBox(
            targetPos,
            Vector2.one * 0.2f, // ungefär 1 tile
            0f,
            blockedLayer | interactableLayer
        );
    }

    void HandleRunToggle()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isRunning = !isRunning;
        }
    }

    void Movement()
    {
        Vector3 SnapToPixel(Vector3 pos)
        {
            float ppu = 16f;
            pos.x = Mathf.Round(pos.x * ppu) / ppu;
            pos.y = Mathf.Round(pos.y * ppu) / ppu;
            return pos;
        }

        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // no diagonal movement
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            input.y = 0;
        else
            input.x = 0;

        if (input != Vector2.zero)
        {
            moveDir = input;

            Vector3 targetPos = transform.position + new Vector3(
                input.x * tileSize,
                input.y * tileSize,
                0
            );

            if (IsBlocked(targetPos))
                return;

            StartCoroutine(Move(targetPos));
        }

        IEnumerator Move(Vector3 target)
        {
            isMoving = true;
            Vector3 start = transform.position;
            float t = 0f;

            // Choose speed based on run toggle
            float currentMoveTime = isRunning ? runMoveTime : walkMoveTime;

            while (t < 1f)
            {
                t += Time.deltaTime / currentMoveTime;
                Vector3 newPos = Vector3.Lerp(start, target, t);
                transform.position = SnapToPixel(newPos);
                yield return null;
            }

            transform.position = SnapToPixel(target);

            isMoving = false;

            CheckForEncounters();
        }
    }

    void Interact()
    {
        // Använd moveDir för att räkna ut vilken tile karaktären tittar på
        var facingDir = new Vector3(moveDir.x, moveDir.y, 0f);
        var interactPos = transform.position + facingDir;

        // Kolla om det finns något objekt på interactableLayer vid denna position
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    private void OnMoveOver()
    {
        CheckForEncounters();
        CheckIfInTrainersView();
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 150) <= 10)
            {
                UpdateAnimationState();
                OnEncountered();
            }
        }
    }

    private void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer);
        if (collider != null)
        {
            OnEnterTrainersView?.Invoke(collider);
        }
    }

    void UpdateAnimationState()
    {
        state = MovementState.idleDown;

        if (isMoving)
        {
            if (isRunning)
            {
                if (moveDir.x == 1) state = MovementState.runRight;
                else if (moveDir.x == -1) state = MovementState.runLeft;
                else if (moveDir.y == 1) state = MovementState.runUp;
                else if (moveDir.y == -1) state = MovementState.runDown;
            }
            else
            {
                if (moveDir.x == 1) state = MovementState.walkRight;
                else if (moveDir.x == -1) state = MovementState.walkLeft;
                else if (moveDir.y == 1) state = MovementState.walkUp;
                else if (moveDir.y == -1) state = MovementState.walkDown;
            }
        }
        else
        {
            if (moveDir.x == 1) state = MovementState.idleRight;
            else if (moveDir.x == -1) state = MovementState.idleLeft;
            else if (moveDir.y == 1) state = MovementState.idleUp;
            else if (moveDir.y == -1) state = MovementState.idleDown;
        }

        anim.SetInteger("State", (int)state);
    }

    public void HandleUpdate()
    {
        HandleRunToggle();

        if (isMoving) return;

        Movement();
        UpdateAnimationState();

        OnMoveOver();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    public void TriggerEncounter()
    {
        OnEncountered?.Invoke();
    }
}
