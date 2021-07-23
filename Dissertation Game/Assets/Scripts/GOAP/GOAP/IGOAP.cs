using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGOAP
{

	HashSet<KeyValuePair<string, object>> GetWorldState();

	HashSet<KeyValuePair<string, object>> CreateGoalState();

	void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

	void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions);

	void AllActionsFinished();

	void AbortPlan(GOAPAction aborter);

	bool MoveAgentToAction(GOAPAction nextAction);
}
