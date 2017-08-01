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
        if (whiteBallRB.velocity.sqrMagnitude >= 0.5f || whiteBallRB.angularVelocity.sqrMagnitude >= 0.5f) {
            return; // no updates when the ball is moving
        }

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

        if (Input.GetMouseButtonUp(0)) {
            // hit the ball!
            if (null != whiteBallRB) {
                whiteBallRB.AddForce(GetCameraToBallVector() * 5, ForceMode.VelocityChange);
            }
        }
        
        // move the camera over to the new ball position
        Vector3 ballPos = whiteBall.transform.position;
        ballPos.y = 0;
        Vector3 vecDir = ballPos - cameraLookAt;
        if (vecDir.sqrMagnitude > 0) {
            updateCameraTransform = true;
            cameraLookAt = cameraLookAt + (vecDir * 0.08f);
        }

        if (updateCameraTransform) {
            UpdateCameraTransformation();
        }
    }

    private void UpdateCameraTransformation()
    {
        Vector3 zoomVector = new Vector3(1, 0, 0);
        zoomVector = Quaternion.Euler(0, cameraAngle, 30) * zoomVector;
        transform.position = cameraLookAt + zoomVector * cameraZoom;
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
