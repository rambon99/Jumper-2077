using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseChar : MonoBehaviour, IKillable
{
    [SerializeField] protected float maxLife;
    [SerializeField] Collider deathbox;
    [SerializeField] AudioSource death;
    protected float life;

    public void EditHealth(float dmg)
    {
        life -= dmg;
        life = Mathf.Min(life, maxLife);
        if (transform.tag == "Player")
        {
            life = Mathf.Max(life, 0);
            GameObject.FindGameObjectWithTag("Menu").GetComponent<HUD>().UpdateHealthbar(maxLife, life);
        }
        if (life <= 0)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Animator>().SetBool("Dead", true);
            deathbox.enabled = true;
            death.Play();
            if (transform.tag == "Player")
            {
                StartCoroutine(DeathDelay());
            }
            else
            {
                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<Enemy>().NME = null;
                GameObject.FindGameObjectWithTag("Counter").GetComponent<CoinSpawner>().UpdateEnemyCount();
            }
        }
    }

    

    IEnumerator DeathDelay()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        GameObject.FindGameObjectWithTag("Menu").GetComponent<DeathScreen>().DeathMenu();
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
    }
}
