using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counter;

    private int _currentValue = 0;
    
    public void Init(int initialValue)
    {
        SetValue(initialValue);
    }

    public void AddValue(int value)
    {
        SetValue(_currentValue + value);
    }
    
    public bool CanSpend(int value) => 
        _currentValue >= value;

    public void SpendValue(int value)
    {
        SetValue(_currentValue - value);
    }
    
    private void SetValue(int value)
    {
        _currentValue = value;
        _counter.text = value.ToString();
    }
}
