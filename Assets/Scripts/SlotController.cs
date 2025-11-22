using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SlotController<T> : Selectable where T : ISlotContainer
    {
        public int SlotIndex;
        public bool HasContainer;
        public bool IsHovering;
        public Action OnSelected;
        protected bool CanHover;

        public T Container { get; private set; }

        protected override void Start()
        {
        }
        
        public virtual void ResetContainer(T container = default)
        {
            HasContainer = container != null;
            Container = container;
            UpdateGraphic();
        }

        public virtual void UpdateGraphic()
        {
            if (HasContainer)
            {
                image.enabled = true;
                image.sprite = Container.Icon;
            }
            else
            {
                image.enabled = false;
            }
        }

        public virtual void Hover(bool hovering)
        {
        }

        public virtual void SetSelected(bool selected)
        {
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnSelected?.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (CanHover)
            {
                Hover(true);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (CanHover)
            {
                Hover(false);
            }
        }
    }
}