using R3;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public readonly Subject<Unit> LevelFinishedSubject = new();

    [SerializeField] private Slider _slider;

    private float _barValue;

    public float BarValue
    {
        set
        {
            _barValue = Mathf.Clamp(value, 0, _slider.value);

            UpdateValue();

            if(_barValue >= _slider.value)
                OnLevelFinished();
        }
    }

    public void SetBarMaxValue(int maxValue) => _slider.maxValue = maxValue;

    public void ResetBar() => BarValue = 0;

    private void DoubleBarMaxValue() => _slider.maxValue *= 2;

    private void UpdateValue() => _slider.value = _barValue;

    private void OnLevelFinished()
    {
        DoubleBarMaxValue();
        ResetBar();
        UpdateValue();

        LevelFinishedSubject?.OnNext(Unit.Default);
    }
}