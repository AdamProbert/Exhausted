using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using System.Collections.Generic;


namespace NodeCanvas.Tasks.Conditions
{

    [Category("GameObject")]
    [Description("Can see any object in given gameobject. A combination of line of sight and view angle check")]
    public class CanSeeAnyone : ConditionTask<Transform>
    {

        [RequiredField]
        public BBParameter<List<GameObject>> targets;

        public BBParameter<float> maxDistance = 50;
        public BBParameter<float> awarnessDistance = 0f;
        [SliderField(1, 180)]
        public BBParameter<float> viewAngle = 70f;
        public Vector3 offset;

        private RaycastHit hit;

        protected override string info {
            get { return "Can See " + targets; }
        }

        protected override bool OnCheck() 
        {
            foreach(GameObject target in targets.value)
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
                    return true;
                }
            }
            return false;
        }
    }
}