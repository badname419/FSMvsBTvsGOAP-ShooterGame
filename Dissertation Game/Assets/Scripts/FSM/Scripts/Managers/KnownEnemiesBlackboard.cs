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
        PlayerLogic playerLogic = transform.gameObject.GetComponent<PlayerLogic>();
        KnownEnemy enemy = new KnownEnemy(transform, transform.position, transform.position, playerLogic.CurrentHealth);
        knownEnemiesList.Add(enemy);
    }

    public GameObject DetermineTheClosestEnemy(Vector3 origin)
    {
        int index = -1;
        float shortestDistance = 0;
        if(knownEnemiesList.Count == 1)
        {
            return knownEnemiesList[0].transform.gameObject;
        }
        else
        {
            for(int i=0; i < knownEnemiesList.Count; i++)
            {
                float distance = Vector3.Distance(origin, knownEnemiesList[i].currentPosition);
                if(i == 0)
                {
                    shortestDistance = distance;
                    index = 0;
                }
                else
                {
                    if(distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        index = i;
                    }
                }
            }
            return knownEnemiesList[index].transform.gameObject;
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
        knownEnemiesList[index].hp = newHP;
    }
}
