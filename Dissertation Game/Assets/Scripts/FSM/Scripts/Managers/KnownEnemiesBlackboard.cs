using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnownEnemiesBlackboard
{
    public List<KnownEnemy> knownEnemiesList { get; set; }
    private int enemyIndex;

    public KnownEnemiesBlackboard()
    {
        knownEnemiesList = new List<KnownEnemy>();
        enemyIndex = -1;
    }

    public class KnownEnemy
    {
        public Transform transform { get; set; }
        public Vector3 currentPosition { get; set; }
        public Vector3 previousPosition { get; set; }

        public int hp { get; set; }

        public KnownEnemy(Transform transform, Vector3 currentPosition, Vector3 previousPosition, int hp)
        {
            this.transform = transform;
            this.currentPosition = currentPosition;
            this.previousPosition = previousPosition;
            this.hp = hp;
        }
    }

    private bool CheckIfEnemyExists(Transform transform)
    {
        enemyIndex = -1;
        if (!IsEmpty())
        {
            for(int i=0; i<knownEnemiesList.Count; i++)
            {
                KnownEnemy recordedEnemy = knownEnemiesList[i];
                if (recordedEnemy.transform.gameObject.Equals(transform.gameObject))
                {
                    enemyIndex = i;
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    private bool CheckIfEnemyMoved(Vector3 newPosition, int index)
    {
        if (knownEnemiesList[index].currentPosition.Equals(newPosition))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void UpdateEnemyList(Transform transform)
    {
        if (CheckIfEnemyExists(transform))
        {
            if (CheckIfEnemyMoved(transform.position, enemyIndex))
            {
                UpdateEnemy(transform.position);
            }
        }
        else
        {
            AddEnemy(transform);
        }
    }

    public bool IsEmpty()
    {
        return knownEnemiesList.Count == 0;
    }

    public void RemoveAtIndex(int index)
    {
        knownEnemiesList.RemoveAt(index);
    }

    private void UpdateEnemy(Vector3 newPosition)
    {
        knownEnemiesList[enemyIndex].previousPosition = knownEnemiesList[enemyIndex].currentPosition;
        knownEnemiesList[enemyIndex].currentPosition = newPosition;
    }

    private void AddEnemy(Transform transform)
    {
        int currentHealth = 0;

        if(transform.TryGetComponent<PlayerLogic>(out PlayerLogic playerLogic))
        {
            currentHealth = playerLogic.CurrentHealth;
        }
        else
        {
            EnemyThinker enemyThinker = transform.gameObject.GetComponent<EnemyThinker>();
            if (enemyThinker != null)
            {
                currentHealth = (int)enemyThinker.currentHP;
            }
            else
            {
                return;
            }
        }

        KnownEnemy enemy = new KnownEnemy(transform, transform.position, transform.position, currentHealth);
        knownEnemiesList.Add(enemy);
    }

    public GameObject DetermineTheClosestEnemyObject(Vector3 origin)
    {
        int index = FindTheClosestEnemy(origin);
        if(index == -1)
        {
            return null;
        }
        else
        {
            if (knownEnemiesList.Count > index)
            {
                if (knownEnemiesList[index].transform != null)
                {
                    return knownEnemiesList[index].transform.gameObject;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    private bool IsEnemyVisible(Transform aiTransform, Vector3 enemyPosition, EnemyStats enemyStats)
    {
        Vector3 dirToEnemy = (enemyPosition - aiTransform.position).normalized;
        if (Vector3.Angle(aiTransform.forward, dirToEnemy) < enemyStats.viewAngle / 2)
        {
            float distToEnemy = Vector3.Distance(aiTransform.position, enemyPosition);

            RaycastHit hit;
            if (Physics.SphereCast(aiTransform.position, 0.3f, dirToEnemy, out hit))
            {
                if (!hit.collider.CompareTag(enemyStats.wallTag) && !hit.collider.CompareTag(aiTransform.tag))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public int DetermineTheClosestEnemyIndex(Vector3 origin)
    {
        return FindTheClosestEnemy(origin);
    }

    public Vector3 GetClosestPreviousPosition(Vector3 origin)
    {
        int index = FindTheClosestEnemy(origin);
        if(index == -1)
        {
            return Vector3.zero;
        }
        else
        {
            return knownEnemiesList[index].previousPosition;
        }
    }

    public Vector3 GetClosestCurrentPosition(Vector3 origin)
    {
        int index = FindTheClosestEnemy(origin);
        if (index == -1)
        {
            return Vector3.zero;
        }
        else
        {
            return knownEnemiesList[index].currentPosition;
        }
    }

    private int FindTheClosestEnemy(Vector3 origin)
    {
        int index = -1;
        float shortestDistance = 0;
        if (knownEnemiesList.Count == 1)
        {
            return 0;
        }
        else if (knownEnemiesList.Count == 0)
        {
            return index;
        }
        else
        {
            for (int i = 0; i < knownEnemiesList.Count; i++)
            {
                float distance = Vector3.Distance(origin, knownEnemiesList[i].currentPosition);
                if (i == 0)
                {
                    shortestDistance = distance;
                    index = 0;
                }
                else
                {
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        index = i;
                    }
                }
            }
            return index;
        }
    }

    public int FindEnemyIndex(Transform targetTransform)
    {
        for (int i = 0; i < knownEnemiesList.Count; i++)
        {
            if (knownEnemiesList[i].transform.Equals(targetTransform))
            {
                return i;
            }
        }
        return 0;
    }

    public void UpdateEnemyHP(Transform transform, int newHP)
    {
        int index = FindEnemyIndex(transform);
        if (knownEnemiesList.Count > index)
        {
            knownEnemiesList[index].hp = newHP;
        }
    }

    public void RemoveEnemy(Vector3 knownPosition)
    {
        for(int i=0; i<knownEnemiesList.Count; i++)
        {
            if(knownEnemiesList[i].currentPosition.Equals(knownPosition) ||
                knownEnemiesList[i].previousPosition.Equals(knownPosition))
            {
                knownEnemiesList.RemoveAt(i);
                break;
            }
        }
    }

    public void RemoveEnemy(Transform transform)
    {
        for(int i=0; i < knownEnemiesList.Count; i++)
        {
            if (knownEnemiesList[i].transform.Equals(transform))
            {
                knownEnemiesList.RemoveAt(i);
            }
        }
    }

    public bool AnyEnemiesSeen()
    {
        return knownEnemiesList.Count > 0;
    }
}
