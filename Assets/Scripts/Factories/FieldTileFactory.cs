using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTileFactory : MonoBehaviour, Factory<FieldTile>
{
    [SerializeField]
    private FieldTile prefab;

    public FieldTile Create(Transform root)
    {
        return Object.Instantiate(prefab, root);
    }

    public FieldTile Create(Vector3 position, Transform root)
    {
        FieldTile tile = Create(root);
        var renderer = prefab.GetComponent<SpriteRenderer>();
        var size = renderer.bounds.size;

        Vector3 resultPosition = new(position.x * size.x / 2, position.y * size.y / 2);

        tile.transform.position = transform.position + resultPosition;
        tile.Initialize(new Vector2(resultPosition.x, resultPosition.y));

        return tile;
    }
}
