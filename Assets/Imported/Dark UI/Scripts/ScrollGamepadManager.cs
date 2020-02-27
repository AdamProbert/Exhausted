using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Michsky.UI.Dark
{
    public class ScrollGamepadManager : MonoBehaviour
    {
        public InputMaster controls;

        [Header("SLIDER")]
        public Scrollbar scrollbarObject;
        public float changeValue = 0.05f;

        [Header("INPUT")]
        public bool invertAxis = false;


        private void Awake() 
        {
            controls = new InputMaster();
        }
        
        void Update()
        {
            float h = controls.UI.ScrollWheel.ReadValue<float>();

            if (invertAxis == false)
            {
                if (h == 1)
                    scrollbarObject.value -= changeValue;

                else if (h == -1)
                    scrollbarObject.value += changeValue;
            }

            else
            {
                if (h == 1)
                    scrollbarObject.value += changeValue;

                else if (h == -1)
                    scrollbarObject.value -= changeValue;
            }
        }
    }
}