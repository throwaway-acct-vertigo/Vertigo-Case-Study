using DG.Tweening;
using UnityEngine;

public class CardFrontTweener : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutBack);
    }
}
