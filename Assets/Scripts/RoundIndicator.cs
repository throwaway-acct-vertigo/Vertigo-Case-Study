using TMPro;
using UnityEngine;
public class RoundIndicator : MonoBehaviour
{ 
    private TMP_Text _textMeshPro;

    private void OnValidate()
    {
        _textMeshPro = GetComponent<TMP_Text>();
        if (_textMeshPro == null)
        {
            Debug.LogWarning("Missing TextMeshPro Component");
        }
    }

    private void Awake()
    {
        if (transform.GetSiblingIndex() == 0)
        {
            _textMeshPro.onCullStateChanged.AddListener(Reposition);
        }
    }

    private void OnEnable()
    {
        ResetText();
    }
    
    public void ResetText() => Text = (transform.GetSiblingIndex() + 1).ToString();

    public string Text
    {
        get => _textMeshPro.text;
        set
        {
            _textMeshPro.text = value;
            int parsedValue = int.Parse(value);
            if (parsedValue % 30 == 0)
            {
                _textMeshPro.color = Color.yellow;
            }
            else if (parsedValue % 5 == 0)
            {
                _textMeshPro.color = Color.green;
            }
            else
            {
                _textMeshPro.color = Color.white;
            }
        }
    }

    private void Reposition(bool newState)
    {
        if (newState)
        {            
            UIManager.Instance.StartRebuildNumberIndicators();
        }
    }
}