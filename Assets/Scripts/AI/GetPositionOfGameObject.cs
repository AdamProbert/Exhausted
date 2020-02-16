using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

namespace NodeCanvas.Tasks.Actions
{

    [Description("Returns vector 3 as transform.position")]
    public class GetPositionOfGameObject : ActionTask<Transform>
    {

        [BlackboardOnly]
        public BBParameter<Vector3> saveAs;

        [RequiredField] public BBParameter<GameObject> target;

        protected override string info {
            get { return "Returns position of gameobject"; }
        }
        protected override void OnExecute() 
        {
            saveAs.value = target.value.transform.position;
            EndAction(true);
        }
    }
}