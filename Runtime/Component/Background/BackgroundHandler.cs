using System;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class BackgroundHandler : MainComponentHandler<Background>
    {
        private readonly string s_BackgroundObjectName = "background";
        public override void InitComponent()
        {
            component.painter = chart.painter;
            component.refreshComponent = delegate ()
            {
                var backgroundObj = ChartHelper.AddObject(s_BackgroundObjectName, chart.transform, chart.chartMinAnchor,
                    chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                component.gameObject = backgroundObj;
                backgroundObj.hideFlags = chart.chartHideFlags;

                var backgroundImage = ChartHelper.EnsureComponent<Image>(backgroundObj);
                ChartHelper.UpdateRectTransform(backgroundObj, chart.chartMinAnchor,
                    chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                backgroundImage.sprite = component.image;
                backgroundImage.type = component.imageType;
                backgroundImage.color = chart.theme.GetBackgroundColor(component);

                backgroundObj.transform.SetSiblingIndex(0);
                backgroundObj.SetActive(component.show && component.image != null);
            };
            component.refreshComponent();
        }

        public override void Update()
        {
            if (component.gameObject != null && component.gameObject.transform.GetSiblingIndex() != 0)
                component.gameObject.transform.SetSiblingIndex(0);
        }

        public override void DrawBase(VertexHelper vh)
        {
            if (!component.show)
                return;
            if (component.image != null)
                return;

            var backgroundColor = chart.theme.GetBackgroundColor(component);
            var borderWidth = component.borderStyle.GetRuntimeBorderWidth();
            var borderColor = component.borderStyle.GetRuntimeBorderColor();
            var cornerRadius = component.borderStyle.GetRuntimeCornerRadius();
            UGL.DrawRoundRectangleWithBorder(vh, chart.chartRect, backgroundColor, backgroundColor, cornerRadius,
                borderWidth, borderColor, 0, 1f);
        }
    }
}