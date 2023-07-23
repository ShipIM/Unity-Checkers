using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelData : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;

    private FieldTile selectedTile;

    private List<FieldTile> movingTiles;
    private Unit unit;

    private bool haveSelectedTile;

    private void Awake()
    {
        foreach(FieldTile tile in levelManager.ListedTiles)
        {
            tile.StateChanged += TrySetSelected;
            tile.StateChanged += TryRenderMoves;

            tile.ClickMoving += Move;
        }
    }

    private void OnDisable()
    {
        foreach(FieldTile tile in levelManager.ListedTiles)
        {
            tile.StateChanged -= TrySetSelected;
            tile.StateChanged -= TryRenderMoves;

            tile.ClickMoving -= Move;
        }
    }

    private void TrySetSelected(State state, State oldState, FieldTile tile)
    {
        if (state is SelectedState && oldState is DefaultState)
        {
            if (haveSelectedTile)
            {
                selectedTile.Unselect();
                ClearMoving();
            }

            selectedTile = tile;
            haveSelectedTile = true;
        }

        if (state is DefaultState && oldState is SelectedState)
        {
            selectedTile.Unselect();
            ClearMoving();

            selectedTile = null;
            haveSelectedTile = false;
        }
    }

    private void TryRenderMoves(State state, State oldState, FieldTile tile)
    {
        if (state is SelectedState && oldState is DefaultState && tile.HaveUnit)
        {
            movingTiles = SetMovingTiles(tile.Position, tile.Unit.Moves, tile.Unit.AttackMoves);
            unit = tile.Unit;
        }
    }

    private List<FieldTile> SetMovingTiles(Vector2Int unitPosition, Vector2Int[] moves, Vector2Int[] attackMoves)
    {
        List<FieldTile> tiles = new();

        foreach(Vector2Int move in moves)
        {
            FieldTile movingTile = FindTile(unitPosition, move, false);

            if (movingTile != null)
            {
                movingTile.SetMoving();
                tiles.Add(movingTile);
            }
        }

        foreach (Vector2Int attackMove in attackMoves)
        {
            FieldTile attackTile = FindTile(unitPosition, attackMove, true);

            if (attackTile != null)
            {
                FieldTile movingTile = tiles.FirstOrDefault(tile => attackTile == tile);

                if (movingTile != null)
                {
                    movingTile.SetAttack();
                }
                else
                {
                    attackTile.SetAttack();
                    tiles.Add(attackTile);
                }
            }
        }

        return tiles;
    }

    private FieldTile FindTile(Vector2Int unitPosition, Vector2Int move, bool haveUnit)
    {
        return levelManager.ListedTiles.FirstOrDefault(tile => tile.Position == move + unitPosition && tile.HaveUnit == haveUnit);
    }

    private void ClearMoving()
    {
        if (movingTiles != null)
        {
            movingTiles.ForEach(tile => tile.SetDefault());
            movingTiles.Clear();
        }
    }

    private void Move(FieldTile tile)
    {
        unit.Initialize(tile);

        selectedTile.Unselect();
    }
}
