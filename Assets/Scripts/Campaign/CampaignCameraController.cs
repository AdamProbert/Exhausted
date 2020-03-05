using UnityEngine;
using Cinemachine;

public class CampaignCameraController : MonoBehaviour
{

    public InputMaster controls; // Our custom InputSystem control scheme
    private Vector2 lookCamera; // your LookDelta
    private float rotateCamera;
    private Vector2 cameraZoom;
    public float deadZoneX = 0.2f;

    [SerializeField] float trackingObjectSpeed;
    [SerializeField] float trackingObjectRotationSpeed;
    public Transform trackingObject;
    public Transform player;
 
    private void Awake()
    {
        controls = new InputMaster();
     }
 
    private void Update()
    {
        cameraZoom = controls.Campaign.CameraZoom.ReadValue<Vector2>();
        CinemachineCore.GetInputAxis = GetAxisCustom;
        MoveTrackingObject();
    }

    private void MoveTrackingObject()
    {
        lookCamera = controls.Campaign.CameraControl.ReadValue<Vector2>().normalized;
        rotateCamera = controls.Campaign.CameraRotate.ReadValue<float>();

        trackingObject.position += trackingObject.forward * lookCamera.y * trackingObjectSpeed * Time.deltaTime;
        trackingObject.position += trackingObject.right * lookCamera.x * trackingObjectSpeed * Time.deltaTime;
        
        Vector3 pos = trackingObject.position;
        pos.y = Terrain.activeTerrain.SampleHeight(trackingObject.position);
        trackingObject.position = pos;
        trackingObject.Rotate(0, rotateCamera * trackingObjectRotationSpeed * Time.deltaTime, 0);
    }
 
    public float GetAxisCustom(string axisName)
    {
        // LookCamera.Normalize();
 
        if (axisName == "CamControlX")
        {
          if (lookCamera.x > deadZoneX || lookCamera.x < -deadZoneX) // To stabilise Cam and prevent it from rotating when LookCamera.x value is between deadZoneX and - deadZoneX
           {
             return lookCamera.x;
           }
        }
 
        else if (axisName == "Zoom")
        {
            return cameraZoom.y;
        }
 
        return 0;
    }
 
    private void OnEnable()
    {
        controls.Campaign.Enable();
    }
 
    private void OnDisable()
    {
        controls.Campaign.Disable();
    }
}