using UnityEngine;
using System.Collections;

public class flythroughScript : MonoBehaviour {
	
	public bool firstpass = true;
	public GameObject thisCamera;
	public GameObject theRoot;
	public GameObject theBike;
	// Use this for initialization
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(thisCamera.transform.position == theRoot.transform.position && ! firstpass )
		{
			thisCamera.gameObject.SetActive(false);
			theBike.gameObject.SetActive(true);
		}
		else
		{
			firstpass = false;
		}
	}
}
