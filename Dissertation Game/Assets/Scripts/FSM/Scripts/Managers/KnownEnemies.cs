using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnownEnemies
{
    public List<Transform> enemyTransforms { get; set; }
    public List<Vector3> enemyPositions { get; set; }

    public KnownEnemies()
    {
        enemyTransforms = new List<Transform>();
        enemyPositions = new List<Vector3>();
    }

    public void AddEnemy(Transform transform)
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
