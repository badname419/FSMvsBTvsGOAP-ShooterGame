using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Search")]
public class SearchingAction : Action
{
    public override void Act(StateController controller)
    {
        Search(controller);
    }

    private void Search(StateController controller)
    {
        
    }
}