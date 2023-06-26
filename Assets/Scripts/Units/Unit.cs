using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Vector2[] moves;
    public Vector2[] Moves => moves;

    [SerializeField]
    private Vector2[] attackMoves;
    public Vector2[] AttackMoves => attackMoves;

    [SerializeField]
    private FieldTile tile;
    public FieldTile Tile => tile;

    [SerializeField]
    private float spacing;

    [SerializeField]
    private GameObject _gameObject;
    public GameObject GameObject => _gameObject;

    [SerializeField]
    private Transform _transform;
    public Transform Transform => _transform;

    private void Start()
    {
        FindTile();
    }

    public void FindTile()
    {
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

    public void Initialize(FieldTile tile)
    {
        this.tile.SetUnit();

        _transform.position = tile.transform.position;
        tile.SetUnit(this);
        this.tile = tile;
    }
}
