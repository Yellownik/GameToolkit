using DG.Tweening;
using Orbox.Async;
using UnityEngine;
using UnityEngine.UI;

public class StyleProgress : MonoBehaviour
{
    [SerializeField] private CanvasGroup _container;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration = 3f;

    public void Init()
    {
        _container.alpha = 0;
    }
    
    public IPromise WaitForEnd()
    {
        var promise = new Promise();
        
        _container.DOFade(1, 0.5f);
        
        _slider.value = 0;
        _slider.DOValue(1, _duration)
            .OnComplete(() =>
            {
                _container.DOFade(0, 0.5f);
                promise.Resolve();
            });

        return promise;
    }
}
