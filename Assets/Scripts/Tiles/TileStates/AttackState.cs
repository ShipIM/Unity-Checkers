using UnityEngine.EventSystems;

public class AttackState : FieldTileState
{
    public AttackState(StateMachine stateMachine, FieldTile tile) : base(stateMachine, tile)
    {

    }

    public override void Enter()
    {
        this.tile.PointerClick += Attack;
    }

    public override void Exit()
    {
        this.tile.PointerClick -= Attack;
    }

    public override void Update()
    {
        
    }

    private void Attack(PointerEventData eventData)
    {
        this.tile.ClickMoving?.Invoke(this.tile);
    }
}
