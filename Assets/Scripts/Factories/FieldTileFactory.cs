using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTileFactory : MonoBehaviour, Factory<FieldTile>
{
    [SerializeField]
    private FieldTile white;

    [SerializeField]
    private FieldTile black;

    public FieldTile Create(Transform root, int type)
    {
        FieldTile prefab = type switch
        {
            0 => white,
            1 => black,
            _ => null,
        };

        return Object.Instantiate(prefab, root);
    }

    public FieldTile Create(Vector3 position, Vector2Int index, Transform root)
    {
        FieldTile tile = Create(root, (index.x + index.y) % 2 == 0 ? 1 : 0);
        var renderer = tile.GetComponent<SpriteRenderer>();
        var size = renderer.bounds.size;

        Vector3 resultPosition = new(position.x * size.x / 2, position.y * size.y / 2);

        tile.transform.position = transform.position + resultPosition;
        tile.Initialize(index);

        return tile;
    }
}
