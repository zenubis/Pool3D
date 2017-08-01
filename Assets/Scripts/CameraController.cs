using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float minZoom = 0.8f;
    public float maxZoom = 3;

    private GameObject whiteBall;
    private Rigidbody whiteBallRB;

    // camera will be controlled using these variables
    private float cameraZoom = 1.0f;
    private float cameraAngle = 0; // on the xz plane
    private Vector3 cameraLookAt = new Vector3(0, 0, 0);
	
	void Start ()
    {
        whiteBall = GameObject.Find("WhiteBall");
        if (null != whiteBall) {
            whiteBallRB = whiteBall.GetComponent<Rigidbody>();
        }

        cameraLookAt = whiteBall.transform.position;
        cameraLookAt.y = 0;

        // update camera rotation
        UpdateCameraTransformation();
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool updateCameraTransform = false;

        float wheelScroll = Input.GetAxis("Mouse ScrollWheel");
        if (wheelScroll != 0) {
            cameraZoom += wheelScroll * 0.5f;
            cameraZoom = Mathf.Clamp(cameraZoom, minZoom, maxZoom);
            updateCameraTransform = true;
        }

        if (Input.GetMouseButton(1)) {
            // if the right mouse button is down, we rotate the camera around
            cameraAngle += Input.GetAxis("Mouse X");

            // keep the value of cameraAngle from -360 to 360
            cameraAngle -= (int)(cameraAngle / 360) * 360;
            updateCameraTransform = true;
        }

        if (updateCameraTransform) {
            UpdateCameraTransformation();
        }

        if (Input.GetMouseButtonUp(0)) {
            // hit the ball!
            if (null != whiteBallRB) {
                Debug.Log("adding force");
                whiteBallRB.AddForce(GetCameraToBallVector() * 10, ForceMode.VelocityChange);
            }
        }

        Debug.Log("whiteBallRB.velocity" + whiteBallRB.velocity.ToString() + " angularVelo:" + whiteBallRB.angularVelocity.ToString());
	}

    private void UpdateCameraTransformation()
    {
        Vector3 viewDir = new Vector3(1, 0, 0);
        Quaternion rotate = Quaternion.Euler(0, cameraAngle, 30);
        viewDir = rotate * viewDir;
        Debug.Log("cameraAngle:" + cameraAngle); 
        //Debug.Log("cameraZoom:" + cameraZoom + " viewDir:" + viewDir.ToString());
        transform.position = cameraLookAt + viewDir * cameraZoom;
        transform.LookAt(cameraLookAt);
    }

    private Vector3 GetCameraToBallVector()
    {
        Vector3 dir = whiteBall.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        return dir;
    }
    
}
