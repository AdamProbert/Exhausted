using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(BuildingManager))]

public class BuildingUIManager : MonoBehaviour
{
    [SerializeField] Animator attachmentListAnimator;
    [SerializeField] GameObject attachmentListElementPrefab;
    [SerializeField] Transform attachmentListParent;

    private BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GetComponent<BuildingManager>();
        attachmentListAnimator.Play("Panel In");
    }

    public void PopulateAttachments(List<Attachment> attachments)
    {
        for (int i = 0; i < attachments.Count; i++)
        {
            GameObject newListItem = Instantiate(attachmentListElementPrefab);
            newListItem.transform.SetParent(attachmentListParent);
            newListItem.transform.Find("Thumbnail/Background").GetComponent<Image>().sprite = attachments[i].uiImage;
            newListItem.GetComponentInChildren<TextMeshProUGUI>().text = attachments[i].uiName;
            int closureIndex = i;
            newListItem.GetComponent<Button>().onClick.AddListener( () => OnClickAttachment(closureIndex));
        }
    }

    public void OnClickAttachment(int buttonIndex)
    {
        Debug.Log("attachment: " + buttonIndex + " has been clicked");
        buildingManager.AttachmentSelected(buttonIndex);
    }
}
