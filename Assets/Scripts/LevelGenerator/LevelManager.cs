using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private FieldTileFactory groundFactory;

    [SerializeField]
    private UnitFactory unitFactory;

    [SerializeField]
    private Vector2Int size;

    [SerializeField]
    private float spacing;
    public float Spacing => spacing;

    [SerializeField]
    private List<FieldTile> listedTiles;
    public List<FieldTile> ListedTiles => listedTiles;

    [SerializeField]
    private List<Unit> listedUnits;
    public List<Unit> ListedUnits => listedUnits;

    [ContextMenu("Create")]
    public void Create()
    {
        Clear();

        listedTiles = new();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 position = new((i + j) * spacing, (i - j) * spacing);

                FieldTile tile = groundFactory.Create(position, new Vector2Int(i, j), transform);

                listedTiles.Add(tile);
            }
        }

        SetUpPieces();
    }

    private void SetUpPieces()
    {
        int absolute = 0, xPos, yPos, addition,
            rows = 3 * size.x / 8,
            missing = size.y % 2 == 0 ? 0 : rows / 2,
            amount =  rows * (int) ((size.y + 1) / 2) - missing;

        for (int i = 0; i < amount; i++)
        {
            xPos = absolute / size.y;
            addition = (xPos % 2 == 0) || size.y % 2 != 0 ? 0 : 1;
            yPos = absolute % size.y + addition;

            foreach (FieldTile tile in listedTiles)
            {
                if (tile.Position.x == xPos && tile.Position.y == yPos)
                {
                    Unit unit = unitFactory.Create(transform, 3, tile);

                    listedUnits.Add(unit);
                }

                if (tile.Position.x == size.x - 1 - xPos && tile.Position.y == size.y - 1 - yPos)
                {
                    Unit unit = unitFactory.Create(transform, 2, tile);

                    listedUnits.Add(unit);
                }
            }

            absolute += 2;
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        if (listedTiles != null)
        {
            for (int i = 0; i < listedTiles.Count;)
            {
                if (listedTiles[i].gameObject.activeInHierarchy)
                {
                    DestroyImmediate(listedTiles[i].gameObject);
                    listedTiles.RemoveAt(i);
                }
            }
        }

        if (listedUnits != null)
        {
            for (int i = 0; i < listedUnits.Count;)
            {
                if (listedUnits[i].gameObject.activeInHierarchy)
                {
                    DestroyImmediate(listedUnits[i].gameObject);
                    listedUnits.RemoveAt(i);
                }
            }
        }
    }
}
