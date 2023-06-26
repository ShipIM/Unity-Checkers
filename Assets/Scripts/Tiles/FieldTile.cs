using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldTile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Vector2 position;
    public Vector2 Position => position;

    [SerializeField]
    private Unit unit;
    public Unit Unit => unit;

    public void Initialize(Vector2 position)
    {
        this.position = position;
    }

    [SerializeField]
    private GameObject _gameObject;

    [SerializeField]
    private Transform _transform;
    public Transform Transform => _transform;

    private StateMachine stateMachine;
    public State CurrentState => stateMachine.CurrentState;

    private DefaultState defaultState;
    public DefaultState DefaultState => defaultState;

    private SelectedState selectedState;
    public SelectedState SelectedState => selectedState;

    private MovingState movingState;
    public MovingState MovingState => movingState;

    private AttackState attackState;
    public AttackState AttackState => attackState;

    private bool pointerEnter;
    public bool PointerEnter => pointerEnter;

    [SerializeField]
    private bool haveUnit;
    public bool HaveUnit => haveUnit;

    public delegate void StateHandler(State currentState, State oldState, FieldTile sender);
    public event StateHandler StateChanged;

    public delegate void PointerStateHandler(bool pointerEnter, FieldTile sender);
    public event PointerStateHandler PointerChanged;

    public delegate void PointerHandler(PointerEventData eventData);
    public event PointerHandler PointerClick;

    public Action<FieldTile> ClickMoving;

    public void Unselect()
    {
        if (CurrentState is SelectedState)
            stateMachine.ChangeState(defaultState);
    }

    public void SetUnit(Unit unit = null)
    {
        if (this.unit != null && unit != null && unit != this.unit)
        {
            Destroy(this.unit.GameObject);
        }

        if (unit == null)
        {
            this.unit = null;
            haveUnit = false;
        }
        else
        {
            unit.Transform.position = new Vector3(_transform.position.x, _transform.position.y);

            this.unit = unit;
            haveUnit = true;
        }
    }

    public void SetDefault()
    {
        stateMachine.ChangeState(defaultState);
    }

    public void SetMoving()
    {
        stateMachine.ChangeState(movingState);
    }

    public void SetAttack()
    {
        stateMachine.ChangeState(attackState);
    }

    private void Awake()
    {
        stateMachine = new();

        stateMachine.OnStateChanged += (state, oldState) => StateChanged?.Invoke(state, oldState, this);

        defaultState = new(stateMachine, this);
        selectedState = new(stateMachine, this);
        movingState = new(stateMachine, this);
        attackState = new(stateMachine, this);

        stateMachine.Initialize(defaultState);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerClick?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerEnter = true;

        PointerChanged?.Invoke(pointerEnter, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEnter = false;

        PointerChanged?.Invoke(pointerEnter, this);
    }
}
