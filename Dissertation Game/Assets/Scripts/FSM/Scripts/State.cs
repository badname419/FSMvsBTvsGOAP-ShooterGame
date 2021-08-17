using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class State : ScriptableObject
{
    public Action[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
            //controller.enemyThinker.logWriting.AddLine(actions[i].name.ToString());
        }
    }

    private void CheckTransitions(StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = true;

            for (int j = 0; j < transitions[i].decision.Length; j++)
            {
                decisionSucceeded = transitions[i].decision[j].Decide(controller);
                //controller.enemyThinker.logWriting.AddLine(transitions[i].decision[j].name.ToString() + " " + decisionSucceeded);
                if (!decisionSucceeded)
                {
                    break;
                }
            }

            if (decisionSucceeded)
            {
                if (transitions[i].trueState != null)
                {
                    controller.TransitionToState(transitions[i].trueState);
                }
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }

            if ((decisionSucceeded && transitions[i].trueState != null) 
                || (!decisionSucceeded && transitions[i].trueState == null && transitions[i].falseState != null))
            {
                break;
            }
        }
    }


}