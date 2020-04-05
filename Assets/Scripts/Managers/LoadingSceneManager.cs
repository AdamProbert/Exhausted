using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] Sprite[] LoadingSceneImages;
    [SerializeField] string[] LoadingSceneMessages;
    [SerializeField] Image LoadingSceneImage;
    [SerializeField] TextMeshProUGUI LoadingPercentageText;

    [SerializeField] Image LoadingSlider;
    [SerializeField] float MinLoadTime = 10f;
    private float LoadTime = 0f;

    private void Awake() 
    {
        if(LoadingSceneImages != null && LoadingSceneImages.Length > 0)
        {
            LoadingSceneImage.sprite = LoadingSceneImages[Random.Range(0, LoadingSceneImages.Length -1)];
        }
    }
    private void Update() 
    {
        LoadTime += Time.deltaTime;
        float progress = GameManager.Instance.GetLoadingProgress();
        if(progress < 0.9f &&  LoadTime < MinLoadTime*0.9)
        {
            if(LoadingPercentageText != null){LoadingPercentageText.SetText(Mathf.Round(LoadTime/(MinLoadTime*0.9f) * 100).ToString());}
            LoadingSlider.fillAmount = LoadTime/(MinLoadTime*0.9f);
        }
        else if(progress >= 0.9f && LoadTime < MinLoadTime*0.99)
        {
            LoadTime += Time.deltaTime * 2; // Map is done, let's speed up
            if(LoadingPercentageText != null){LoadingPercentageText.SetText(Mathf.Round(LoadTime/MinLoadTime * 100).ToString());}
            LoadingSlider.fillAmount = LoadTime/MinLoadTime;
        }
        else if(progress == 0.9f && LoadTime >= MinLoadTime)
        {
            GameManager.Instance.AllowNextSceneLoad();
        }
    }
}
