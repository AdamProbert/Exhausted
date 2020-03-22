using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;


namespace NodeCanvas.Tasks.Actions
{
    [Category("Pickups")]
    [Description("Find all pickups in scene")]
    public class FindPickups : ActionTask<GameObject>
    {
        [BlackboardOnly]
        public BBParameter<List<Pickup>> saveAs;

        protected override string info {
            get { return "Collecting pickups in game"; }
        }
        
        protected override void OnExecute() 
        {
            if(GameObject.FindObjectsOfType<Pickup>() != null)
            {
                saveAs.value = new List<Pickup>(GameObject.FindObjectsOfType<Pickup>());
            }
            EndAction();
        }
    }
}