using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSlice : MonoBehaviour
{
    [SerializeField] private Image sliceImage;
    [SerializeField] private TextMeshProUGUI rewardText;

    public void Initialize(SliceData sliceData)
    {
        if (sliceImage != null && sliceData.iconSprite != null)
        {
            sliceImage.sprite = sliceData.iconSprite;
            sliceImage.gameObject.SetActive(true);
        }

        if (rewardText != null)
        {
            if (sliceData.isBomb)
            {
                rewardText.text = "";
            }
            else
            {
                rewardText.text = Mathf.FloorToInt(ZoneManager.Instance.CurrentStrategy.CurrentMultiplier * sliceData.Reward.Amount).ToString();
            }
        }
    }
}