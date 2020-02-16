using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Pathfinding")]
    [Description("Get's a random point on the navmesh")]
    public class GetPointOnNavemesh : ActionTask<Transform>
    {

        [BlackboardOnly]
        public BBParameter<Vector3> saveAs;
        public BBParameter<float> minWanderDistance = 5;
        public BBParameter<float> maxWanderDistance = 20;

        private Vector3 sphereCentre;

        protected override void OnExecute() {
            var min = minWanderDistance.value;
            var max = maxWanderDistance.value;
            min = Mathf.Clamp(min, 0.01f, max);
            max = Mathf.Clamp(max, min, max);

            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
            // Pick the first indice of a random triangle in the nav mesh
            int t = Random.Range(0, navMeshData.indices.Length-3);
            
            // Select a random point on it
            Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t+1]], Random.value);
            Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t+2]], Random.value);

            // Vector3 randomDirection = Random.insideUnitSphere * 500;
            // randomDirection += this.agent.transform.position;
            // NavMeshHit hit;
            // Vector3 finalPosition = Vector3.zero;
            // if (NavMesh.SamplePosition(randomDirection, out hit, 500, 1)) {
            //     finalPosition = hit.position;            
            // }


            // Vector3 randomDirection = Random.insideUnitSphere * max;
            // sphereCentre = this.agent.transform.position + (this.agent.transform.forward * 5);
            // randomDirection += sphereCentre;
            // UnityEngine.AI.NavMeshHit hit;
            // UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, max, UnityEngine.AI.NavMesh.AllAreas);
        
            saveAs.value = point;
            EndAction(true);
                 
        }
    }    
}