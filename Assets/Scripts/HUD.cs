using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] GameObject rocketLauncher, shotgun, rifle;
    [SerializeField] Slider health;
    [SerializeField] RenderTexture rt;

    int ind;
    GameObject[] weapons;
    // Start is called before the first frame update
    void Start()
    {
        weapons = new GameObject[3] { rocketLauncher, shotgun, rifle };
    }

    public void UpdateHealthbar(float maxLife, float curLife)
    {
        float lifePer = curLife / maxLife;
        health.value = lifePer;
    }

    public void UpdateWeapon()
    {
        weapons[ind].SetActive(false);
        if (ind >= weapons.Length - 1)
        {
            ind = 0;
        }
        else
        {
            ind++;
        }
        weapons[ind].SetActive(true);
    }
}
