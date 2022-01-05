
using UnityEngine;

namespace XCharts
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
                title.textStyle.UpdateAlignmentByLocation(title.location);
                title.subTextStyle.UpdateAlignmentByLocation(title.location);
                var fontSize = title.textStyle.GetFontSize(chart.theme.title);
                ChartHelper.UpdateRectTransform(titleObject, anchorMin, anchorMax, pivot, new Vector2(chart.chartWidth, chart.chartHeight));
                var titlePosition = chart.GetTitlePosition(title);
                var subTitlePosition = -new Vector3(0, fontSize + title.itemGap, 0);
                var titleWid = chart.chartWidth;

                titleObject.transform.localPosition = titlePosition;
                titleObject.hideFlags = chart.chartHideFlags;
                ChartHelper.HideAllObject(titleObject);

                var titleText = ChartHelper.AddTextObject(s_TitleObjectName, titleObject.transform, anchorMin, anchorMax,
                    pivot, new Vector2(titleWid, fontSize), title.textStyle, chart.theme.title);
                titleText.SetActive(title.show);
                titleText.SetLocalPosition(Vector3.zero + title.textStyle.offsetv3);
                titleText.SetText(title.text);

                var subText = ChartHelper.AddTextObject(s_SubTitleObjectName, titleObject.transform, anchorMin, anchorMax,
                    pivot, new Vector2(titleWid, title.subTextStyle.GetFontSize(chart.theme.subTitle)), title.subTextStyle,
                    chart.theme.subTitle);
                subText.SetActive(title.show && !string.IsNullOrEmpty(title.subText));
                subText.SetLocalPosition(subTitlePosition + title.subTextStyle.offsetv3);
                subText.SetText(title.subText);
            };
            title.refreshComponent();
        }
    }
}