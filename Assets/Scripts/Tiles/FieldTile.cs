using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldTile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Vector2 position;
    public Vector2 Position => position;

    [SerializeField]
    private Unit unit;

    public void Initialize(Vector2 position)
    {
        this.position = position;
    }

    [SerializeField]
    private GameObject _gameObject;

    [SerializeField]
    private Transform _transform;

    private StateMachine stateMachine;
    public State CurrentState => stateMachine.CurrentState;

    private DefaultState defaultState;
    public DefaultState DefaultState => defaultState;

    private SelectedState selectedState;
    public SelectedState SelectedState => selectedState;

    private bool pointerEnter;
    public bool PointerEnter => pointerEnter;

    private bool haveUnit;
    public bool HaveUnit => haveUnit;

    public delegate void StateHandler(State currentState, State oldState, FieldTile sender);
    public event StateHandler StateChanged;

    public delegate void PointerStateHandler(bool pointerEnter, FieldTile sender);
    public event PointerStateHandler PointerChanged;

    public delegate void PointerHandler(PointerEventData eventData);
    public event PointerHandler PointerClick;

    public void Unselect()
    {
        if (CurrentState is SelectedState)
            stateMachine.ChangeState(defaultState);
    }

    public void SetUnit(Unit unit = null)
    {
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

    private void Awake()
    {
        stateMachine = new();

        stateMachine.OnStateChanged += (state, oldState) => StateChanged?.Invoke(state, oldState, this);

        defaultState = new(stateMachine, this);
        selectedState = new(stateMachine, this);

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
