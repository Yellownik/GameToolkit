using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class DOTweenUtils
    {
        private const float DefaultDuration = 0.5f;
        private const float DefaultScale = 0.9f;
        private static readonly Vector3 DefaultOffset = new Vector3(0, -1, 0);

        private static void CheckDuration(ref float duration)
        {
            if (duration == -1)
                duration = DefaultDuration;
        }

        private static void CheckScale(ref float scale)
        {
            if (scale == -1)
                scale = DefaultScale;
        }

        private static void CheckOffset(ref Vector3? offset)
        {
            if (offset == null)
                offset = DefaultOffset;
        }

        public static Tween DoScale(this Transform transform, bool isShow, float duration = -1, float scale = -1)
        {
            CheckDuration(ref duration);
            CheckScale(ref scale);

            var localScale = transform.localScale;

            if (isShow)
                return transform.DOScale(localScale, duration).From(scale);
            else
                return transform.DOScale(scale, duration);
        }

        public static Tween DoMove(this Transform transform, bool isShow, float duration = -1, Vector3? offset = null)
        {
            CheckDuration(ref duration);
            CheckOffset(ref offset);

            var initPos = transform.localPosition;
            if (isShow)
                return transform.DOLocalMove(initPos, duration).From(initPos + offset.Value);
            else
                return transform.DOLocalMove(initPos + offset.Value, duration);
        }

        #region Fade
        public static Tween DoFade(this SpriteRenderer renderer, bool isShow, float duration = -1)
        {
            CheckDuration(ref duration);

            if (isShow)
                return renderer.DOFade(1, duration).From(0);
            else
                return renderer.DOFade(0, duration);
        }

        public static Tween DoFade(this Image renderer, bool isShow, float duration = -1)
        {
            CheckDuration(ref duration);

            if (isShow)
                return renderer.DOFade(1, duration).From(0);
            else
                return renderer.DOFade(0, duration);
        }

        public static Tween DoFade(this TextMeshProUGUI renderer, bool isShow, float duration = -1)
        {
            CheckDuration(ref duration);

            if (isShow)
                return renderer.DOFade(1, duration).From(0);
            else
                return renderer.DOFade(0, duration);
        }
        #endregion

        #region Set Alpha
        public static void SetAlpha(this Image renderer, float toAlpha)
        {
            var col = renderer.color;
            col.a = toAlpha;
            renderer.color = col;
        }

        public static void SetAlpha(this SpriteRenderer renderer, float toAlpha)
        {
            var col = renderer.color;
            col.a = toAlpha;
            renderer.color = col;
        }

        public static void SetAlpha(this TextMeshProUGUI renderer, float toAlpha)
        {
            var col = renderer.color;
            col.a = toAlpha;
            renderer.color = col;
        }
        #endregion
    }
}