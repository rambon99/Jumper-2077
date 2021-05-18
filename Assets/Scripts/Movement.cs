using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IKillable
{
    [SerializeField]  float maxLife;
    [SerializeField] Collider deathbox;
    [SerializeField] AudioSource death;
     float life;

    [SerializeField] float  speed, turnSpeed, jumpForce;
    [SerializeField] Weapon[] guns;

    Animator anim;
    bool onGrounded = true;
    Vector3 horizontal, vertical;
    GameObject cam;
    Weapon curGun;
    int gunInd = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        life = maxLife;
        curGun = guns[0];
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        if (Time.timeScale == 1)
        {
            Mov();
        }
    }

    private void Mov() 
    {
        horizontal = cam.transform.right.normalized * speed * Input.GetAxis("Horizontal");
        vertical = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized * speed * Input.GetAxis("Vertical");
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //root.transform.Rotate(0, 90, 0, Space.World);
            anim.SetFloat("Speed", 1);
            transform.forward = Vector3.Lerp(transform.forward, (horizontal + vertical), turnSpeed);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
        transform.Translate(horizontal + vertical, Space.World);

        if (Input.GetKeyDown(KeyCode.Space) && onGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
           curGun.g.UseGun(curGun.transform.position, curGun.gameObject, gameObject);
        }
        else if(curGun.g.effect == 1 && Input.GetKeyUp(KeyCode.Mouse0))
        {
            curGun.g.bang.Stop();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapGun();
            GameObject.FindGameObjectWithTag("Menu").GetComponent<HUD>().UpdateWeapon();
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

        float dist = Vector3.Distance(anim.GetIKPosition(AvatarIKGoal.RightHand), anim.GetIKPosition(AvatarIKGoal.LeftHand));
        Vector3 dir = cam.transform.forward * dist;
        if (transform.InverseTransformVector(dir).z >= 0)
        {
            if (curGun.g.effect == 0)
            {
                curGun.transform.parent.forward = dir;
            }
            else
            {
                curGun.transform.forward = dir;
            }
        }
        anim.SetIKPosition(AvatarIKGoal.LeftHand, curGun.g.weaponSocket.transform.position);
    }

    private void Jump()
    {
        onGrounded = false;
        anim.SetBool("Grounded", false);
        anim.SetTrigger("Jump");
        GetComponent<Rigidbody>().velocity += new Vector3(0, jumpForce);
    }

    private void SwapGun()
    {
        curGun.StopIt();
        curGun.g.SetActive();
        curGun.gameObject.SetActive(false);
        if (gunInd < guns.Length - 1)
        {
            gunInd++;
        }
        else
        {
            gunInd = 0;
        }
        curGun = guns[gunInd];
        curGun.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Jumpable")
        {
            anim.SetBool("Grounded", true);
            onGrounded = true;
        }
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
        life = Mathf.Min(life, maxLife);
        life = Mathf.Max(life, 0);
        GameObject.FindGameObjectWithTag("Menu").GetComponent<HUD>().UpdateHealthbar(maxLife, life);
        if (life <= 0)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Animator>().SetBool("Dead", true);
            deathbox.enabled = true;
            death.Play();
            StartCoroutine(DeathDelay());
        }
    }

    IEnumerator DeathDelay()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        GameObject.FindGameObjectWithTag("Menu").GetComponent<DeathScreen>().DeathMenu();
    }

    /*private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            if (gunInd == 1)
            {
                Gizmos.DrawLine(curGun.transform.position, cam.transform.forward + Vector3.up);
                Gizmos.DrawLine(curGun.transform.position, cam.transform.forward + Vector3.down);
                Gizmos.DrawLine(curGun.transform.position, cam.transform.forward + Vector3.right);
                Gizmos.DrawLine(curGun.transform.position, cam.transform.forward + Vector3.left);
            }
            else
            {
                Gizmos.DrawLine(curGun.transform.position, cam.transform.forward);
            }
        }
    }*/
}
