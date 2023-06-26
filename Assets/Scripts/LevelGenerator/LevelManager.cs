using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private FieldTileFactory groundFactory;

    [SerializeField]
    private Vector2Int size;

    [SerializeField]
    private float spacing;

    [SerializeField]
    private FieldTile[,] indexedTiles;

    [SerializeField]
    private List<FieldTile> listedTiles;

    [ContextMenu("Create")]
    public void Create()
    {
        this.Clear();

        indexedTiles = new FieldTile[size.x, size.y];
        listedTiles = new();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 position = new((i + j) * spacing, (i - j) * spacing);

                FieldTile tile = groundFactory.Create(position, transform);

                indexedTiles[i, j] = tile;
                listedTiles.Add(tile);
            }
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
        
        this.indexedTiles = null;
    }
}
