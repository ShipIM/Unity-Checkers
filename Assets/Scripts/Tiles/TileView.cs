using UnityEngine;

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
    private Color movingColor = Color.white;

    [SerializeField]
    private Color movingEnterColor = Color.white;

    [SerializeField]
    private Color attackColor = Color.white;

    [SerializeField]
    private Color attackEnterColor = Color.white;

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
            (MovingState, true) => movingEnterColor,
            (MovingState, _) => movingColor,
            (AttackState, true) => attackEnterColor,
            (AttackState, _) => attackColor,
            _ => spriteRenderer.color
        };
    }
}
