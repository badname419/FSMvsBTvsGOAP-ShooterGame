
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class GOAPAgent : MonoBehaviour
{

	private FSM stateMachine;
	private FSM.FSMState idleState;
	private FSM.FSMState moveToState;
	private FSM.FSMState performActionState;

	private HashSet<GOAPAction> availableActions;
	private Queue<GOAPAction> currentActions;
	private IGOAP dataProvider;
	private GOAPPlanner planner;

	public float timer;


	// Use this for initialization
	void Start()
	{
		stateMachine = new FSM();
		availableActions = new HashSet<GOAPAction>();
		currentActions = new Queue<GOAPAction>();
		planner = new GOAPPlanner();

		FindDataProviderInterface();
		CreateIdleState();
		CreateMovingState();
		CreatePerformActionState();

		stateMachine.PushState(idleState);
		LoadAvailableActions();

		timer = 0f;
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		stateMachine.Update(this.gameObject);
	}

	public void AddAvialableAction(GOAPAction action)
	{
		availableActions.Add(action);
	}

	public GOAPAction getAction(Type action)
	{
		foreach (GOAPAction currAction in availableActions)
		{
			if (currAction.GetType().Equals(action))
			{
				return currAction;
			}
		}

		return null;
	}

	public void RemoveAvailableAction(GOAPAction action)
	{
		availableActions.Remove(action);
	}

	private bool HasActionPlan()
	{
		return currentActions.Count > 0;
	}

	private void CreateIdleState()
	{
		Debug.Log("Idle");
		idleState = (fsm, obj) => {

			HashSet<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState();
			HashSet<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState();

			Queue<GOAPAction> plan = planner.CreateActionPlan(gameObject, availableActions, worldState, goal);
			if (plan != null)
			{
				currentActions = plan;
				dataProvider.PlanFound(goal, plan);

				fsm.PopState();
				fsm.PushState(performActionState);
			}
			else
			{
				dataProvider.PlanFailed(goal);
				fsm.PopState();
				fsm.PushState(idleState);
			}
		};
	}

	private void CreateMovingState()
	{
		Debug.Log("Move");
		moveToState = (fsm, gameObject) => {

			GOAPAction action = currentActions.Peek();
			if (action.NeedsToBeInRange() && action.target == null)
			{
				fsm.PopState();
				fsm.PopState();
				fsm.PushState(idleState);
				return;
			}

			if (dataProvider.MoveAgentToAction(action))
			{
				fsm.PopState();
			}

		};
	}

	private void CreatePerformActionState()
	{
		Debug.Log("Perform");
		performActionState = (fsm, obj) => {

			if (!HasActionPlan())
			{
				fsm.PopState();
				fsm.PushState(idleState);
				dataProvider.AllActionsFinished();
				return;
			}

			GOAPAction action = currentActions.Peek();
			if (action.IsActionFinished())
			{
				currentActions.Dequeue();
			}

			if (HasActionPlan())
			{
				Debug.Log("Plan");
				action = currentActions.Peek();
				bool inRange = action.NeedsToBeInRange() ? action.IsAgentInRange() : true;

				//Debug.Log(inRange);

				if (inRange)
				{
					Debug.Log("Range");
					bool actionSuccess = action.PerformAction(obj);
					if (!actionSuccess)
					{
						fsm.PopState();
						fsm.PushState(idleState);
						CreateIdleState();
						dataProvider.AbortPlan(action);
					}
				}
				else
				{
					Debug.Log("not range");
					fsm.PushState(moveToState);
				}
			}
			else
			{
				fsm.PopState();
				fsm.PushState(idleState);
				dataProvider.AllActionsFinished();
			}
		};
	}

	private void FindDataProviderInterface()
	{
		foreach (Component comp in gameObject.GetComponents(typeof(Component)))
		{
			if (typeof(IGOAP).IsAssignableFrom(comp.GetType()))
			{
				dataProvider = (IGOAP)comp;
				return;
			}
		}
	}

	private void LoadAvailableActions()
	{
		GOAPAction[] actions = gameObject.GetComponents<GOAPAction>();
		foreach (GOAPAction a in actions)
		{
			availableActions.Add(a);
		}
	}

	public IGOAP GetDataProviderInterface()
	{
		return dataProvider;
	}
}
