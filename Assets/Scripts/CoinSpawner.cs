using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject redCoinCoins, enemiesCoins;

    int rCoins = 5, enemies = 5;

    public void UpdateEnemyCount()
    {
        enemies--;
        if (enemies <= 0)
        {
            enemiesCoins.SetActive(true);
        }
    }

    public void UpdateRedCoinCount()
    {
        rCoins--;
        if (rCoins == 0)
        {
            redCoinCoins.SetActive(true);
            GameObject.FindGameObjectWithTag("Menu").GetComponent<CoinCounter>().UpdateRedCoins();
        }
    }
}
