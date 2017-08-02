using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {
    public float minZoom = 0.8f;
    public float maxZoom = 3;

    private GameManager gm;

    // cue stick
    private GameObject cueStick;
    private MeshRenderer cueStickRenderer;

    // camera will be controlled using these variables
    private float cameraZoom = 1.0f;
    private float cameraAngle = 0; // on the xz plane
    private Vector3 cameraLookAt = new Vector3(0, 0, 0);

    void Start ()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        cueStick = GameObject.Find("Cue");
        if (null != cueStick) {
            cueStickRenderer = cueStick.GetComponent<MeshRenderer>();
            cueStickRenderer.enabled = false;
        }

        cameraLookAt = gm.whiteBall.transform.position;
        cameraLookAt.y = 0;

        // update camera rotation
        UpdateCameraTransformation();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gm.whiteBallRB.velocity.sqrMagnitude >= 0.5f || gm.whiteBallRB.angularVelocity.sqrMagnitude >= 0.5f) {
            cueStickRenderer.enabled = false;
            return; // no updates when the ball is moving
        }

        UpdateCueStickTransformation();

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
            // Check if the mouse was clicked over a UI element
            if (!EventSystem.current.IsPointerOverGameObject()) {
                // hit the ball!
                if (null != gm.whiteBallRB) {
                    gm.whiteBallRB.AddForce(GetCueVector() * 6, ForceMode.VelocityChange);
                    cueStickRenderer.enabled = false;
                }
            }
        }
        
        // move the camera over to the new ball position
        Vector3 ballPos = gm.whiteBall.transform.position;
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
        // to update camera's transformation from variables
        Vector3 zoomVector = new Vector3(1, 0, 0);
        zoomVector = Quaternion.Euler(0, cameraAngle, 30) * zoomVector;
        transform.position = cameraLookAt + zoomVector * cameraZoom;
        transform.LookAt(cameraLookAt);
    }

    private void UpdateCueStickTransformation()
    {
        // to update cue stick so that it will point to white ball
        cueStickRenderer.enabled = true;

        cueStick.transform.rotation = Quaternion.Euler(0, cameraAngle, 100);
        cueStick.transform.position = gm.whiteBall.transform.position + Quaternion.Euler(0, cameraAngle, 0) * (new Vector3(0.751f, 0.139f, 0));
    }

    private Vector3 GetCueVector()
    {
        return Quaternion.Euler(0, cameraAngle, 0) * (new Vector3(-1, 0, 0));        
    }
    
}
