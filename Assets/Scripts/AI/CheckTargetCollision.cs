using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions
{

    [Category("System Events")]
    [Description("Check if we have collided with the designated target")]

    public class CheckTargetCollision : ConditionTask
    {

        public BBParameter<GameObject> target;

        protected override void OnEnable() {
            router.onCollisionEnter += OnCollisionEnter;
            // router.onCollisionExit += OnCollisionExit;
        }

        protected override void OnDisable() {
            router.onCollisionEnter -= OnCollisionEnter;
            // router.onCollisionExit -= OnCollisionExit;
        }

        public void OnCollisionEnter(ParadoxNotion.EventData<Collision> data) {
            if ( data.value.gameObject == target.value ) 
            {
                YieldReturn(true);
            }
        }
    }
}