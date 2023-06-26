using UnityEngine.EventSystems;

public class MovingState : FieldTileState
{
    public MovingState(StateMachine stateMachine, FieldTile tile) : base(stateMachine, tile)
    {

    }

    public override void Enter()
    {
        this.tile.PointerClick += Move;
    }

    public override void Exit()
    {
        this.tile.PointerClick -= Move;
    }

    public override void Update()
    {
        
    }

    private void Move(PointerEventData eventData)
    {
        this.tile.ClickMoving?.Invoke(this.tile);
    }
}
