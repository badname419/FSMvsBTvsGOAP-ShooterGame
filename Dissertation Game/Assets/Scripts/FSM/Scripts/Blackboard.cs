using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Blackboard")]
public class Blackboard : ScriptableObject
{
    public List<Transform> enemyTransforms = new List<Transform>();
    public List<Vector3> enemyPositions = new List<Vector3>();

    [HideInInspector] public void AddEnemy(Transform transform)
    {
        if (enemyTransforms.Count > 0)
        {
            for (int i = 0; i < enemyTransforms.Count; i++)
            {
                if (enemyTransforms[i].Equals(transform))
                {
                    enemyPositions[i] = transform.position;
                    break;
                }

                if (i == enemyTransforms.Count - 1)
                {
                    enemyTransforms.Add(transform);
                    enemyPositions.Add(transform.position);
                }
            }
        }
        else
        {
            enemyTransforms.Add(transform);
            enemyPositions.Add(transform.position);
        }
    }

}
