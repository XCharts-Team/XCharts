
using UnityEngine;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class TitleHander : MainComponentHandler<Title>
    {
        private static readonly string s_TitleObjectName = "title";
        private static readonly string s_SubTitleObjectName = "title_sub";

        public override void InitComponent()
        {
            var title = component;
            title.painter = null;
            title.refreshComponent = delegate ()
            {
                title.OnChanged();
                var anchorMin = title.location.runtimeAnchorMin;
                var anchorMax = title.location.runtimeAnchorMax;
                var pivot = title.location.runtimePivot;
                var objName = ChartCached.GetComponentObjectName(title);
                var titleObject = ChartHelper.AddObject(objName, chart.transform, anchorMin, anchorMax,
                    pivot, chart.chartSizeDelta);
                title.gameObject = titleObject;
                title.gameObject.transform.SetSiblingIndex(chart.m_PainterTop.transform.GetSiblingIndex() + 1);
                anchorMin = title.location.runtimeAnchorMin;
                anchorMax = title.location.runtimeAnchorMax;
                pivot = title.location.runtimePivot;
                var fontSize = title.labelStyle.textStyle.GetFontSize(chart.theme.title);
                ChartHelper.UpdateRectTransform(titleObject, anchorMin, anchorMax, pivot, new Vector2(chart.chartWidth, chart.chartHeight));
                var titlePosition = chart.GetTitlePosition(title);
                var subTitlePosition = -new Vector3(0, fontSize + title.itemGap, 0);

                titleObject.transform.localPosition = titlePosition;
                titleObject.hideFlags = chart.chartHideFlags;
                ChartHelper.HideAllObject(titleObject);

                var label = ChartHelper.AddChartLabel(s_TitleObjectName, titleObject.transform, title.labelStyle, chart.theme.title,
                    title.text, Color.clear, title.location.runtimeTextAlignment);
                label.SetActive(title.show && title.labelStyle.show);

                var subLabel = ChartHelper.AddChartLabel(s_SubTitleObjectName, titleObject.transform, title.subLabelStyle, chart.theme.subTitle,
                    title.subText, Color.clear, title.location.runtimeTextAlignment);
                subLabel.SetActive(title.show && title.subLabelStyle.show);
                subLabel.transform.localPosition = subTitlePosition + title.subLabelStyle.offset;
            };
            title.refreshComponent();
        }
    }
}