using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Pathfinding")]
    [Description("SHOULD NOT BE USED. IS EMPTY")]
    public class GetPointOnNavmesh : ActionTask<Transform>
    {
        protected override void OnExecute() {
            EndAction(true);    
        }
    }    
}