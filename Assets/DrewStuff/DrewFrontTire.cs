using UnityEngine;
using System.Collections;

public class DrewFrontTire : MonoBehaviour
{
    public Transform bike;
    public Transform backTireTrans;

    private DrewBackTire backTire;
    private DrewBikePhysics bikePhysics;

    private int layerMask = 1 << 8;

	void Awake ()
    {
        bikePhysics = bike.GetComponent<DrewBikePhysics>();
        backTire = backTireTrans.GetComponent<DrewBackTire>();
	}

    void Start()
    {
        layerMask = ~layerMask;
    }

    void Update()
    {
        CrashGroundCheck();
    }

    void CrashGroundCheck()
    {
        if (Physics.Raycast(transform.position, transform.forward, 1, layerMask) && !backTire.isGrounded)
        {
            bikePhysics.hasCrashed = true;
        }
    }
}
