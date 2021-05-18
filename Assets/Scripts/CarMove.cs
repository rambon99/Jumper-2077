using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    Vector3 origColl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && transform.position.y <= collision.transform.position.y)
        {
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player" && transform.position.y <= collision.transform.position.y)
        {
            collision.transform.position = new Vector3(transform.position.x + origColl.x, collision.transform.position.y, transform.position.z + origColl.z);
        }
    }
}
