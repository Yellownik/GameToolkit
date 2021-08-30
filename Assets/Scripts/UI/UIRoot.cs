using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        [SerializeField] private RectTransform _MainCanvas;
        [SerializeField] private RectTransform _MenuCanvas;

        [Space]
        [SerializeField] private Image _FadeImage;

        public RectTransform MainCanvas => _MainCanvas;
        public RectTransform MenuCanvas => _MenuCanvas;

        public Image FadeImage => _FadeImage;
    }
}
