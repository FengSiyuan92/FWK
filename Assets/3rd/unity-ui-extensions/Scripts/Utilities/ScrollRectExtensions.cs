/// Credit Feaver1968 
/// Sourced from - http://forum.unity3d.com/threads/scroll-to-the-bottom-of-a-scrollrect-in-code.310919/

namespace UnityEngine.UI.Extensions
{
    public static class ScrollRectExtensions
    {
        public static void ScrollToTop(this ScrollRect scrollRect)
        {
            scrollRect.normalizedPosition = new Vector2(0, 1);
        }
        public static void ScrollToBottom(this ScrollRect scrollRect)
        {
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
        
        public static Vector2 CaculateNormalizedPos(this ScrollRect scrollRect, float x, float y)
        {
            var rectContent = scrollRect.content.rect;
            var rectView = scrollRect.viewport.rect;

            var dx = rectContent.width - rectView.width;
            var dy = rectContent.height - rectView.height;
            var nx = Mathf.Abs(dx) < 0.0001f ? 0 : (x - rectView.width * 0.5f) / dx;
            var ny = Mathf.Abs(dy) < 0.0001f ? 0 : (y - rectView.height * 0.5f) / dy;

            nx = Mathf.Clamp01(nx);
            ny = Mathf.Clamp01(ny);
            return new Vector2(nx, ny);
        }

        public static Vector2 CaculateNormalizedPosByTransform(this ScrollRect scrollRect, Transform child, bool lookChildCenter = true)
        {
            var content = scrollRect.content;
            var rect = content.rect;
            var pivot = content.pivot;
            var relative = content.transform.InverseTransformPoint(child.position);
            var lbx = relative.x + rect.width * pivot.x;
            var lby = relative.y + rect.height * pivot.y;

            var rt = child.GetComponent<RectTransform>();
            if (lookChildCenter)
            {
                lbx += rt.rect.width * (0.5f - rt.pivot.x);
                lby += rt.rect.height * (0.5f - rt.pivot.y);
            }
            return CaculateNormalizedPos(scrollRect, lbx, lby);
        }
    }
}