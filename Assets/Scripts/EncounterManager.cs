using UnityEngine;
using UnityEngine.Tilemaps;

public class EncounterZone : MonoBehaviour
{
    [SerializeField] private Tilemap encounterTilemap;
    [SerializeField] private TileBase encounterTile;

    public bool IsOnEncounterTile(Vector3 worldPosition)
    {
        Vector3Int cellPosition = encounterTilemap.WorldToCell(worldPosition);
        TileBase tile = encounterTilemap.GetTile(cellPosition);

        return tile == encounterTile;
    }
}
