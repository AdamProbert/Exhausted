﻿using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Pathfinding")]
    [Description("Get's a random point on the navmesh")]
    public class GetRandomPointOnNavemesh : ActionTask<Transform>
    {

        [BlackboardOnly]
        public BBParameter<Vector3> saveAs;
        public BBParameter<float> minWanderDistance = 5;
        public BBParameter<float> maxWanderDistance = 20;
        protected override void OnExecute() {
            var min = minWanderDistance.value;
            var max = maxWanderDistance.value;
            min = Mathf.Clamp(min, 0.01f, max);
            max = Mathf.Clamp(max, min, max);

            var wanderPos = agent.transform.position;
            while ( ( wanderPos - agent.transform.position ).sqrMagnitude < min ) {
                wanderPos = ( Random.insideUnitSphere * max ) + agent.transform.position;
            }
            
            int bestnavmask = 1 << NavMesh.GetAreaFromName("BestDriving");
            NavMeshHit hit;
            Vector3 finalPosition = agent.transform.position;
            if ( NavMesh.SamplePosition(wanderPos, out hit, float.PositiveInfinity, bestnavmask) ) {
                finalPosition = hit.position;
            }


            // NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
            // // Pick the first indice of a random triangle in the nav mesh
            // int t = Random.Range(0, navMeshData.indices.Length-3);
            
            // // Select a random point on it
            // finalPosition = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t+1]], Random.value);
            // Vector3.Lerp(finalPosition, navMeshData.vertices[navMeshData.indices[t+2]], Random.value);    
            
            saveAs.value = finalPosition;
            EndAction(true);    
        }
    }    
}