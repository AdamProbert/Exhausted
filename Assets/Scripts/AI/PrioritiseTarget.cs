using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using NodeCanvas.Framework.Internal;

namespace NodeCanvas.Tasks.Actions
{

    [Description("Assigns Target")]
    public class PrioritiseTarget : ActionTask<Transform>
    {

        [BlackboardOnly]
        public BBParameter<GameObject> saveAs;

        [BlackboardOnly][RequiredField]
        public BBParameter<List<GameObject>> possibleTargets;


        public BBParameter<float> maxDistance = 50;
        public BBParameter<float> awarnessDistance = 0f;
        [SliderField(1, 180)]
        public BBParameter<float> viewAngle = 70f;
        public Vector3 offset;

        private RaycastHit hit;

        protected override string info {
            get { return "Assigning a target with highest priority"; }
        }
        protected override void OnExecute() 
        {
            
            Dictionary<GameObject, float> priorities = new Dictionary<GameObject, float>();
            GameObject bestTarget = null;
            float bestTargetPriority = -1;

            foreach(GameObject target in possibleTargets.value)
            {
                if(target && AIHelperFunctions.CanSeeTarget(
                    agent,
                    target.transform,
                    maxDistance.value,
                    awarnessDistance.value,
                    viewAngle.value
                    )
                )
                {
                    float p = CalculatePriority(target);
                    if(p > bestTargetPriority)
                    {
                        bestTargetPriority = p;
                        bestTarget = target;
                    }
                }                
            }

            if(bestTarget != null)
            {
                saveAs.value = bestTarget;
                EndAction(true);    
            }
            EndAction(false);
            
        }

        private float CalculatePriority(GameObject target)
        {
            float p = 0f;
            p += Vector3.Distance(agent.position, target.transform.position);
            return p;       
        }
    }
}