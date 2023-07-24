using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour, Factory<Unit>
{
    [SerializeField]
    private Unit blackKing;

    [SerializeField]
    private Unit whiteKing;

    [SerializeField]
    private Unit blackPiece;

    [SerializeField]
    private Unit whitePiece;

    public Unit Create(Transform root, int type)
    {
        Unit prefab = type switch
        {
            0 => blackKing,
            1 => whiteKing,
            2 => blackPiece,
            3 => whitePiece,
            _ => null
        };

        return Object.Instantiate(prefab, root);
    }

    public Unit Create(Transform root, int type, FieldTile tile)
    {
        Unit unit = Create(root, type);

        unit.Initialize(tile);

        return unit;
    }
}
