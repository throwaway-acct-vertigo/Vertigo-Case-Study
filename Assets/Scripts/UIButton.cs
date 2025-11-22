using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Transform buttonTransform;
    private Vector3 originalScale;

    [Header("Animation")] [SerializeField] private float pressScale = 0.9f;
    [SerializeField] private float animationDuration = 0.1f;
    private Action _onAwake;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonTransform = transform;
        originalScale = buttonTransform.localScale;
        _onAwake?.Invoke();
    }

    private void OnValidate()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            buttonTransform.DOScale(originalScale * pressScale, animationDuration);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
        {
            buttonTransform.DOScale(originalScale, animationDuration);
        }
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    public void AddClickListener(Action callback)
    {
        if (button == null)
        {
            _onAwake = () => button.onClick.AddListener(() => callback?.Invoke());
        }
        else
        {
            button.onClick.AddListener(() => callback?.Invoke());
        }
    }
}