using UnityEngine;

namespace Utils
{
    public static class RectTransformUtils
    {
        public static void StretchRectTransform(RectTransform transform)
        {
            transform.anchoredPosition = transform.parent.position;
            transform.anchorMin = new Vector2(0f, 0f);
            transform.anchorMax = new Vector2(1f, 1f);
            transform.offsetMin = Vector2.zero;
            transform.offsetMax = Vector2.zero;
            transform.pivot = new Vector2(0.5f, 0.5f);
        }
        
        public static void TopStretchRectTransform(RectTransform transform)
        {
            transform.anchoredPosition = transform.parent.position;
            transform.anchorMin = new Vector2(0f, 1f);
            transform.anchorMax = new Vector2(1f, 1f);
            transform.offsetMin = Vector2.zero;
            transform.offsetMax = Vector2.zero;
            transform.pivot = new Vector2(0.5f, 1f);
        }
        
        public static void StickRectTransformToTop(RectTransform transform)
        {
            transform.anchoredPosition = transform.parent.position;
            transform.anchorMin = new Vector2(0.5f, 1f);
            transform.anchorMax = new Vector2(0.5f, 1f);
            transform.anchoredPosition = Vector2.zero;
            transform.pivot = new Vector2(0.5f, 1f);
        }
    }
}
