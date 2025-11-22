using DefaultNamespace;
using TMPro;
using UnityEngine;

public class RewardCardSlotController : SlotController<Reward>
{
    [SerializeField] private TMP_Text _textMeshPro;
    
    public override void UpdateGraphic()
    {
        base.UpdateGraphic();
        _textMeshPro.text = HasContainer ? $"{Container.Name}\nx{Container.Amount}" : string.Empty;
    }
}