using UnityEngine;

namespace XCharts.Runtime
{
    public static class LayerHelper
    {
        private static Vector2 s_Vector0And0 = new Vector2(0, 0);
        private static Vector2 s_Vector0And0Dot5 = new Vector2(0, 0.5f);
        private static Vector2 s_Vector0And1 = new Vector2(0, 1f);
        private static Vector2 s_Vector0Dot5And1 = new Vector2(0.5f, 1f);
        private static Vector2 s_Vector0Dot5And0Dot5 = new Vector2(0.5f, 0.5f);
        private static Vector2 s_Vector0Dot5And0 = new Vector2(0.5f, 0f);
        private static Vector2 s_Vector1And1 = new Vector2(1f, 1f);
        private static Vector2 s_Vector1And0Dot5 = new Vector2(1f, 0.5f);
        private static Vector2 s_Vector1And0 = new Vector2(1f, 0);

        internal static Vector2 ResetChartPositionAndPivot(Vector2 minAnchor, Vector2 maxAnchor, float width,
            float height, ref float chartX, ref float chartY)
        {
            if (IsLeftTop(minAnchor, maxAnchor))
            {
                chartX = 0;
                chartY = -height;
                return s_Vector0And1;
            }
            else if (IsLeftCenter(minAnchor, maxAnchor))
            {
                chartX = 0;
                chartY = -height / 2;
                return s_Vector0And0Dot5;
            }
            else if (IsLeftBottom(minAnchor, maxAnchor))
            {
                chartX = 0;
                chartY = 0;
                return s_Vector0And0;
            }
            else if (IsCenterTop(minAnchor, maxAnchor))
            {
                chartX = -width / 2;
                chartY = -height;
                return s_Vector0Dot5And1;
            }
            else if (IsCenterCenter(minAnchor, maxAnchor))
            {
                chartX = -width / 2;
                chartY = -height / 2;
                return s_Vector0Dot5And0Dot5;
            }
            else if (IsCenterBottom(minAnchor, maxAnchor))
            {
                chartX = -width / 2;
                chartY = 0;
                return s_Vector0Dot5And0;
            }
            else if (IsRightTop(minAnchor, maxAnchor))
            {
                chartX = -width;
                chartY = -height;
                return s_Vector1And1;
            }
            else if (IsRightCenter(minAnchor, maxAnchor))
            {
                chartX = -width;
                chartY = -height / 2;
                return s_Vector1And0Dot5;
            }
            else if (IsRightBottom(minAnchor, maxAnchor))
            {
                chartX = -width;
                chartY = 0;
                return s_Vector1And0;
            }
            else if (IsStretchTop(minAnchor, maxAnchor))
            {
                chartX = -width / 2;
                chartY = -height;
                return s_Vector0Dot5And1;
            }
            else if (IsStretchMiddle(minAnchor, maxAnchor))
            {
                chartX = -width / 2;
                chartY = -height / 2;
                return s_Vector0Dot5And0Dot5;
            }
            else if (IsStretchBottom(minAnchor, maxAnchor))
            {
                chartX = -width / 2;
                chartY = 0;
                return s_Vector0Dot5And0;
            }
            else if (IsStretchLeft(minAnchor, maxAnchor))
            {
                chartX = 0;
                chartY = -height / 2;
                return s_Vector0And0Dot5;
            }
            else if (IsStretchCenter(minAnchor, maxAnchor))
            {
                chartX = -width / 2;
                chartY = -height / 2;
                return s_Vector0Dot5And0Dot5;
            }
            else if (IsStretchRight(minAnchor, maxAnchor))
            {
                chartX = -width;
                chartY = -height / 2;
                return s_Vector1And0Dot5;
            }
            else if (IsStretchStrech(minAnchor, maxAnchor))
            {
                chartX = -width / 2;
                chartY = -height / 2;
                return s_Vector0Dot5And0Dot5;
            }
            chartX = 0;
            chartY = 0;
            return Vector2.zero;
        }

        private static bool IsLeftTop(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0And1 && maxAnchor == s_Vector0And1;
        }

        private static bool IsLeftCenter(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0And0Dot5 && maxAnchor == s_Vector0And0Dot5;
        }

        private static bool IsLeftBottom(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == Vector2.zero && maxAnchor == Vector2.zero;
        }

        private static bool IsCenterTop(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0Dot5And1 && maxAnchor == s_Vector0Dot5And1;
        }

        private static bool IsCenterCenter(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0Dot5And0Dot5 && maxAnchor == s_Vector0Dot5And0Dot5;
        }

        private static bool IsCenterBottom(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0Dot5And0 && maxAnchor == s_Vector0Dot5And0;
        }

        private static bool IsRightTop(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector1And1 && maxAnchor == s_Vector1And1;
        }

        private static bool IsRightCenter(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector1And0Dot5 && maxAnchor == s_Vector1And0Dot5;
        }

        private static bool IsRightBottom(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector1And0 && maxAnchor == s_Vector1And0;
        }

        private static bool IsStretchTop(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0And1 && maxAnchor == s_Vector1And1;
        }

        private static bool IsStretchMiddle(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0And0Dot5 && maxAnchor == s_Vector1And0Dot5;
        }

        private static bool IsStretchBottom(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0And0 && maxAnchor == s_Vector1And0;
        }

        private static bool IsStretchLeft(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0And0 && maxAnchor == s_Vector0And1;
        }

        private static bool IsStretchCenter(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0Dot5And0 && maxAnchor == s_Vector0Dot5And1;
        }

        private static bool IsStretchRight(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector1And0 && maxAnchor == s_Vector1And1;
        }

        private static bool IsStretchStrech(Vector2 minAnchor, Vector2 maxAnchor)
        {
            return minAnchor == s_Vector0And0 && maxAnchor == s_Vector1And1;
        }

        public static bool IsStretchPivot(RectTransform rt)
        {
            return IsStretchTop(rt.anchorMin, rt.anchorMax) ||
                IsStretchMiddle(rt.anchorMin, rt.anchorMax) ||
                IsStretchBottom(rt.anchorMin, rt.anchorMax) ||
                IsStretchLeft(rt.anchorMin, rt.anchorMax) ||
                IsStretchCenter(rt.anchorMin, rt.anchorMax) ||
                IsStretchRight(rt.anchorMin, rt.anchorMax) ||
                IsStretchStrech(rt.anchorMin, rt.anchorMax);
        }

        public static bool IsFixedWidthHeight(RectTransform rt)
        {
            return IsLeftTop(rt.anchorMin, rt.anchorMax) ||
                IsLeftCenter(rt.anchorMin, rt.anchorMax) ||
                IsLeftBottom(rt.anchorMin, rt.anchorMax) ||
                IsCenterTop(rt.anchorMin, rt.anchorMax) ||
                IsCenterCenter(rt.anchorMin, rt.anchorMax) ||
                IsCenterBottom(rt.anchorMin, rt.anchorMax) ||
                IsRightTop(rt.anchorMin, rt.anchorMax) ||
                IsRightCenter(rt.anchorMin, rt.anchorMax) ||
                IsRightBottom(rt.anchorMin, rt.anchorMax);
        }

    }
}