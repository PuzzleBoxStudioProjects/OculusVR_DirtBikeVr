using UnityEngine;
using System.Collections;

public class DrewBikePhysics : MonoBehaviour {

    public float speed = 10.0f;
    public float steerSpeed = 10.0f;
    public float steerAngle = 30.0f;

	public Transform backTire;
	public Transform frontTire;

    private Vector3 orientation = Vector3.zero;

    void Awake() {
        orientation = transform.eulerAngles;
    }

	// Use this for initialization
	void Start () {
	
	}

    void Update() {
        //transform.eulerAngles = orientation;
    }

	// Update is called once per frame
	void FixedUpdate () {
        Movement();
	}

    void Movement() {
        float forwardInput = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float steer = Input.GetAxis("Horizontal");

        Vector3 rotDir = new Vector3(backTire.eulerAngles.x, backTire.eulerAngles.y, Mathf.LerpAngle(backTire.eulerAngles.z, -steerAngle * steer, 0.3f));
        backTire.eulerAngles = rotDir;

        backTire.rigidbody.AddForce(backTire.forward * forwardInput, ForceMode.Impulse);
    }
}
