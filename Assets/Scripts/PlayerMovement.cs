using UnityEngine;
using System.Collections;

public class GridPlayerMovement : MonoBehaviour
{
    public float tileSize = 1f;
    public float moveTime = 0.15f; // how fast one step is

    private bool isMoving = false;

    void Update()
    {
        if (isMoving) return;

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
    }

    IEnumerator Move(Vector3 target)
    {
        isMoving = true;
        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
}
