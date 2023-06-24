using UnityEngine.EventSystems;

public class DefaultState : FieldTileState
{
    public DefaultState(StateMachine stateMachine, FieldTile tile) : base(stateMachine, tile)
    {
        
    }

    public override void Enter()
    {
        tile.PointerClick += Select;
    }

    public override void Exit()
    {
        tile.PointerClick -= Select;
    }

    public override void Update()
    {

    }

    private void Select(PointerEventData eventData)
    {
        stateMachine.ChangeState(tile.SelectedState);
    }
}
