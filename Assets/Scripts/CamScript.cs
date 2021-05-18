using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    [SerializeField] float camSpeed, smoothing;
    [SerializeField] GameObject cam;

    //Vector3 cameraOffset;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //cameraOffset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 1)
        {
            transform.position = player.transform.position + Vector3.up;
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));

            transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y"));
            //    Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y"), Vector3.left);
            cam.transform.LookAt(transform);

            //Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) * Quaternion.AngleAxis(Input.GetAxis("Mouse Y"), Vector3.left);
            /*cameraOffset = camTurnAngle * cameraOffset;
            Vector3 newPos = player.transform.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPos, smoothing);
            transform.LookAt(player.transform.position + Vector3.up * verticalOffset);*/
        }
    }
}
