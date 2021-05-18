using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject explosion;
    float damage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathCounter());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime* speed);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    IEnumerator DeathCounter()
    {
        yield return new WaitForSeconds(2);
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<IKillable>().TakeDamage(damage / 2);
        }
        GameObject g = Instantiate(explosion, collision.GetContact(0).point, transform.rotation);
        g.GetComponent<Explosion>().DamageValue(damage);
        Destroy(gameObject);
    }
}
