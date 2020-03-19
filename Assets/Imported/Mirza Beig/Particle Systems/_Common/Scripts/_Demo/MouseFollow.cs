
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEngine.InputSystem;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================
            
            public class MouseFollow : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                public float speed = 8.0f;
                public float distanceFromCamera = 5.0f;

                public bool ignoreTimeScale;

                // =================================	
                // Functions.
                // =================================

                // ...

                void Awake()
                {

                }

                // ...

                void Start()
                {

                }

                // ...

                void Update()
                {
                    Vector3 mousePosition = Mouse.current.position.ReadValue();
                    mousePosition.z = distanceFromCamera;

                    Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

                    float deltaTime = !ignoreTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
                    Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * deltaTime));

                    transform.position = position;
                }

                // ...

                void LateUpdate()
                {

                }

                // =================================	
                // End functions.
                // =================================

            }

            // =================================	
            // End namespace.
            // =================================

        }

    }

}

// =================================	
// --END-- //
// =================================
