using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    private KnownEnemiesBlackboard enemiesBlackboard;
    private int maxHealth;
    private int currentHealth;
    private HpBarScript hpBarScript;
    private GameManager gameManager;
    private Image healthBar;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        hpBarScript = GetComponent<HpBarScript>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthBar = transform.Find("Canvas/HealthBG/HealthBar").GetComponent<Image>();
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
        UpdateHPBar();
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
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }

}
