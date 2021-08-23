using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public interface IUIRoot
    {
        RectTransform MainCanvas { get; }
        RectTransform MenuCanvas { get; }
        Image FadeImage { get; }
    }
}
