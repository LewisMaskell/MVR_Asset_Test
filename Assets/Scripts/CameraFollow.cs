using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private float followSpeed = 5f;
    private float distance = 15f;
    //private Vector3 offset = new Vector3(0, 0, 20);
    private float offset = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Vector3 cameraMovement = Vector3.Lerp(
            transform.position, 
            player.transform.position - (player.transform.forward * offset) + (Vector3.up * offset), 
            Time.fixedDeltaTime * followSpeed
            );

        transform.position = new Vector3(cameraMovement.x, distance, cameraMovement.z);
        transform.LookAt(player.transform.position);
    }
}
