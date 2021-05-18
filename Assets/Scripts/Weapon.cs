using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Weapon : MonoBehaviour
{
    public Gun g;

    public static Weapon Instance { get; private set; }
    
    private void OnEnable()
    {
        Instance = this;
    }

    public void CoroutineStuff(IEnumerator cor)
    {
        Debug.Log(transform.name);
        StartCoroutine(cor);
    }

    public void StopIt()
    {
        StopAllCoroutines();

    }

    [System.Serializable]
    public class Gun
    {
        public string name;
        public int effect;
        public float bulletDelay, bulletStrength, damage;
        public GameObject rocket, weaponSocket, FX;
        public AudioSource bang;

        bool active;

        public Gun(string na, int ef, GameObject ro, float bd)
        {
        }

        public void SetActive()
        {
            active = false;
        }

        public void UseGun(Vector3 pos, GameObject gun, GameObject owner)
        {
            //Ray ray = new Ray(player.transform.position, cam.transform.forward);
            //Physics.Raycast(ray);
            Debug.Log("Bullet: " + Instance.name);
            if (!active)
            {
                FX.GetComponent<ParticleSystem>().Play();
                if (!bang.isPlaying)
                {
                    bang.Play();
                }
                owner.GetComponent<Animator>()?.SetTrigger("Fire");
                if (owner.tag == "Player")
                {
                    owner.GetComponent<Rigidbody>().velocity -= gun.transform.forward * bulletStrength;
                }
                else
                {
                    owner.GetComponent<NavMeshAgent>().velocity -= gun.transform.forward * bulletStrength;
                }
                switch (effect)
                {
                    case 0:
                        GameObject go = Instantiate(rocket, pos, owner.transform.rotation);
                        go.transform.up = -gun.transform.forward;
                        go.GetComponent<Rocket>().SetDamage(damage);
                        break;
                    case 1:
                        //owner.GetComponent<Rigidbody>().AddForce(-gun.transform.forward * bulletStrength * 1000);
                        RaycastHit r;
                        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out r))
                        {
                            if (r.transform.tag != owner.transform.tag && (r.transform.tag == "Player" || r.transform.tag == "Enemy"))
                            {
                                r.transform.GetComponent<IKillable>().TakeDamage(damage);
                            }
                        }
                        break;
                    case 2:
                        RaycastHit[] r2 = new RaycastHit[5];
                        Physics.Raycast(gun.transform.position, gun.transform.forward, out r2[0]);
                        Physics.Raycast(gun.transform.position, gun.transform.forward + Vector3.up, out r2[1]);
                        Physics.Raycast(gun.transform.position, gun.transform.forward + Vector3.down, out r2[2]);
                        Physics.Raycast(gun.transform.position, gun.transform.forward + Vector3.right, out r2[3]);
                        Physics.Raycast(gun.transform.position, gun.transform.forward + Vector3.left, out r2[4]);
                        foreach (RaycastHit rh in r2)
                        {
                            if (rh.transform != null && rh.transform.tag != owner.transform.tag && (rh.transform.tag == "Player" || rh.transform.tag == "Enemy"))
                            {
                                rh.transform.GetComponent<IKillable>().TakeDamage(damage);
                            }
                        }
                        break;
                }
                Instance.CoroutineStuff(BulletDelay());
            }
            else
            {
            }
        }

        public IEnumerator BulletDelay()
        {
            active = true;
            yield return new WaitForSeconds(bulletDelay);
            active = false;
        }
    }


}
