using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image _content;
    private float _currentFillRatio;
    [SerializeField] private float _lerpSpeed;
    public float MaxValue { get; set; }

    private float _currentValue;
    public float CurrentValue
    {
        get { return _currentValue; }
        set {
            if (value > MaxValue)
                _currentValue = MaxValue;
            else if (value < 0)
                _currentValue = 0;
            else
                _currentValue = value;
            

            _currentFillRatio = _currentValue / MaxValue;
        }
    }

    void Start()
    {
        _content = GetComponent<Image>();
    }

    void Update()
    {
        if(_currentFillRatio != _content.fillAmount)
            _content.fillAmount = Mathf.Lerp(_content.fillAmount, _currentFillRatio, _lerpSpeed * Time.deltaTime);

        print(_currentValue + "," +  MaxValue);
    }

    public void Initialize(float currentValue, float maxValue)
    {
        MaxValue = maxValue;
        CurrentValue = currentValue;
    }
}
