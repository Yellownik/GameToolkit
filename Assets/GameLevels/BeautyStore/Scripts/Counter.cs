using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counter;

    public int CurrentValue { get; private set; }

    public void Init(int initialValue) => 
        SetValue(initialValue);

    public void AddValue(int value) => 
        SetValue(CurrentValue + value);

    public bool CanSpend(int value) => 
        CurrentValue >= value;

    public void SpendValue(int value) => 
        SetValue(CurrentValue - value);

    private void SetValue(int value)
    {
        CurrentValue = value;
        _counter.text = value.ToString();
    }
}
