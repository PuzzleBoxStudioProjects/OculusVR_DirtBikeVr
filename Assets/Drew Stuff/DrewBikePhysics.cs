using UnityEngine;
using System.Collections;

public class DrewBikePhysics : MonoBehaviour
{

    public float speed = 10.0f;
    public float maxSpeed = 20.0f;
    public float minSpeed = 10.0f;
    public float steerSpeed = 10.0f;
    public float steerAngle = 30.0f;

	public Transform backTire;
	public Transform frontTire;

    private Vector3 rotDir;

	void FixedUpdate ()
    {
        Movement();
	}

    void Movement()
    {
        //get inputs
        float forwardInput = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float steer = Input.GetAxis("Horizontal");

        //bank the bike
        rotDir = new Vector3(transform.eulerAngles.x, rotDir.y, Mathf.LerpAngle(transform.eulerAngles.z, -steerAngle * steer, 0.3f));
        //turn the bike
        rotDir.y += steer * steerSpeed;
        
        //apply the rotation values
        transform.eulerAngles = rotDir;
        
        Vector3 vel = rigidbody.velocity;
        //set max and min velocities
        vel.z = Mathf.Clamp(vel.z, -minSpeed, maxSpeed);
        rigidbody.velocity = vel;
        
        //move forward
        rigidbody.AddForce(transform.forward * forwardInput, ForceMode.Impulse);
    }
}
