using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
    [Category("GameMode")]
    [Description("Determines the game mode from the battle manager")]
    public class DetermineGameMode : ActionTask<GameObject>
    {
        [BlackboardOnly]
        public BBParameter<GameModeBase.GameModeType> saveAs;

        protected override string info {
            get { return "Collecting game mode from  BattleManager"; }
        }
        
        protected override void OnExecute() 
        {
            if(GameObject.FindObjectOfType<BattleManager>() != null)
            {
                saveAs.value = GameObject.FindObjectOfType<BattleManager>().requestedGameMode;
            }
            else
            {
                // Default to free for all
                saveAs.value = GameModeBase.GameModeType.FreeForAll;
            }   
            EndAction();
        }
    }
}