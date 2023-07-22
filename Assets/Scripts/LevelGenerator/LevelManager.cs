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
    public float Spacing => spacing;

    [SerializeField]
    private List<FieldTile> listedTiles;
    public List<FieldTile> ListedTiles => listedTiles;

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
    }
}
