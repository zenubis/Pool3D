using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float minZoom = 1;
    public float maxZoom = 5;

    private GameObject cueBall;
    private float currentZoom = 1.0f;
	
	void Start ()
    {
        cueBall = GameObject.Find("Ball");
        
        Vector3 lookAt = cueBall.transform.position;
        lookAt.y = 0;

        Vector3 viewDir = new Vector3(1, 1, 1);
        viewDir.Normalize();
        
        transform.position = lookAt + viewDir * currentZoom;
        transform.LookAt(lookAt);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.LeftShift)) {

        }

        // 
        //Input.GetAxis("Mouse ScrollWheel");
	}

    
}
