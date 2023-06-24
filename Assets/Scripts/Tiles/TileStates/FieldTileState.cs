
public abstract class FieldTileState : State
{
    protected readonly StateMachine stateMachine;
    protected readonly FieldTile tile;

    public FieldTileState(StateMachine stateMachine, FieldTile tile)
    {
        this.stateMachine = stateMachine;
        this.tile = tile;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();
}
