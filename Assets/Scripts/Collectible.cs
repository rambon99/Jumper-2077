using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollType { Coins, Health, CoinSin};
    public CollType cType;

    [SerializeField] ScriptObj saveData;
    [SerializeField] int coinNum, health;
    [SerializeField] Material empty;
    [SerializeField] AudioSource coinGet;

    public delegate void CollectibleFunc();
    public CollectibleFunc cFunc;

    // Start is called before the first frame update
    void Start()
    {
        switch (cType)

        {
            case CollType.Coins:
                cFunc = CoinCollect;
                if (saveData.CoinsObtained(coinNum))
                {
                    foreach (MeshRenderer m in transform.GetComponentsInChildren<MeshRenderer>())
                    {
                        m.material = empty;
                    }
                }
                break;
            case CollType.Health:
                cFunc = HealthCollect;
                break;
            case CollType.CoinSin:
                cFunc = SingleCoinCollect;
                break;
        }
    }

    void CoinCollect()
    {
        saveData.AddCoin(coinNum);
        coinGet.Play();
        GameObject.FindGameObjectWithTag("Menu").GetComponent<CoinCounter>().UpdateYellowCoins(saveData.tempCoins);
        saveData.CheckForWin();
    }

    void HealthCollect()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<IKillable>().TakeDamage(-health);
    }

    void SingleCoinCollect()
    {

        GameObject.FindGameObjectWithTag("Menu").GetComponent<CoinCounter>().UpdateRedCoins();
        GameObject.FindGameObjectWithTag("Counter").GetComponent<CoinSpawner>().UpdateRedCoinCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cFunc.Invoke();
            if (cType == CollType.Health)
            {
                StartCoroutine(RespawnHealth());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator RespawnHealth()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(10);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
