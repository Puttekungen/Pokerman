using UnityEngine;

public class Grass : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite normalGrass;
    public Sprite steppedGrass;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalGrass;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = steppedGrass;
            // Om du vill kan du också lägga till encounter-chans här
            // TryEncounter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = normalGrass;
        }
    }

    // Exempel på slumpad encounter
    void TryEncounter()
    {
        if (Random.Range(0, 100) < 10) // 10% chans
        {
            Debug.Log("Wild Pokémon appeared!");
        }
    }
}
