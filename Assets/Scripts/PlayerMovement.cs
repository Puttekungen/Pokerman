using UnityEngine;
using System.Collections;

public class GridPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    public float tileSize = 1f;
    public float moveTime = 0.1f; // how fast one step is

    private bool isMoving = false;


    private enum MovementState {idleDown, idleUp, idleLeft, idleRight, walkDown, walkUp, walkLeft, walkRight}
    private MovementState state = MovementState.idleDown;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    void Movement()
    {
        Vector3 SnapToPixel(Vector3 pos)
        {
            float ppu = 16f; // MUST match your tile sprite PPU
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
            Vector3 targetPos = transform.position + new Vector3(
                input.x * tileSize,
                input.y * tileSize,
                0
            );

            StartCoroutine(Move(targetPos));
        }

        IEnumerator Move(Vector3 target)
        {
            isMoving = true;
            Vector3 start = transform.position;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / moveTime;
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

        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
        Debug.Log(input);
        // gör så att if grejen kollar på det som är ovanför här
        if (rb.linearVelocity.x < -0.1f) { sprite.flipX = true; state = MovementState.walkLeft; }
        if (rb.linearVelocity.x > 0.1f) { sprite.flipX = false; state = MovementState.walkRight; }

        //if (rb.linearVelocity.y > 0.1f) state = MovementState.jumping;
        //if (rb.linearVelocity.y > 0.1f && jumps == 0) state = MovementState.doubleJumping;

        //if (rb.linearVelocity.y < -0.1f) state = MovementState.falling;
        

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
        if (isMoving) return;
        Movement();

        UpdateAnimationState();

        audioPlayer();
    }
}
