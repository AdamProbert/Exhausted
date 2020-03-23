using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.Tasks.Conditions
{

    [Category("Pickups")]
    [Description("Can see any pickup in given list. A combination of line of sight and view angle check")]
    public class FindPickup : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<List<Pickup>> targets;
        public BBParameter<Pickup> saveAs;
        public BBParameter<Pickup.PTypes> pickupType;
        public BBParameter<float> maxDistance = 50;
        public BBParameter<float> awarnessDistance = 0f;
        [SliderField(1, 180)]
        public BBParameter<float> viewAngle = 70f;

        protected override string info {
            get { return "Finding the best of " + targets; }
        }

        protected override void OnExecute() 
        {
            List<Pickup> visible = new  List<Pickup>();
            foreach(Pickup target in targets.value)
            {
                if(!target.available)
                {
                    continue;
                }
                if(target == null && target.PType != pickupType.value)
                {
                    continue;
                }
            
                if (Vector3.Angle((target.transform.position - agent.position).normalized, agent.transform.forward) > viewAngle.value)
                {
                    continue;
                }
                if (Vector3.Distance(agent.position, target.transform.position) > awarnessDistance.value) 
                {
                    continue;
                }

                visible.Add(target);
            }

            float closestDistance = 9999;
            Pickup closestPickup = null;
            foreach(Pickup p in visible)
            {
                float dist = Vector3.Distance(agent.position, p.transform.position);
                if(dist < closestDistance)
                {
                    closestDistance = dist;
                    closestPickup = p;
                }
            }
            if(closestPickup != null)
            {
                saveAs.value = closestPickup;
                EndAction(true);
            }
            EndAction(false);
        }
    }
}