using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    /*private float viewRadius;
    [Range(0, 360)]
    private float viewAngle;

    private LayerMask enemyMask;
    private LayerMask coverMask;

    private List<Transform> visibleEnemies = new List<Transform>();
    private GameObject ai;*/

    /*private void Start()
    {
        //StartCoroutine("FindEnemiesWithDelay", .2f);
    }*/

    IEnumerator FindEnemiesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            //FindVisibleEnemies();
        }
    }

    public List<Transform> FindVisibleEnemies(float viewRadius, float viewAngle, LayerMask enemyMask, LayerMask coverMask, List<Transform> visibleEnemies, GameObject ai)
    {
        visibleEnemies.Clear();

        Collider[] enemiesInViewRadius = Physics.OverlapSphere(ai.transform.position, viewRadius, enemyMask);

        for(int i=0; i<enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInViewRadius[i].transform;
            Vector3 dirToEnemy = (enemy.position - ai.transform.position).normalized;
            if(Vector3.Angle(ai.transform.forward, dirToEnemy) < viewAngle / 2)
            {
                float distToEnemy = Vector3.Distance(ai.transform.position, enemy.position);

                if(!Physics.Raycast(ai.transform.position, dirToEnemy, distToEnemy, coverMask))
                {
                    visibleEnemies.Add(enemy);
                }
            }
        }
        return visibleEnemies;
    }

    /*public Vector3 DirFromAngle(float angleInDegress, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegress += ai.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }*/
}
