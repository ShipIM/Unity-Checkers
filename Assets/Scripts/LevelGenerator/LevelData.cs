using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    private FieldTile[] tiles;
    private FieldTile selectedTile;

    private bool haveSelectedTile;

    private void Awake()
    {
        FieldTile[] tiles = FindObjectsOfType<FieldTile>();

        if (tiles.Length == 0) throw new NullReferenceException("Tiles not found");

        this.tiles = tiles;

        foreach(FieldTile tile in tiles)
        {
            tile.StateChanged += TrySetSelected;
        }
    }

    private void OnDisable()
    {
        foreach(FieldTile tile in tiles)
        {
            tile.StateChanged -= TrySetSelected;
        }
    }

    private void TrySetSelected(State state, State oldState, FieldTile tile)
    {
        if (state is SelectedState && oldState is DefaultState)
        {
            if (haveSelectedTile)
            {
                selectedTile.Unselect();
            }

            selectedTile = tile;

            haveSelectedTile = true;
        }

        if (state is DefaultState && oldState is SelectedState)
        {
            selectedTile.Unselect();

            selectedTile = null;
            haveSelectedTile = false;
        }
    }
}
