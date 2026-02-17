using UnityEngine;
using System.Collections;

public class GridPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask blockedLayer;

    private float tileSize = 1f;

    // Separate timings for walk vs run
    [SerializeField] private float walkMoveTime = 0.35f;
    [SerializeField] private float runMoveTime = 0.18f;

    private bool isMoving = false;
    private bool isRunning = false; // toggled with X

    private Vector2 moveDir = Vector2.down; // default facing


    private enum MovementState {idleDown, idleUp, idleLeft, idleRight, walkDown, walkUp, walkLeft, walkRight, runDown, runUp, runLeft, runRight, cycleDown, cycleUp, cycleLeft, cycleRight}
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
            blockedLayer
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

    void audioPlayer()
    {
        //if (state == MovementState.running && IsGrounded())
        //{
        //    if (!runningSound.isPlaying)
        //        runningSound.Play();
        //}
        //else
        //{
        //    if (runningSound.isPlaying)
        //        runningSound.Stop();
        //}

        //if (state == MovementState.falling)
        //{
        //    if (!fallingSound.isPlaying)
        //        fallingSound.Play();
        //}
        //else
        //{
        //    if (fallingSound.isPlaying)
        //        fallingSound.Stop();
        //}
    }

    void Update()
    {
        HandleRunToggle();

        if (isMoving) return;

        Movement();
        UpdateAnimationState();
        audioPlayer();
    }
}
