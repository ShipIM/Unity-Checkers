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
            tile.ClickAttacking += Attack;
        }
    }

    private void OnDisable()
    {
        foreach(FieldTile tile in levelManager.ListedTiles)
        {
            tile.StateChanged -= TrySetSelected;
            tile.StateChanged -= TryRenderMoves;

            tile.ClickMoving -= Move;
            tile.ClickAttacking -= Attack;
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
            FieldTile movingTile = FindMoveTile(unitPosition, move);

            if (movingTile != null)
            {
                movingTile.SetMoving();
                tiles.Add(movingTile);
            }
        }

        foreach (Vector2Int attackMove in attackMoves)
        {
            FieldTile attackTile = FindAttackTile(unitPosition, attackMove);

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

    private FieldTile FindAttackTile(Vector2Int unitPosition, Vector2Int move)
    {
        List<FieldTile> affected = FindLine(unitPosition, move);

        int enemies = 0;
        foreach (FieldTile tile in affected)
        {
            if (tile == null) return null;

            if (tile.HaveUnit) enemies++;
        }

        if (affected.Count < 2 || affected[^1] == null || affected[^1].HaveUnit || enemies > 1 || enemies == 0)
            return null;
        
        return affected[^1];
    }

    private List<FieldTile> FindLine(Vector2Int unitPosition, Vector2Int move)
    {
        List<FieldTile> affected = new();

        Vector2Int step = move / Math.Max(Math.Abs(move.x), Math.Abs(move.y));

        for (int i = 1; step * i != move; i++)
            affected.Add(levelManager.ListedTiles.FirstOrDefault(tile => tile.Position == step * i + unitPosition));

        affected.Add(levelManager.ListedTiles.FirstOrDefault(tile => tile.Position == move + unitPosition));

        return affected;
    }

    private FieldTile FindMoveTile(Vector2Int unitPosition, Vector2Int move)
    {
        List<FieldTile> affected = FindLine(unitPosition, move);

        bool isFree = true;
        foreach (FieldTile tile in affected)
            isFree = isFree && tile != null && !tile.HaveUnit;

        return isFree ? affected[^1] : null;
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

    private void Attack(FieldTile tile)
    {
        Vector2Int current = unit.Tile.Position;

        List<FieldTile> affected = FindLine(current, tile.Position - current);
        affected.ForEach((tile) => tile.SetUnit(true));

        Move(tile);
    }
}
