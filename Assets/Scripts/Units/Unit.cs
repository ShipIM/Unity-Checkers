using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Vector2Int[] moves;

    [SerializeField]
    private Vector2Int[] attackMoves;

    [SerializeField]
    private FieldTile tile;

    [SerializeField]
    private Color moveColor = Color.white;

    [SerializeField]
    private Color attackColor = Color.white;

    [SerializeField]
    private Color universalColor = Color.white;

    [SerializeField]
    private float spacing;

    [SerializeField]
    private GameObject _gameObject;

    [SerializeField]
    private Transform _transform;
    public Transform Transform => _transform;

    public Vector2Int[] Moves => moves;
    public Vector2Int[] AttackMover => attackMoves;

    public void FindTile()
    {
        if (this.tile != null)
            this.tile.SetUnit(null);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(_transform.position, _transform.localScale, 0);

        if (colliders.Length == 0)
            return;

        FieldTile tile = null;
        colliders.First(collider => {
            tile = collider.GetComponent<FieldTile>();
            return tile != null;
        });

        if (tile != null)
        {
            tile.SetUnit(this);
            this.tile = tile;
        }
    }
}
