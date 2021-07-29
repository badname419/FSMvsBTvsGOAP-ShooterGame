using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decisions/RotationsDone")]
public class RotationsDoneDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool rotationsDone = CheckIfRotationsDone(controller);
        return rotationsDone;
    }

    private bool CheckIfRotationsDone(StateController controller)
    {
        if(controller.enemyThinker.numOfRotations < controller.enemyThinker.totalRotations)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}