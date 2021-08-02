using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private KnownEnemiesBlackboard enemiesBlackboard;
    private int maxHealth;
    private int currentHealth;
    private HpBarScript hpBarScript;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        hpBarScript = GetComponent<HpBarScript>();
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
        hpBarScript.UpdateHealthBar();
    }

    public void RestoreHP(int value)
    {
        currentHealth += value;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        enemiesBlackboard.UpdateEnemyHP(transform, currentHealth);
        hpBarScript.UpdateHealthBar();
    }

}
