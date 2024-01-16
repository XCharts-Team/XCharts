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

        internal static void InitBackground(UIComponent table)
        {
            if (table.background.show == false ||
                (table.background.image == null && ChartHelper.IsClearColor(table.background.imageColor)))
            {
                ChartHelper.DestoryGameObject(table.transform, "Background");
                return;
            }
            var sizeDelta = table.background.imageWidth > 0 && table.background.imageHeight > 0 ?
                new Vector2(table.background.imageWidth, table.background.imageHeight) :
                table.graphSizeDelta;
            var backgroundObj = ChartHelper.AddObject("Background", table.transform, table.graphMinAnchor,
                table.graphMaxAnchor, table.graphPivot, sizeDelta);
            backgroundObj.hideFlags = table.chartHideFlags;

            var backgroundImage = ChartHelper.EnsureComponent<Image>(backgroundObj);
            ChartHelper.UpdateRectTransform(backgroundObj, table.graphMinAnchor,
                table.graphMaxAnchor, table.graphPivot, sizeDelta);
            ChartHelper.SetBackground(backgroundImage, table.background);
            backgroundObj.transform.SetSiblingIndex(0);
            backgroundObj.SetActive(table.background.show && table.background.image != null);
        }
    }
}