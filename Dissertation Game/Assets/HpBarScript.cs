using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    private Image healthBar;
    private Text healthText;
    private PlayerLogic playerLogic;

    void Start()
    {
        healthBar = GameObject.Find("HP").GetComponent<Image>();
        healthText = GameObject.Find("HpText").GetComponent<Text>();
        playerLogic = GetComponent<PlayerLogic>();
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float fillAmount = (float)playerLogic.CurrentHealth / (float)playerLogic.MaxHealth;
        healthBar.fillAmount = fillAmount;
        healthText.text = playerLogic.CurrentHealth + "/" + playerLogic.MaxHealth;
    }
}
