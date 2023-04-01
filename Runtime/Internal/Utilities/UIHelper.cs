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
            if (component.background.show == false ||
                (component.background.sprite == null && ChartHelper.IsClearColor(component.background.color)))
            {
                var p1 = new Vector3(component.graphX, component.graphY);
                var p2 = new Vector3(component.graphX + component.graphWidth, component.graphY);
                var p3 = new Vector3(component.graphX + component.graphWidth, component.graphY + component.graphHeight);
                var p4 = new Vector3(component.graphX, component.graphY + component.graphHeight);
                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, GetBackgroundColor(component));
            }
        }

        internal static void InitBackground(UIComponent table)
        {
            if (table.background.show == false ||
                (table.background.sprite == null && ChartHelper.IsClearColor(table.background.color)))
            {
                ChartHelper.DestoryGameObject(table.transform, "Background");
                return;
            }
            var sizeDelta = table.background.width > 0 && table.background.height > 0 ?
                new Vector2(table.background.width, table.background.height) :
                table.graphSizeDelta;
            var backgroundObj = ChartHelper.AddObject("Background", table.transform, table.graphMinAnchor,
                table.graphMaxAnchor, table.graphPivot, sizeDelta);
            backgroundObj.hideFlags = table.chartHideFlags;

            var backgroundImage = ChartHelper.EnsureComponent<Image>(backgroundObj);
            ChartHelper.UpdateRectTransform(backgroundObj, table.graphMinAnchor,
                table.graphMaxAnchor, table.graphPivot, sizeDelta);
            ChartHelper.SetBackground(backgroundImage, table.background);
            backgroundObj.transform.SetSiblingIndex(0);
        }

        public static Color32 GetBackgroundColor(UIComponent component)
        {
            if (component.background.show && !ChartHelper.IsClearColor(component.background.color))
                return component.background.color;
            else
                return component.theme.backgroundColor;
        }
    }
}