using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelData : MonoBehaviour
{
    private FieldTile[] tiles;
    private FieldTile selectedTile;

    private List<FieldTile> movingTiles;
    private Unit unit;

    private bool haveSelectedTile;

    private void Awake()
    {
        FieldTile[] tiles = FindObjectsOfType<FieldTile>();

        if (tiles.Length == 0) throw new NullReferenceException("Tiles not found");

        this.tiles = tiles;

        foreach(FieldTile tile in tiles)
        {
            tile.StateChanged += TrySetSelected;
            tile.StateChanged += TryRenderMoves;
        }
    }

    private void OnDisable()
    {
        foreach(FieldTile tile in tiles)
        {
            tile.StateChanged -= TrySetSelected;
            tile.StateChanged -= TryRenderMoves;
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

    private List<FieldTile> SetMovingTiles(Vector2 unitPosition, Vector2[] moves, Vector2[] attackMoves)
    {
        List<FieldTile> tiles = new();

        foreach(Vector2 move in moves)
        {
            FieldTile movingTile = FindTile(unitPosition, move, false);

            if (movingTile != null)
            {
                print("selected");
                movingTile.SetMoving();
                tiles.Add(movingTile);
            }
        }

        foreach (Vector2 attackMove in attackMoves)
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

    private FieldTile FindTile(Vector2 unitPosition, Vector2 move, bool haveUnit)
    {
        return tiles.FirstOrDefault(tile => tile.Position == move + unitPosition && tile.HaveUnit == haveUnit);
    }

    private void ClearMoving()
    {
        if (movingTiles != null)
        {
            movingTiles.ForEach(tile => tile.SetDefault());
            movingTiles.Clear();
        }
    }
}
