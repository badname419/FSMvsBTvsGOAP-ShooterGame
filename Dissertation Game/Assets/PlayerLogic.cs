using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private KnownEnemiesBlackboard enemiesBlackboard;
    private int maxHealth;
    private int currentHealth;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(KnownEnemiesBlackboard blackboard)
    {
        enemiesBlackboard = blackboard;
    }

    public void LowerHP(int value)
    {
        currentHealth -= value;
        enemiesBlackboard.UpdateEnemyHP(transform, currentHealth);
    }

    public void RestoreHP(int value)
    {
        currentHealth += value;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        enemiesBlackboard.UpdateEnemyHP(transform, currentHealth);
    }

}
