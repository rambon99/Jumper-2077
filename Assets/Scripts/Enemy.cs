using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IKillable
{
    [SerializeField] float maxLife;
    [SerializeField] Collider deathbox;
    [SerializeField] AudioSource death;
    float life;

    [SerializeField] float shootDist, audioRadius;
    [SerializeField] Weapon gun;
    [SerializeField] GameObject container;
    [SerializeField] LayerMask layer;
    [SerializeField] AudioSource hey;

    int ind = 0;
    bool aim;
    Vector3 curDest;
    NavMeshAgent agent;
    Animator anim;
    GameObject target;
    Transform[] points;

    public delegate void UpdateChar();
    public UpdateChar NME;
    // Start is called before the first frame update
    void Start()
    {
        life = maxLife;

        target = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        GetAllPoints();
        NextPoint();

        NME += Patrol;
        NME += Sight;
        NME += Hearing;
        anim.SetFloat("Speed", 1);
    }

    // Update is called once per frame
    void Update()
    {
        NME?.Invoke();
    }

    void NextPoint()
    {
        if (ind >= points.Length - 1)
        {
            ind = 0;
        }
        else
        {
            ind++;
        }
        agent.SetDestination(points[ind].position);
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, agent.destination) <= 1 && agent.isStopped == false)
        {
            anim.SetFloat("Speed", 0);
            StartCoroutine(TimeChange());
        }
    }

    private void Attack()
    {
        //aim gun
        transform.LookAt(target.transform);
        if (Vector3.Distance(transform.position, target.transform.position) >= shootDist)
        {
            anim.SetFloat("Speed", 1);
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            aim = false;
        }
        else
        {
            anim.SetFloat("Speed", 0);
            agent.isStopped = true;
            aim = true;
            gun.g.UseGun(gun.transform.position, gun.gameObject, gameObject);
        }
    }

    private void Sight()
    {
        if (Vector3.Angle(transform.forward, target.transform.position - transform.position) <= 67 && Vector3.Angle(transform.forward, target.transform.position - transform.position) >= -67)
        {
            RaycastHit Ray;
            Physics.Raycast(transform.position + Vector3.up, target.transform.position - (transform.position), out Ray, layer);
            if (Ray.transform != null && Ray.transform.tag == "Player")
            {
                hey.Play();
                gun.g.SetActive();
                NME += Attack;
                NME += Detection;
                NME -= Sight;
                NME -= Patrol;
            }
        }
    }

    private void Detection()
    {
        RaycastHit Ray;
        Physics.Raycast(transform.position + Vector3.up, target.transform.position - (transform.position), out Ray, layer);
        if (Ray.transform == null || Ray.transform.tag != "Player")
        {
            NME += Sight;
            NME += Patrol;
            NME -= Attack;
            NME -= Detection;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (aim)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

            float dist = Vector3.Distance(anim.GetIKPosition(AvatarIKGoal.RightHand), anim.GetIKPosition(AvatarIKGoal.LeftHand));
            Vector3 dir = ((target.transform.position + Vector3.up) - gun.transform.position).normalized;
            if (transform.InverseTransformVector(dir).z >= 0)
            {
                gun.transform.forward = dir;
            }
            anim.SetIKPosition(AvatarIKGoal.LeftHand, gun.g.weaponSocket.transform.position);
        }
    }

    private void Hearing()
    {

    }

    IEnumerator TimeChange()
    {
        NextPoint();
        agent.isStopped = true;
        yield return new WaitForSeconds(1.2f);
        agent.isStopped = false;
        anim.SetFloat("Speed", 1);
    }

    void GetAllPoints()
    {
        int i = 0;
        points = new Transform[container.transform.childCount];
        foreach (Transform t in container.transform)
        {
            points[i] = t;
            i++;
        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, audioRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, (Quaternion.AngleAxis(67, Vector3.up) * transform.forward) * 2);
        Gizmos.DrawRay(transform.position + Vector3.up, (Quaternion.AngleAxis(-67, Vector3.up) * transform.forward) * 2);

        RaycastHit Ray;
        if ((Vector3.Angle(transform.forward, GameObject.FindGameObjectWithTag("Player").transform.position - transform.position) <= 67 && Vector3.Angle(transform.forward, GameObject.FindGameObjectWithTag("Player").transform.position - transform.position) >= -67) && (Physics.Raycast(transform.position + Vector3.up, GameObject.FindGameObjectWithTag("Player").transform.position - (transform.position), out Ray, layer) && Ray.transform.tag == "Player"))
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position + Vector3.up, GameObject.FindGameObjectWithTag("Player").transform.position - (transform.position));
        }
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
        life = Mathf.Min(life, maxLife);
        GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().SetBool("Dead", true);
        deathbox.enabled = true;
        death.Play();
        GetComponent<NavMeshAgent>().enabled = false;
        NME = null;
        GameObject.FindGameObjectWithTag("Counter").GetComponent<CoinSpawner>().UpdateEnemyCount();
    }
}
