using UnityEngine;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class TitleHander : MainComponentHandler<Title>
    {
        private static readonly string s_TitleObjectName = "title";
        private static readonly string s_SubTitleObjectName = "title_sub";
        private ChartLabel m_LabelObject;
        private ChartLabel m_SubLabelObject;

        public override void InitComponent()
        {
            var title = component;
            title.painter = null;
            title.refreshComponent = delegate()
            {
                title.OnChanged();
                var anchorMin = title.location.runtimeAnchorMin;
                var anchorMax = title.location.runtimeAnchorMax;
                var pivot = title.location.runtimePivot;
                var objName = ChartCached.GetComponentObjectName(title);
                var titleObject = ChartHelper.AddObject(objName, chart.transform, anchorMin, anchorMax,
                    pivot, chart.chartSizeDelta);
                title.gameObject = titleObject;
                title.gameObject.transform.SetSiblingIndex(chart.m_PainterUpper.transform.GetSiblingIndex() + 1);
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

                m_LabelObject = ChartHelper.AddChartLabel(s_TitleObjectName, titleObject.transform, title.labelStyle, chart.theme.title,
                    GetTitleText(title), Color.clear, title.location.runtimeTextAlignment);
                m_LabelObject.SetActive(title.show && title.labelStyle.show);

                m_SubLabelObject = ChartHelper.AddChartLabel(s_SubTitleObjectName, titleObject.transform, title.subLabelStyle, chart.theme.subTitle,
                    GetSubTitleText(title), Color.clear, title.location.runtimeTextAlignment);
                m_SubLabelObject.SetActive(title.show && title.subLabelStyle.show);
                m_SubLabelObject.transform.localPosition = subTitlePosition + title.subLabelStyle.offset;
            };
            title.refreshComponent();
        }

        public override void OnSerieDataUpdate(int serieIndex)
        {
            if (m_LabelObject != null && FormatterHelper.NeedFormat(component.text))
                m_LabelObject.SetText(GetTitleText(component));
            if (m_SubLabelObject != null && FormatterHelper.NeedFormat(component.subText))
                m_SubLabelObject.SetText(GetSubTitleText(component));
        }

        private string GetTitleText(Title title)
        {
            if (FormatterHelper.NeedFormat(title.text))
            {
                var content = title.text;
                FormatterHelper.ReplaceContent(ref content, -1, title.labelStyle.numericFormatter, null, chart);
                return content;
            }
            else
            {
                return title.text;
            }
        }

        private string GetSubTitleText(Title title)
        {
            if (FormatterHelper.NeedFormat(title.subText))
            {
                var content = title.subText;
                FormatterHelper.ReplaceContent(ref content, -1, title.subLabelStyle.numericFormatter, null, chart);
                return content;
            }
            else
            {
                return title.subText;
            }
        }
    }
}