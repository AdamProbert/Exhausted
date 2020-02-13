using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;


// Arena manager for CarDerby demo
public class Arena : MonoBehaviour 
{
    // The car prefab model to instantiate
    public GameObject CarModel;

    // The deformable cube prefab model to instantiate
    public GameObject BoxModel;

    void Start()
    {
        // Create AI cars
        CreateAICar(Color.red, 1);
        CreateAICar(Color.green, 2);
        CreateAICar(Color.yellow, 3);
        CreateAICar(Color.white, 4);
        CreateAICar(Color.magenta, 5);
        CreateAICar(Color.cyan, 6);
        CreateAICar(Color.gray, 7);
    }

    // Create a new AI car
    void CreateAICar(Color color, int pos)
    {
        Car car = Instantiate<Car>(CarModel.GetComponent<Car>());
        Destroy(car.GetComponent<PlayerControl>());
        Destroy(car.transform.Find("CarCam").gameObject);
        car.Color = color;
        car.transform.position = Quaternion.Euler(0, pos * 45, 0) * new Vector3(0, 0, -15);
        car.transform.forward = -car.transform.position;
        car.transform.parent = transform;
        car.name = "AI Car";
        car.gameObject.AddComponent<AIControl>();
    }

    void Update()
    {
        // Repair on mouse down
        if (Input.GetMouseButton(0))
        {
            // For any camera
            foreach (Camera cam in Camera.allCameras)
            {
                // Check if mouse ray hit something deformable
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    ImpactDeformable impactDeformable = hit.collider.GetComponent<ImpactDeformable>();
                    if (impactDeformable != null)
                        // Repair
                        impactDeformable.Repair(0.05f, hit.point, 0.75f);
                }
            }
        }
    }

    // Create a deformable cube at random position
    public void CreateCube()
    {
        Vector3 p = Random.insideUnitCircle * 20;
        p.z = p.y;
        p.y = 15;
        Instantiate(BoxModel, p, Random.rotation);
    }

    // Repair all Impact Deformable objects on scene
    public void RepairAll()
    {
        FindObjectsOfType<ImpactDeformable>().ToList().ForEach(i => i.Repair(1));
    }

    // Apply random deformation to all Impact Deformable objects in scene
    public void RandomDamageAll()
    {
        FindObjectsOfType<ImpactDeformable>()
            .ToList()
            .ForEach(i => 
                {
                    // Get collider (if any)
                    Collider collider = i.GetComponent<Collider>();
                    if (collider == null)
                        return;

                    // Random ray
                    Vector3 v = (Random.onUnitSphere * 5) + i.transform.position;
                    Ray ray = new Ray(v, i.transform.position - v);
                    
                    // Apply manual deformation
                    RaycastHit hit;
                    if (collider.Raycast(ray, out hit, 10))
                        i.Deform(hit.point, ray.direction.normalized * 0.3f);
                });
    }
}
