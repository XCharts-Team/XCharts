using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// UI帮助类。
    /// </summary>
    public static class UIHelper
    {
        public static void DrawBackground(VertexHelper vh, UIComponent component)
        {
            var background = component.background;
            var rect = component.graphRect;
            if (background.imageWidth > 0 || background.imageHeight > 0)
            {
                if (background.imageWidth > 0)
                {
                    rect.width = background.imageWidth;
                    rect.x = component.graphX + (component.graphWidth - background.imageWidth) / 2;
                }
                if (background.imageHeight > 0)
                {
                    rect.height = background.imageHeight;
                    rect.y = component.graphY + (component.graphHeight - background.imageHeight) / 2;
                }
            }
            background.rect = rect;
            if (!background.show)
                return;
            if (background.image != null)
                return;
            var backgroundColor = component.theme.GetBackgroundColor(background);
            DrawBackground(vh, background, backgroundColor);
        }

        public static void DrawBackground(VertexHelper vh, Background background, Color32 color, float smoothness = 2)
        {
            if (!background.show)
                return;
            if (background.image != null)
                return;
            var borderWidth = background.borderStyle.GetRuntimeBorderWidth();
            var borderColor = background.borderStyle.GetRuntimeBorderColor();
            var cornerRadius = background.borderStyle.GetRuntimeCornerRadius();
            UGL.DrawRoundRectangleWithBorder(vh, background.rect, color, color, cornerRadius,
                borderWidth, borderColor, 0, smoothness);
        }

        internal static void InitBackground(UIComponent component)
        {
            if (component.background.show == false ||
                (component.background.image == null && ChartHelper.IsClearColor(component.background.imageColor)))
            {
                ChartHelper.DestoryGameObject(component.transform, "Background");
                return;
            }
            var sizeDelta = component.background.imageWidth > 0 && component.background.imageHeight > 0 ?
                new Vector2(component.background.imageWidth, component.background.imageHeight) :
                component.graphSizeDelta;
            var backgroundObj = ChartHelper.AddObject("Background", component.transform, component.graphMinAnchor,
                component.graphMaxAnchor, component.graphPivot, sizeDelta);
            backgroundObj.hideFlags = component.chartHideFlags;

            var backgroundImage = ChartHelper.EnsureComponent<Image>(backgroundObj);
            ChartHelper.UpdateRectTransform(backgroundObj, component.graphMinAnchor,
                component.graphMaxAnchor, component.graphPivot, sizeDelta);
            ChartHelper.SetBackground(backgroundImage, component.background);
            backgroundObj.transform.SetSiblingIndex(0);
            backgroundObj.SetActive(component.background.show && component.background.image != null);
        }
    }
}