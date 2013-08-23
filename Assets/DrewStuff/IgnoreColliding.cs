using UnityEngine;
using System.Collections;

public class IgnoreColliding : MonoBehaviour
{
    public Transform lapCounter;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Physics.IgnoreCollision(lapCounter.collider, collider);
	}
}
