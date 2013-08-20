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
    public float flipSpeed = 5.0f;

    public float flipAngle = 20.0f;
    public float wheelieAngle = 100.0f;

    [HideInInspector]
    public float turboBar = 0.0f;
    public float turboBoostSpeed = 12.0f;
    public float maxTurboBar = 30.0f;
    public float turboSpeed = 40.0f;
    public float turboDepletionSpeed = 5.0f;

    public Transform backTire;
	public Transform frontTire;
    public Transform bikeBody;

    private float accelFactor = 0.0f;
    private float curMaxSpeed = 0.0f;
    
    public Vector3 rotDir;
    private Vector3 moveDir;

    public Quaternion initRot;

    private DrewBackTire drewBackTire;

    void Awake()
    {
        drewBackTire = backTire.GetComponent<DrewBackTire>();
    }

    void Start()
    {
        initRot = bikeBody.rotation;

        curMaxSpeed = maxSpeed;
        //TEMPORARY
        //rotDir.y = 90;
    }

	void Update ()
    {
        Movement();
        Turbo();
	}
    
    void Movement()
    {
        //get inputs
        float forwardInput = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        float flipInput = Input.GetAxis("Flip");

        if (drewBackTire.isGrounded)
        {
            //turn the bike
            rotDir.y += steer * steerSpeed * Time.deltaTime;
            //do a wheelie
            //bikeBody.localRotation = Quaternion.RotateTowards(bikeBody.localRotation, Quaternion.Euler(wheelieAngle, 0, 0), flipSpeed * Time.deltaTime);
            //reset gravity
            Physics.gravity = new Vector3(0, -9.81f, 0);

            if (forwardInput != 0)
            {
                //accelerate
                accelFactor = Mathf.MoveTowards(accelFactor, curMaxSpeed, forwardInput * speed * Time.deltaTime);
            }
            else
            {
                //deccelerate
                accelFactor = Mathf.MoveTowards(accelFactor, 0, deccelSpeed * Time.deltaTime);
            }
        }
        else
        {
            //rotate bike for a flip
            bikeBody.Rotate(Vector3.right * flipSpeed * flipInput * Time.deltaTime);
            //make gravity stronger
            Physics.gravity = new Vector3(0, -14, 0);
        }

        if (flipInput == 0)
        {
            //reset bike's rotation when not flipping
            bikeBody.localRotation = Quaternion.RotateTowards(bikeBody.localRotation, initRot, flipSpeed * Time.deltaTime);
        }
        
        //set max and min speeds
        moveDir.z = Mathf.Clamp(moveDir.z, -minSpeed, accelFactor);
        //apply speed values
        moveDir = new Vector3(0, rigidbody.velocity.y, accelFactor);

        //move
        transform.Translate(moveDir * Time.deltaTime);

        //bank the bike
        rotDir = new Vector3(rotDir.x, rotDir.y, Mathf.LerpAngle(transform.eulerAngles.z, -steerAngle * steer, 0.3f));
        //limit the x angle
        rotDir.x = ClampAngle(transform.eulerAngles.x, flipAngle);

        //apply the rotation values
        transform.eulerAngles = rotDir;
    }

    void Turbo()
    {
        //activate turbo
        if (Input.GetButton("Jump") && turboBar > 0)
        {
            //deplete turbo
            turboBar -= turboDepletionSpeed * Time.deltaTime;
            //set turbo speed
            curMaxSpeed = turboSpeed;
        }
        else
        {
            //reset speed
            curMaxSpeed = maxSpeed;
        }
    }

    //limit the angle
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
