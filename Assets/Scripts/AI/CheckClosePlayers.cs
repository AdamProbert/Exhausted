using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using System.Collections.Generic;


namespace NodeCanvas.Tasks.Conditions
{

    [Category("System Events")]
    [Description("Check how many players are close by")]

    public class CheckClosePlayers : ConditionTask
    {

       [RequiredField]
        public BBParameter<int> playerThreshold = 2;
        public BBParameter<float> awarnessDistance = 5f;

        public BBParameter<LayerMask> ignoreLayers;
        protected override string info {
            get { return "close to " + playerThreshold + " players"; }
        }

        protected override bool OnCheck() 
        {
            // Get all affected colliders
            List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(this.agent.transform.position, awarnessDistance.value, ignoreLayers.value));
            if(colliders.Count > playerThreshold.value)
            {
                return true;
            }
            return false;
        }
    }
}