using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private KnownEnemiesBlackboard enemiesBlackboard;
    private int maxHealth;
    private int currentHealth;
    private HpBarScript hpBarScript;
    private GameManager gameManager;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        hpBarScript = GetComponent<HpBarScript>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            gameManager.SetPlayerStatus(false);
            enemiesBlackboard.RemoveEnemy(transform);
            Destroy(this.gameObject);
        }
    }

    public void Setup(KnownEnemiesBlackboard blackboard)
    {
        enemiesBlackboard = blackboard;
    }

    public void LowerHP(int value)
    {
        currentHealth -= value;
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
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
