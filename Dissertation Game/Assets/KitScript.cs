using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitScript : MonoBehaviour
{
    [SerializeField] private int HPRestored;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float pickUpDistance;

    private GameManager gameManager;

    private void Start()
    {
        StartCoroutine(CheckSurroundingsWithDelay(0.2f));
    }

    IEnumerator CheckSurroundingsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            CheckSurroundings();
        }
    }

    private void CheckSurroundings()
    {
        Collider[] detectedPlayers = Physics.OverlapSphere(transform.position, pickUpDistance, playerMask);
        if(detectedPlayers.Length != 0)
        {
            PlayerLogic playerLogic = detectedPlayers[0].GetComponent<PlayerLogic>();
            playerLogic.RestoreHP(HPRestored);
            Destroy(this.gameObject);
        }
        else
        {
            Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, pickUpDistance, enemyMask);
            if (detectedEnemies.Length != 0) 
            {
                EnemyThinker enemyThinker = detectedEnemies[0].GetComponent<EnemyThinker>();
                enemyThinker.RestoreHP(HPRestored);
                Destroy(this.gameObject);
            }
        }      
    }

    private void OnDestroy()
    {
        gameManager.ReduceNumOfKits();
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

}
