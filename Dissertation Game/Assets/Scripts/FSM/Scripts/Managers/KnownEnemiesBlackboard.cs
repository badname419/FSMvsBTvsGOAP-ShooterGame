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

        public KnownEnemy(Transform transform, Vector3 currentPosition, Vector3 previousPosition)
        {
            this.transform = transform;
            this.currentPosition = currentPosition;
            this.previousPosition = previousPosition;
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
        KnownEnemy enemy = new KnownEnemy(transform, transform.position, transform.position);
        knownEnemiesList.Add(enemy);
    }
}
