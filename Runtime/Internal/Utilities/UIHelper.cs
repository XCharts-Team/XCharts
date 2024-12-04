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
        internal static void DrawBackground(VertexHelper vh, UIComponent component)
        {
            var background = component.background;
            if (!background.show)
                return;
            if (background.image != null)
                return;

            var backgroundColor = component.theme.GetBackgroundColor(background);
            var borderWidth = background.borderStyle.GetRuntimeBorderWidth();
            var borderColor = background.borderStyle.GetRuntimeBorderColor();
            var cornerRadius = background.borderStyle.GetRuntimeCornerRadius();
            UGL.DrawRoundRectangleWithBorder(vh, component.graphRect, backgroundColor, backgroundColor, cornerRadius,
                borderWidth, borderColor);
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