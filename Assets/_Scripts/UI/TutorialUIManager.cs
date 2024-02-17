using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using DG.Tweening;


public class TutorialUIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("UI Elements")]
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text GuideText;
    [SerializeField] private Image GuideImage;

    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Replacement")]
    [SerializeField] private string newTitleText;
    [SerializeField] private string newGuideText;
    [SerializeField] private Sprite newGuideImage;

    
    void Start()
    {
        setTutorialUIElement(newGuideImage, newTitleText, newGuideText);
        if(!canvasGroup){
            canvasGroup = GetComponent<CanvasGroup>();
        }
        FadeInTutorialMenu();
        
    }

    public void setTutorialUIElement(Sprite newGuideImage, string newTitleText, string newGuideText){
        setTutorialGuideImage(newGuideImage);
        setTutorialTitleText(newTitleText);
        setTutorialGuideText(newGuideText);
    }

    public void setTutorialGuideImage(Sprite newGuideImage){
        GuideImage.sprite = newGuideImage;
    }

    public void setTutorialTitleText(string newTitleText){
        TitleText.text = newTitleText;
    }

    public void setTutorialGuideText(string newGuideText){
        GuideText.text = newGuideText;
    }

    public void FadeOutTutorialMenu(){
        canvasGroup.DOFade(0f, 1).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void FadeInTutorialMenu(){
        canvasGroup.DOFade(1f, 1);
    }
}
