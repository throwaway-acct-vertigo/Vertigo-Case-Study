using UnityEngine;
using DG.Tweening;

public abstract class UIScreen : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected float fadeDuration = 0.3f;

    protected virtual void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, fadeDuration).OnComplete(OnShowComplete);
    }

    public virtual void Hide()
    {
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            gameObject.SetActive(false);
            OnHideComplete();
        });
    }

    protected virtual void OnShowComplete() { }
    protected virtual void OnHideComplete() { }
}