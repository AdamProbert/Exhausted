using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Michsky.UI.Dark
{
    public class PressKeyEvent : MonoBehaviour
    {
        [Header("KEY")]
        [SerializeField]
        public KeyControl hotkey;
        public bool pressAnyKey;

        [Header("KEY ACTION")]
        [SerializeField]
        public UnityEvent pressAction;

        void Update()
        {
            if(pressAnyKey == true)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    pressAction.Invoke();
                } 
            }

            else
            {
                if (hotkey.wasPressedThisFrame)
                {
                    pressAction.Invoke();
                } 
            }
        }
    }
}