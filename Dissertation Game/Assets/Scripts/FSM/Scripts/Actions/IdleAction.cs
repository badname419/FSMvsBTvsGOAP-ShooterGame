using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Idle")]
public class IdleAction: Action
{
    // Start is called before the first frame update
    public override void Act(StateController controller)
    {
        DoNothing();
    }

    private void DoNothing()
    {
        Debug.Log("IDLE");
    }
}
