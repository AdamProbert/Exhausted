// using UnityEngine;
// using Cinemachine;
 
// public class InputCameraController : MonoBehaviour
// {
//     //-------------------------------------------------------------------------------------------------//
//     // Unfortunately this script is required as Cinemachine does not support the new input system yet..
//     //-------------------------------------------------------------------------------------------------//

//     private InputMaster controls; //default controls is just the Csharp code you generate from the action maps asset
//     private bool Zoom;

//     private void Awake()
//     {
//         controls = new InputMaster();
//     } 

//     private void OnEnable() => controls.Player.Enable();
//     private void OnDisable() => controls.Player.Disable();

//     private void Update()
//     {
//     }

//     public float GetAxisCustom(string axisName)
//     {
//         LookDelta = controls.Player.Look.ReadValue<Vector2>(); // reads the available camera values and uses them.
//         LookDelta.Normalize();

//         Zoom = controls.Player.Zoom.ReadValue<Value>(); // reads the available camera values and uses them.

//         if (axisName == "Mouse X")
//         {
//             return LookDelta.x;
//         }
//         else if (axisName == "Mouse Y")
//         {
//             if(Zoom)
//             {
//                 return 1;
//             }
//             else
//             {
//                 return 0;
//             }
            
//         }
//         return 0;
//     }
// }