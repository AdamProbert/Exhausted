using UnityEngine;
using UnityEngine.AI;
using TMPro;
 
// Use physics raycast hit from mouse click 
// to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CarController))]

public class AIInput : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI direction, destinationAngle, horizontalIn, verticalIn, distanceToTarget;
    NavMeshAgent m_Agent;
    NavMeshPath m_Path;

    CarController carController;
    int pathIter = 1;
    Vector3 destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    Vector3 endDestination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    RaycastHit m_HitInfo = new RaycastHit();
 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(m_Path != null && m_Path.corners != null && m_Path.corners.Length > 0)
        {
            var prev = m_Agent.transform.position;
            for(int i = pathIter; 
                i < m_Path.corners.Length; ++i)
            {
                Gizmos.DrawLine(prev, m_Path.corners[i]);
                prev = m_Path.corners[i];
            }

            if(m_Path.corners.Length > pathIter)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(m_Path.corners[pathIter], new Vector3(.5f, .5f, .5f));
            }
        }
    }
 
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updatePosition = false;
        m_Agent.updateRotation = false;

        m_Path = new NavMeshPath();

        carController = GetComponent<CarController>();
    }
 
    void Update()
    {
        // Debug.Log("Car world position: " + carController.transform.position);
        // m_Agent.nextPosition = carController.transform.position;
        if (Input.GetMouseButtonDown(0) && 
            !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
            {
                //m_Agent.destination = m_HitInfo.point;
                m_Path = new NavMeshPath();
                endDestination = m_HitInfo.point;
            }
        }

        // Recalc path every frame..
        m_Agent.CalculatePath(endDestination, m_Path);
        pathIter = 1;
 
        if (m_Path.corners == null || m_Path.corners.Length == 0)
            return;
 
 
        if (pathIter >= m_Path.corners.Length)
        {
            destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            return;
        }
        else
        {
            destination = m_Path.corners[pathIter];
        }

        if (destination.x < float.PositiveInfinity)
        {
            Vector3 heading = destination - transform.position;
            float distance = heading.magnitude;

            // Are we still travelling?
            if (distance > m_Agent.radius*2)
            {
                carController.BrakeAllWheels(false);

                Vector3 direction = heading / distance;
                bool reversing;
                float angle;
                float steeringAngle;

                angle = Vector3.Angle(heading, transform.forward);
                

                // Should we be reversing?
                if(angle > 135)
                {
                    reversing = true;
                    angle = Vector3.Angle(heading, -transform.forward); // Recalc angle with reverse heading
                }
                else
                {
                    reversing = false;
                }

                // If target is close, make tight turn
                if(distance < 5f)
                {
                    steeringAngle = 1;
                }

                // Get percentage for steering angle
                steeringAngle = angle/180;
                

                // If turning left, steering angle should be negative
                if(direction.x < 0)
                {
                    steeringAngle *=-1;
                }

                if(reversing)
                {
                    carController.Move(steeringAngle, -1);
                    UpdateDebugUI(direction, angle, steeringAngle, -1, distance);
                }
                else
                {
                    carController.Move(steeringAngle, 1);
                    UpdateDebugUI(direction, angle, steeringAngle, 1, distance);
                }
                
            }
            else
            {
                ++pathIter;
                if (pathIter >= m_Path.corners.Length)
                {
                    destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
                    carController.BrakeAllWheels(true);
                }
            }

        }
        m_Agent.nextPosition = transform.position;
    }

    void UpdateDebugUI(Vector3 dir, float a, float h, float v, float d)
    {
        direction.SetText(dir.ToString());
        horizontalIn.SetText(h.ToString());
        destinationAngle.SetText(a.ToString());
        verticalIn.SetText(v.ToString());
        distanceToTarget.SetText(d.ToString());
    }
 
}