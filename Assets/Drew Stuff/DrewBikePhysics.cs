using UnityEngine;
using System.Collections;

public class DrewBikePhysics : MonoBehaviour
{
    public float speed = 10.0f;
    public float maxSpeed = 20.0f;
    public float minSpeed = 10.0f;
    public float steerSpeed = 10.0f;
    public float steerAngle = 30.0f;
    public float deccelSpeed = 3.0f;

	public Transform backTire;
	public Transform frontTire;

    private float accelFactor = 0.0f;

    private Vector3 rotDir;
    private Vector3 moveDir;

    private DrewBackTire drewBackTire;

    void Awake()
    {
        drewBackTire = backTire.GetComponent<DrewBackTire>();
    }

    void Start()
    {
        rotDir.y = 90;
    }

	void Update ()
    {
        Movement();
	}
    
    void Movement()
    {
        //get inputs
        float forwardInput = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");

        if (drewBackTire.isGrounded)
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
            if (forwardInput != 0)
            {
                //accelerate
                accelFactor = Mathf.MoveTowards(accelFactor, maxSpeed, forwardInput * speed * Time.deltaTime);
            }
            else
            {
                //deccelerate
                accelFactor = Mathf.MoveTowards(accelFactor, 0, deccelSpeed * Time.deltaTime);
            }
        }
        else
        {
            Physics.gravity = new Vector3(0, -20, 0);
        }
        //set max and min speeds
        moveDir.z = Mathf.Clamp(moveDir.z, -accelFactor, accelFactor);
        //apply speed values
        moveDir = new Vector3(0, rigidbody.velocity.y, accelFactor);

        //move
        transform.Translate(moveDir * Time.deltaTime);

        //bank the bike
        rotDir = new Vector3(rotDir.x, rotDir.y, Mathf.LerpAngle(transform.eulerAngles.z, -steerAngle * steer, 0.3f));
        //turn the bike
        rotDir.y += steer * steerSpeed;
        rotDir.x = ClampAngle(transform.eulerAngles.x, 20);

        //apply the rotation values
        transform.eulerAngles = rotDir;
    }

    float ClampAngle(float angle, float limit)
    {
        if (angle > 180)
        {
            float angleB = 360 - angle;

            if (angleB > limit)
            {
                angle = 360 - limit;
            }
            else
            {
                return angle;
            }
        }
        if (angle < 180)
        {
            if (angle > limit && angle < 360 - limit)
            {
                angle = limit;
            }
            else
            {
                return angle;
            }
        }

        return angle;
    }
}
