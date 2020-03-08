using System.Collections.Generic;

[System.Serializable]
public class PlayerConfig
{
    public string baseCarPrefabName;
    public string[] weaponPrefabNames;

    public PlayerConfig(Player player)
    {
        baseCarPrefabName = player.transform.Find("BaseVehicle").GetChild(0).gameObject.name;
        baseCarPrefabName = baseCarPrefabName.Split('(')[0];

        // Car attachments
        List<string> weaponNames = new List<string>();
        foreach (CarAttachPoint attachment in player.GetComponentsInChildren<CarAttachPoint>())
        {
            if(attachment.currentAttachment != null)
            {
                weaponNames.Add(attachment.currentAttachment.name.Split('(')[0]);
            }
            else
            {
                weaponNames.Add("NONE");
            }
        }

        weaponPrefabNames = weaponNames.ToArray();
    }
}