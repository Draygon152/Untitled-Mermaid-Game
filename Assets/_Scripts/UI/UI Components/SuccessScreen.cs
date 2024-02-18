using System;
using UnityEngine;
using UnityEngine.UI;

public class SuccessScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup = null;
    public CanvasGroup canvasGroup => _canvasGroup;
    [SerializeField] private Button tryAgain = null;
    [SerializeField] private Button next = null;



    public void Init(Action tryAgainAction, Action nextAction)
    {
        tryAgain.onClick.AddListener( () =>
        {
            tryAgainAction.InvokeNullCheck();
        } );

        next.onClick.AddListener(() =>
        {
            nextAction.InvokeNullCheck();
        });
    }

    private void OnDestroy()
    {
        tryAgain.onClick.RemoveAllListeners();
        next.onClick.RemoveAllListeners();
    }
}