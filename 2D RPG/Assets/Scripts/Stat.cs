using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image _content;
    private float _currentFillRatio;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] Text statText;
    private float _currentValue;
    public float CurrentValue
    {
        get => _currentValue;
        set
        {
            if (value > MaxValue)
                _currentValue = MaxValue;
            else if (value < 0)
                _currentValue = 0;
            else
                _currentValue = value;

            _currentFillRatio = _currentValue / MaxValue;
            statText.text = $"{_currentValue}/{MaxValue}";
        }
    }
    public float MaxValue { get; set; }

    void Start()
    {
        _content = GetComponent<Image>();
    }

    void Update()
    {
        if (_currentFillRatio != _content.fillAmount)
            _content.fillAmount = Mathf.Lerp(_content.fillAmount, _currentFillRatio, _lerpSpeed * Time.deltaTime);
    }

    public void Initialize(float currentValue, float maxValue)
    {
        MaxValue = maxValue;
        CurrentValue = currentValue;
    }
}
