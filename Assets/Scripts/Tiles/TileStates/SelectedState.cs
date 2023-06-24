using UnityEngine.EventSystems;

public class SelectedState : FieldTileState
{
    public SelectedState(StateMachine stateMachine, FieldTile tile) : base(stateMachine, tile)
    {
        
    }

    public override void Enter()
    {
        tile.PointerClick += Unselect;
    }

    public override void Exit()
    {
        tile.PointerClick -= Unselect;
    }

    public override void Update()
    {
        
    }

    private void Unselect(PointerEventData eventData)
    {
        stateMachine.ChangeState(tile.DefaultState);
    }
}
