using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitScript : MonoBehaviour
{
    [SerializeField] private int HPRestored;
    [SerializeField] private List<LayerMask> enemyMasksList;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float pickUpDistance;

    private GameManager gameManager;

    private void Start()
    {
        StartCoroutine(CheckSurroundingsWithDelay(0.2f));
        for(int i=0; i<gameManager.enemyMasksList.Count; i++)
        {
            enemyMasksList.Add(gameManager.enemyMasksList[i]);
        }
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
            gameManager.RemoveKit(transform.position);
            Destroy(this.gameObject);
        }
        else
        {
            for(int i=0; i<enemyMasksList.Count; i++)
            {
                Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, pickUpDistance, enemyMasksList[i]);
                if (detectedEnemies.Length != 0)
                {
                    EnemyThinker enemyThinker = detectedEnemies[0].GetComponent<EnemyThinker>();
                    SensingSystem sensingSystem = detectedEnemies[0].GetComponent<SensingSystem>();
                    enemyThinker.RestoreHP(HPRestored);
                    sensingSystem.RemoveKit(transform);
                    gameManager.RemoveKit(transform.position);

                    Destroy(this.gameObject);
                }
            }

            /*
            Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, pickUpDistance, enemyMask1);
            if (detectedEnemies.Length != 0) 
            {
                EnemyThinker enemyThinker = detectedEnemies[0].GetComponent<EnemyThinker>();
                enemyThinker.RestoreHP(HPRestored);
                Destroy(this.gameObject);
            }*/
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
