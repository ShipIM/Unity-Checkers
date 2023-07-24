using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Unit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Vector2Int[] moves;
    public Vector2Int[] Moves => moves;

    [SerializeField]
    private Vector2Int[] attackMoves;
    public Vector2Int[] AttackMoves => attackMoves;

    [SerializeField]
    private FieldTile tile;
    public FieldTile Tile => tile;

    [SerializeField]
    private GameObject _gameObject;
    public GameObject GameObject => _gameObject;

    [SerializeField]
    private Transform _transform;
    public Transform Transform => _transform;

    /* private void Start()
    {
        FindTile();
    }

    public void FindTile()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_transform.position, _transform.localScale, 0);

        if (colliders.Length == 0)
            return;

        Debug.Log(colliders.Length);

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
    } */

    public void Initialize(FieldTile tile)
    {
        if (this.tile != null)
            this.tile.SetUnit();

        _transform.position = tile.transform.position;
        tile.SetUnit(this);
        this.tile = tile;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tile.OnPointerClick(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tile.OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tile.OnPointerExit(eventData);
    }
}
