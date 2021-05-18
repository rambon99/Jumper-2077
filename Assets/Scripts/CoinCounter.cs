using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] GameObject yCoinContainer, rCoinContainer;
    [SerializeField] ScriptObj saveData;

    Transform[] yCoinList, rCoinList;
    int rList;
    // Start is called before the first frame update
    void Start()
    {
        yCoinList = InitializeLists(yCoinContainer);
        rCoinList = InitializeLists(rCoinContainer);
        UpdateYellowCoins(saveData.CoinCollection);
    }

    Transform[] InitializeLists(GameObject container)
    {
        int i = 0;
        Transform[] list = new Transform[container.transform.childCount];
        foreach(Transform g in container.GetComponentsInChildren<Transform>())
        {
            if (i != 0)
            {
                list[i-1] = g;
            }
            i++;
        }
        return list;
    }

    public void UpdateRedCoins()
    {
        if (rList < rCoinList.Length)
        {
            rCoinList[rList].GetComponent<Image>().color = Color.red;
            rList++;
        }
    }

    public void UpdateYellowCoins(int[] sd)
    {
        for (int j = 0; j < sd.Length; j++)
        {
            if (sd[j] == 1)
            {
                yCoinList[j].GetComponent<Image>().color = Color.white;
            }
        }
    }
}
