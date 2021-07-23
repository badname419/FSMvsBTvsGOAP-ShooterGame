using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Plans what actions can be completed in order to fulfill a goal state.
 */
public class GOAPPlanner
{

	/**
	 * Plan what sequence of actions can fulfill the goal.
	 * Returns null if a plan could not be found, or a list of the actions
	 * that must be performed, in order, to fulfill the goal.
	 */
	public Queue<GOAPAction> CreateActionPlan(GameObject agent, HashSet<GOAPAction> availableActions,
								  HashSet<KeyValuePair<string, object>> worldState,
								  HashSet<KeyValuePair<string, object>> goal)
	{
		// reset the actions so we can start fresh with them
		foreach (GOAPAction action in availableActions)
		{
			action.ResetAction();
		}

		// check what actions can run using their checkProceduralPrecondition
		HashSet<GOAPAction> usableActions = new HashSet<GOAPAction>();
		foreach (GOAPAction action in availableActions)
		{
			if (action.CheckPrecondition(agent))
				usableActions.Add(action);
		}

		// we now have all actions that can run, stored in usableActions

		// build up the tree and record the leaf nodes that provide a solution to the goal.
		List<Node> leaves = new List<Node>();

		// build graph
		Node startNode = new Node(null, 0, worldState, null);
		bool success = BuildGOAPPlan(startNode, leaves, usableActions, goal);

		if (!success)
		{
			// oh no, we didn't get a plan
			//Debug.Log("NO PLAN");
			return null;
		}

		//Find the cheapest plan of action out of each generated plan
		Node cheapestPlan = null;
		foreach (Node node in leaves)
		{
			if (cheapestPlan == null)
				cheapestPlan = node;
			else
			{
				if (node.runningCost < cheapestPlan.runningCost)
					cheapestPlan = node;
			}
		}

		// get its node and work back through the parents
		List<GOAPAction> finishedPlan = new List<GOAPAction>();
		Node cheapestNodeClone = cheapestPlan;
		while (cheapestNodeClone != null)
		{
			if (cheapestNodeClone.action != null)
			{
				finishedPlan.Insert(0, cheapestNodeClone.action); // insert the action in the front
			}
			cheapestNodeClone = cheapestNodeClone.parent;
		}
		// we now have this action list in correct order

		Queue<GOAPAction> actionQueue = new Queue<GOAPAction>();
		foreach (GOAPAction action in finishedPlan)
		{
			actionQueue.Enqueue(action);
		}

		// hooray we have a plan!
		return actionQueue;
	}

	/**
	 * Returns true if at least one solution was found.
	 * The possible paths are stored in the leaves list. Each leaf has a
	 * 'runningCost' value where the lowest cost will be the best action
	 * sequence.
	 */
	protected bool BuildGOAPPlan(Node parent, List<Node> leaves, HashSet<GOAPAction> usableActions, HashSet<KeyValuePair<string, object>> goal)
	{
		bool foundSuccessfulPath = false;

		// go through each action available at this node and see if we can use it here
		foreach (GOAPAction action in usableActions)
		{

			// if the parent state has the conditions for this action's preconditions, we can use it here
			if (IsInState(action.Preconditions, parent.state))
			{

				// apply the action's effects to the parent state
				HashSet<KeyValuePair<string, object>> currentState = PopulateState(parent.state, action.Effects);
				//Debug.Log(GoapAgent.prettyPrint(currentState));
				Node newNode = new Node(parent, parent.runningCost + action.cost, currentState, action);

				if (GoalInState(goal, currentState))
				{
					// we found a solution!
					leaves.Add(newNode);
					foundSuccessfulPath = true;
				}
				else
				{
					// test all the remaining actions and branch out the tree
					HashSet<GOAPAction> subset = ActionSubset(usableActions, action);
					bool foundSecondPath = BuildGOAPPlan(newNode, leaves, subset, goal);
					if (foundSecondPath)
						foundSuccessfulPath = true;
				}


			}
		}

		return foundSuccessfulPath;
	}

	/**
	 * Create a subset of the actions excluding the removeMe one. Creates a new set.
	 */
	protected HashSet<GOAPAction> ActionSubset(HashSet<GOAPAction> actions, GOAPAction removeMe)
	{
		HashSet<GOAPAction> subset = new HashSet<GOAPAction>();
		foreach (GOAPAction action in actions)
		{
			if (!action.Equals(removeMe))
				subset.Add(action);
		}
		return subset;
	}

	/*
	 * Checks if at least one goal is met. 
	 * to-do: Create a system for weighting towards paths that fulfill more goals
	 */
	protected bool GoalInState(HashSet<KeyValuePair<string, object>> test, HashSet<KeyValuePair<string, object>> state)
	{
		bool match = false;
		foreach (KeyValuePair<string, object> t in test)
		{
			foreach (KeyValuePair<string, object> s in state)
			{
				if (s.Equals(t))
				{
					match = true;
					break;
				}
			}
		}
		return match;
	}

	/*
	 * Check that all items in 'test' are in 'state'. If just one does not match or is not there
	 * then this returns false.
	 */
	protected bool IsInState(HashSet<KeyValuePair<string, object>> test, HashSet<KeyValuePair<string, object>> state)
	{
		bool allMatch = true;
		foreach (KeyValuePair<string, object> t in test)
		{
			bool match = false;
			foreach (KeyValuePair<string, object> s in state)
			{
				if (s.Equals(t))
				{
					match = true;
					break;
				}
			}
			if (!match)
				allMatch = false;
		}
		return allMatch;
	}

	/**
	 * Apply the stateChange to the currentState
	 */
	protected HashSet<KeyValuePair<string, object>> PopulateState(HashSet<KeyValuePair<string, object>> currentState, HashSet<KeyValuePair<string, object>> stateChange)
	{
		HashSet<KeyValuePair<string, object>> state = new HashSet<KeyValuePair<string, object>>();
		// copy the KVPs over as new objects
		foreach (KeyValuePair<string, object> s in currentState)
		{
			state.Add(new KeyValuePair<string, object>(s.Key, s.Value));
		}

		foreach (KeyValuePair<string, object> change in stateChange)
		{
			// if the key exists in the current state, update the Value
			bool exists = false;

			foreach (KeyValuePair<string, object> s in state)
			{
				if (s.Key.Equals(change.Key))
				{
					exists = true;
					break;
				}
			}

			if (exists)
			{
				state.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(change.Key); });
				KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key, change.Value);
				state.Add(updated);
			}
			// if it does not exist in the current state, add it
			else
			{
				state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
			}
		}
		return state;
	}

	/**
	 * Used for building up the graph and holding the running costs of actions.
	 */
	protected class Node
	{
		public Node parent;
		public float runningCost;
		public HashSet<KeyValuePair<string, object>> state;
		public GOAPAction action;

		public Node(Node parent, float runningCost, HashSet<KeyValuePair<string, object>> state, GOAPAction action)
		{
			this.parent = parent;
			this.runningCost = runningCost;
			this.state = state;
			this.action = action;
		}
	}

}

