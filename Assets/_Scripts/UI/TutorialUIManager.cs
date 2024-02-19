using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup = null;
    [Space]
    [Header("UI Elements")]
    [SerializeField] private TMP_Text TitleText = null;
    [SerializeField] private TMP_Text GuideText = null;
    [SerializeField] private Image GuideImage = null;
    [SerializeField] private Button startButton = null;
    [Space]
    [Header("Replacement")]
    [SerializeField] private string newTitleText = null;
    [SerializeField] private string newGuideText = null;
    [SerializeField] private Sprite newGuideImage = null;


    
    public void Init(Action buttonCallback)
    {
        startButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.buttonMiniGame);

            buttonCallback.InvokeNullCheck();
            FadeOutTutorialMenu();
        } );

        setTutorialUIElement(newGuideImage, newTitleText, newGuideText);
        FadeInTutorialMenu();
    }

    public void setTutorialUIElement(Sprite newGuideImage, string newTitleText, string newGuideText)
    {
        setTutorialGuideImage(newGuideImage);
        setTutorialTitleText(newTitleText);
        setTutorialGuideText(newGuideText);
    }

    public void setTutorialGuideImage(Sprite newGuideImage)
    {
        GuideImage.sprite = newGuideImage;
    }

    public void setTutorialTitleText(string newTitleText)
    {
        TitleText.text = newTitleText;
    }

    public void setTutorialGuideText(string newGuideText)
    {
        GuideText.text = newGuideText;
    }

    public void FadeOutTutorialMenu()
    {
        canvasGroup.DOFade(0f, 1).OnComplete( () =>
        {
            gameObject.SetActive(false);
        } );
    }

    public void FadeInTutorialMenu()
    {
        canvasGroup.DOFade(1f, 1);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveAllListeners();
    }
}