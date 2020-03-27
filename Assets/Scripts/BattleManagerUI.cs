using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;
using TMPro;
public class BattleManagerUI : MonoBehaviour
{
    BattleManager battleManager;
    CarController playerCar;
    Player player;
    CarModifier carModifier;

    [SerializeField] TextMeshProUGUI currentSpeedUI;
    [SerializeField] Image currentBoostUI;
    [SerializeField] Image currentHealthUI;
    [SerializeField] Image currentArmourUI;

    [Header("Debug")]
    [SerializeField] bool debugMode;
    [SerializeField] TextMeshProUGUI currentSpeedDebugUI;
    [SerializeField] TextMeshProUGUI currentBoostDebugUI;

    private void OnEnable() 
    {
        battleManager = GetComponentInParent<BattleManager>();
        player = battleManager.GetHumanPlayer();
        playerCar = player.GetComponent<CarController>();
        carModifier = player.GetComponent<CarModifier>();
    }

    private void LateUpdate() 
    {
        currentSpeedUI.SetText(Mathf.RoundToInt(playerCar.CurrentSpeed).ToString());
        currentBoostUI.fillAmount = carModifier.currentBoost / carModifier.maxBoost;
        currentArmourUI.fillAmount = player.currentArmour / player.maxArmour;
        currentHealthUI.fillAmount = player.currentHealth / player.maxHealth;

        if(debugMode)
        {
            currentSpeedDebugUI.SetText(playerCar.CurrentSpeed.ToString());
            currentBoostDebugUI.SetText(carModifier.currentBoost.ToString());
        }
    }
}
