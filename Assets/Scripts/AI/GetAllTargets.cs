using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Pathfinding")]
    [Description("Get's all targets in game")]
    public class GetAllTargets : ActionTask<GameObject>
    {

        [BlackboardOnly]
        public BBParameter<List<GameObject>> saveAs;

        [TagField]
        public string searchTag = "Untagged";
        public bool ignoreCurrent;

        protected override string info {
            get { return "Collecting " + searchTag + " gameobjects"; }
        }
        protected override void OnExecute() 
        {
            List<GameObject> targets = new List<GameObject>();    
            foreach(GameObject g in GameObject.FindGameObjectsWithTag(searchTag))
            {
                Debug.Log("Ignorecurrent: " + ignoreCurrent);
                Debug.Log("agent: " + ownerAgent.gameObject + " g: " + g);
                if(ignoreCurrent)
                {
                    if(g != ownerAgent.gameObject)
                    {
                        targets.Add(g);
                    }
                }
                else
                {
                    targets.Add(g);
                }

            }
            saveAs.value = targets;
            EndAction();
        }
        
    }
}