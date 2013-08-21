﻿using UnityEngine;
using System.Collections;

public class DrewBikePhysics : MonoBehaviour
{
    public float speed = 10.0f;
    public float brakeForce = 40.0f;
    public float maxSpeed = 20.0f;
    public float minSpeed = 10.0f;
    public float steerSpeed = 10.0f;
    public float steerAngle = 30.0f;
    public float deccelSpeed = 3.0f;
    public float flipSpeed = 5.0f;

    public float gravity = 20.0f;

    public float flipAngle = 20.0f;
    public float wheelieAngle = 100.0f;

    //[HideInInspector]
    public float turboBar = 0.0f;
    public float turboBoostSpeed = 12.0f;
    public float maxTurboBar = 30.0f;
    public float turboSpeed = 40.0f;
    public float turboDepletionSpeed = 5.0f;

    public Transform backTire;
	public Transform frontTire;
    public Transform bikeBody;
    
    //[HideInInspector]
    public bool hasCrashed = false;

    //[HideInInspector]
    public float accelFactor = 0.0f;
    public float curMaxSpeed = 0.0f;
    private float vertVel = 10.0f;
    public float distFromGround = 5.0f;

    private bool hasCollidedBarrier = false;

    private Vector3 rotDir;
    private Vector3 moveDir;
    private Vector3 lastPos;

    private Quaternion initRot;

    private DrewBackTire drewBackTire;
    private CheckPoints checkPoints;

    private int layerMask = 1 << 8;

    void Awake()
    {
        drewBackTire = backTire.GetComponent<DrewBackTire>();
        checkPoints = GetComponent<CheckPoints>();
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
        HeightControl();
	}

    void HeightControl()
    {
        RaycastHit hitInfo;

        if (!Physics.Raycast(transform.position, Vector3.down, out hitInfo, distFromGround))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0.3f;
            }
        }
        else
        {
            if (Time.timeScale != 1)
            {
                Time.timeScale = 1;
            }
        }
    }

    void Movement()
    {
        //get inputs
        float forwardInput = Input.GetAxis("Vertical");
        float gasInput = Input.GetAxis("RightTrigger");
        float steer = Input.GetAxis("Horizontal");
        float flipInput = Input.GetAxis("RightAnalog");
        float brakeInput = Input.GetAxis("LeftTrigger");

        GearShifts(gasInput);

        if (drewBackTire.isGrounded)
        {
            //turn the bike
            rotDir.y += steer * steerSpeed * Time.deltaTime;
            //do a wheelie
            //bikeBody.localRotation = Quaternion.RotateTowards(bikeBody.localRotation, Quaternion.Euler(wheelieAngle, 0, 0), flipSpeed * Time.deltaTime);
            //reset gravity
            Physics.gravity = new Vector3(0, -9.81f, 0);
            vertVel = 0;

            if (gasInput == 0)
            {
                //slow to a stop with no input
                accelFactor = Mathf.MoveTowards(accelFactor, 0, deccelSpeed * Time.deltaTime);
            }

            if (!hasCrashed)
            {
                if (gasInput != 0)
                {
                    //accelerate
                    accelFactor = Mathf.MoveTowards(accelFactor, curMaxSpeed, gasInput * speed * Time.deltaTime);
                }
                if (brakeInput != 0)
                {
                    //brake
                    accelFactor = Mathf.MoveTowards(accelFactor, 0, brakeInput * brakeForce * Time.deltaTime);   
                }
                if (forwardInput > 0)
                {
                    //accelerate
                    accelFactor = Mathf.MoveTowards(accelFactor, curMaxSpeed, forwardInput * speed * Time.deltaTime);
                }
                if (forwardInput < 0)
                {
                    //brake
                    accelFactor = Mathf.MoveTowards(accelFactor, 0, forwardInput * -brakeForce * Time.deltaTime);   
                }
            }
        }
        else
        {
            if (!hasCrashed && flipInput > 0)
            {
                //rotate bike for a flip
                bikeBody.Rotate(Vector3.left * flipSpeed * flipInput * Time.deltaTime);
            }
            //make gravity stronger
            Physics.gravity = new Vector3(0, -20, 0);
            vertVel = -30;
        }

        if (flipInput == 0 && !hasCrashed)
        {
            //reset bike's rotation
            bikeBody.localRotation = Quaternion.RotateTowards(bikeBody.localRotation, initRot, flipSpeed * Time.deltaTime);
        }

        //bank the bike
        rotDir = new Vector3(rotDir.x, rotDir.y, Mathf.LerpAngle(transform.eulerAngles.z, -steerAngle * steer, 0.3f));
        //limit the x angle
        rotDir.x = ClampAngle(transform.eulerAngles.x, flipAngle);

        //apply the rotation values
        transform.eulerAngles = rotDir;

        //if crashed stop moving and reset rotation and reposition at the last checkpoint
        if (hasCrashed)
        {
            accelFactor = 0;
            
            if (!IsInvoking("Respawn"))
            {
                Invoke("Respawn", 2.0f);
            }
        }

        //set max and min speeds
        moveDir.z = Mathf.Clamp(moveDir.z, 0, accelFactor);
        //apply speed values
        moveDir = new Vector3(0, rigidbody.velocity.y, accelFactor);
        
        //move
        //rigidbody.velocity = transform.TransformDirection(moveDir);
        if (!LevelScripts.isGreen)
        {
            transform.Translate(moveDir * Time.deltaTime);
        }
        //RaycastHit hitInfo;

        //if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 2))
        //{
        //    Vector3 surfaceNormal = hitInfo.normal;
        //    surfaceNormal.Normalize();

        //    transform.rotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;
        //}
    }

    void Respawn()
    {
        transform.rotation = checkPoints.currentCheckpoint.rotation;
        transform.position = checkPoints.currentCheckpoint.position;

        bikeBody.localRotation = initRot;
        
        hasCrashed = false;
    }

    void Turbo()
    {
        //activate turbo
        if (Input.GetButton("Boost") && turboBar > 0)
        {
            //deplete turbo
            turboBar -= turboDepletionSpeed * Time.deltaTime;
            //set turbo speed
            curMaxSpeed = turboSpeed;
            audio.pitch = 3;
        }
        else
        {
            //reset speed
            curMaxSpeed = maxSpeed;
        }
    }

    void GearShifts(float gasPressure)
    {
        if (gasPressure != 0 && !Input.GetButton("Boost"))
        {
            audio.pitch = gasPressure * speed * Time.deltaTime;
            if (audio.pitch < 0.5f)
            {
                audio.pitch = 0.5f;
            }
            if (audio.pitch > 1.5f)
            {
                audio.pitch = 1.5f;
            }
        }
        if (gasPressure == 0)
        {
            audio.pitch = 0.5f;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Barrier")
        {
            accelFactor = 0;
            Respawn();
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