/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XChartsDemo
{
    internal static class UIUtil
    {

        public static RectTransform GetRectTransform(Transform transform, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                transform = transform.Find(path);
            }
            if (transform != null)
            {
                return transform.gameObject.GetComponent<RectTransform>();
            }
            return null;
        }
        public static void SetRectTransformWidth(Transform transform, float width, string path = null)
        {
            var rect = GetRectTransform(transform, path);
            if (rect != null)
            {
                rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
            }
        }
        public static void SetRectTransformHeight(Transform transform, float height, string path = null)
        {
            var rect = GetRectTransform(transform, path);
            if (rect != null)
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
            }
        }
        public static void SetRectTransformLeft(Transform transform, float width, string path = null)
        {
            var rect = GetRectTransform(transform, path);
            if (rect != null)
            {
                rect.anchoredPosition = new Vector2(width, rect.anchoredPosition.y);
            }
        }

        public static void SetGridLayoutGroup(Transform transform, Vector2 cellSize, Vector2 spacing, string path = null)
        {
            if (!string.IsNullOrEmpty(path))
            {
                transform = transform.Find(path);
            }
            if (transform != null)
            {
                var com = transform.gameObject.GetComponent<GridLayoutGroup>();
                if (com != null)
                {
                    com.cellSize = cellSize;
                    com.spacing = spacing;
                }
                else
                {
                    Debug.LogError("SetGridLayoutGroupSize ERROR:can't find GridLayoutGroup: " + (transform.name + "/" + path));
                }
            }
        }
    }
}