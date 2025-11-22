using TMPro;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class RewardSlotController : SlotController<Reward>
    {
        private TMP_Text _textMeshPro;

        protected override void Awake()
        {
            base.Awake();
            image = GetComponentInChildren<Image>();
        }
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _textMeshPro = GetComponentInChildren<TMP_Text>(true);
        }
#endif

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateGraphic();
        }

        public override void UpdateGraphic()
        {
            base.UpdateGraphic();
            if (HasContainer)
            {
                _textMeshPro.text = Container.Amount.ToString();
            }
            else
            {
                _textMeshPro.text = "";
            }
        }
    }
}