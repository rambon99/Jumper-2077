using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Explosion : MonoBehaviour
{
    [SerializeField] float power, deathTime;
    float damage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deathTime);
    }

    public void DamageValue(float dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        float dist = power/Vector3.Distance(transform.position, other.transform.position);
        Vector3 dir = (other.transform.position - transform.position).normalized;
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity += dir * dist * Time.deltaTime;
            GetComponent<Collider>().enabled = false;
        }
        else if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<NavMeshAgent>().velocity += dir * dist * Time.deltaTime;
            other.gameObject.GetComponent<IKillable>().TakeDamage(damage);
        }
    }
}
