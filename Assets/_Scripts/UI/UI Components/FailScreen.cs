using System;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup = null;
    public CanvasGroup canvasGroup => _canvasGroup;
    [SerializeField] private Button tryAgain = null;



    public void Init(Action tryAgainAction)
    {
        tryAgain.onClick.AddListener( () => { tryAgainAction.InvokeNullCheck();} );
    }

    private void OnDestroy()
    {
        tryAgain.onClick.RemoveAllListeners();
    }
}