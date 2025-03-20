using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider throttle;
    [SerializeField] Slider steer;
    
    public float topSpeed = 60f;
    public float topAngularSpeed = .3f;
    private float fwdAccel;
    private float rotAccel;
    private float steerValue;
    private Rigidbody rb;
    void Start()
    {
        steer = GameObject.FindGameObjectWithTag("Steering").GetComponent<Slider>();
        throttle = GameObject.FindGameObjectWithTag("Throttle").GetComponent<Slider>();
        rb = GetComponent<Rigidbody>();
        rb.drag = 1f;
        rb.angularDrag = 2f;
    }


    void FixedUpdate()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        fwdAccel = 3f * throttle.value;
        rotAccel = .3f * throttle.value;
        steerValue = (steer.value / 10);
        Vector2 steerVector = new Vector2(Mathf.Cos(steerValue * Mathf.PI/2), Mathf.Sin(steerValue * Mathf.PI/2)); //steerVector.x represents the forward component of the acceleration, steerVector.y represents the horizontal component (rotational acceleration)
        Vector3 velocity = (steerVector.x * transform.forward * fwdAccel);
        Vector3 rotation = (steerVector.y * Vector3.up * rotAccel);
  
        rb.AddForce(velocity, ForceMode.Acceleration);
        rb.AddTorque(rotation, ForceMode.Acceleration);
        if (rb.velocity.magnitude > topSpeed) //clamping top speed
        {
            rb.velocity = rb.velocity.normalized * topSpeed;
        }
        if (rb.angularVelocity.magnitude > topAngularSpeed) //clamping top angular speed
        {
            rb.angularVelocity = rb.angularVelocity.normalized * topAngularSpeed;
        }
        rb.AddForce(new Vector3(0, -.66f - rb.position.y, 0) * 9.81f, ForceMode.Acceleration); //unable to freeze Y position do to issues with rotation, so instead we just accelerate the boat towards its waterline level using AddForce
    }
}
