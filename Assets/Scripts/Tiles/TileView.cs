using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileView : MonoBehaviour
{
    [SerializeField]
    private Color defaultColor = Color.white;

    [SerializeField]
    private Color enterColor = Color.white;

    [SerializeField]
    private Color selectColor = Color.white;

    [SerializeField]
    private Color selectEnterColor = Color.white;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private FieldTile tile;

    private void OnEnable()
    {
        tile.StateChanged += (state, oldState, sender) => SetColorByState(state, sender.PointerEnter);
        tile.PointerChanged += (pointerEnter, sender) => SetColorByState(sender.CurrentState, pointerEnter);
    }

    private void OnDisable()
    {
        tile.StateChanged -= (state, oldState, sender) => SetColorByState(state, sender.PointerEnter);
        tile.PointerChanged -= (pointerEnter, sender) => SetColorByState(sender.CurrentState, pointerEnter);
    }

    private void SetColorByState(State tileState, bool pointerEnter)
    {
        spriteRenderer.color = (tileState, pointerEnter) switch
        {
            (DefaultState, true) => enterColor,
            (DefaultState, _) => defaultColor,
            (SelectedState, true) => selectEnterColor,
            (SelectedState, _) => selectColor,
            _ => spriteRenderer.color
        };
    }
}
