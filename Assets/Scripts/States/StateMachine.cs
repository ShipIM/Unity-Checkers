public class StateMachine
{
    public delegate void StateHandler(State state, State oldState);
    public event StateHandler OnStateChanged;

    private State currentState;
    public State CurrentState => currentState;

    private State oldState;
    public State OldState => oldState;

    public void Initialize(State startState)
    {
        currentState = startState;
        startState.Enter();
    }

    public void ChangeState(State state)
    {
        oldState = currentState;
        currentState.Exit();

        currentState = state;
        state.Enter();

        OnStateChanged?.Invoke(CurrentState, OldState);
    }
}
