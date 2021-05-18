using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObject/SaveData", order = 1)]
public class ScriptObj : ScriptableObject
{
    public int[] CoinCollection, tempCoins;

    public void DeleteCoins()
    {
        CoinCollection = new int[5];
        RestartCoins();
    }

    public void RestartCoins()
    {
        tempCoins = new int[5];
    }

    public void AddCoin(int coinInd)
    {
        tempCoins[coinInd] = 1;
    }

    public void SaveCoins()
    {
        foreach (int i in CoinCollection)
        {
            if (CoinCollection[i] == 0) CoinCollection[i] = tempCoins[i];
        }
    }

    public bool CoinsObtained(int coinInd)
    {
        if (CoinCollection[coinInd] == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckCoins()
    {
        foreach (int n in CoinCollection)
        {
            if (CoinCollection[n] == 1)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckForWin()
    {
        foreach (int n in tempCoins)
        {
            if (tempCoins[n] + CoinCollection[n] == 0)
            {
                return;
            }
        }
        GameManager.ChangeScene("EndScene");
    }
}
