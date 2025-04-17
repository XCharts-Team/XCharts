using UnityEngine;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    public sealed class TitleHandler : MainComponentHandler<Title>
    {
        private static readonly string s_TitleObjectName = "title";
        private static readonly string s_SubTitleObjectName = "title_sub";
        private ChartLabel m_LabelObject;
        private ChartLabel m_SubLabelObject;

        public override void InitComponent()
        {
            var title = component;
            title.painter = null;
            title.refreshComponent = delegate ()
            {
                title.OnChanged();
                var titleObject = AddTitleObject(chart, title, chart.theme.title, chart.m_PainterUpper.transform.GetSiblingIndex() + 1);

                m_LabelObject = AddTitleLabel(titleObject.transform, title, chart.theme.title, chart);
                m_SubLabelObject = AddSubTitleLabel(titleObject.transform, title, chart.theme.subTitle, chart);

            };
            title.refreshComponent();
        }

        public static GameObject AddTitleObject(BaseGraph graph, Title title, ComponentTheme componentTheme, int titleSiblingIndex, string objectName = null)
        {
            var anchorMin = title.location.runtimeAnchorMin;
            var anchorMax = title.location.runtimeAnchorMax;
            var pivot = title.location.runtimePivot;
            var objName = objectName == null ? ChartCached.GetComponentObjectName(title) : objectName;
            var titleObject = ChartHelper.AddObject(objName, graph.transform, anchorMin, anchorMax,
                pivot, graph.graphSizeDelta, -1, graph.childrenNodeNames);
            title.gameObject = titleObject;
            title.gameObject.transform.SetSiblingIndex(titleSiblingIndex);
            anchorMin = title.location.runtimeAnchorMin;
            anchorMax = title.location.runtimeAnchorMax;
            pivot = title.location.runtimePivot;

            ChartHelper.UpdateRectTransform(titleObject, anchorMin, anchorMax, pivot, new Vector2(graph.graphWidth, graph.graphHeight));
            var titlePosition = graph.GetTitlePosition(title);
            titleObject.transform.localPosition = titlePosition;
            titleObject.hideFlags = graph.chartHideFlags;
            ChartHelper.HideAllObject(titleObject);
            return titleObject;
        }

        public static ChartLabel AddTitleLabel(Transform parent, Title title, ComponentTheme componentTheme, BaseChart chart = null)
        {
            var m_LabelObject = ChartHelper.AddChartLabel(s_TitleObjectName, parent, title.labelStyle, componentTheme,
                    GetTitleText(title, chart), Color.clear, title.location.runtimeTextAlignment);
            m_LabelObject.SetActive(title.show && title.labelStyle.show, true);
            return m_LabelObject;
        }

        public static ChartLabel AddSubTitleLabel(Transform parent, Title title, ComponentTheme componentTheme, BaseChart chart = null)
        {
            var fontSize = title.labelStyle.textStyle.GetFontSize(componentTheme);
            var subTitlePosition = -new Vector3(0, fontSize + title.itemGap, 0);
            var m_SubLabelObject = ChartHelper.AddChartLabel(s_SubTitleObjectName, parent, title.subLabelStyle, componentTheme,
                    GetSubTitleText(title, chart), Color.clear, title.location.runtimeTextAlignment);
            m_SubLabelObject.SetActive(title.show && title.subLabelStyle.show, true);
            m_SubLabelObject.transform.localPosition = subTitlePosition + title.subLabelStyle.offset;
            return m_SubLabelObject;
        }

        public override void OnSerieDataUpdate(int serieIndex)
        {
            if (m_LabelObject != null && FormatterHelper.NeedFormat(component.text))
                m_LabelObject.SetText(GetTitleText(component, chart));
            if (m_SubLabelObject != null && FormatterHelper.NeedFormat(component.subText))
                m_SubLabelObject.SetText(GetSubTitleText(component, chart));
        }

        private static string GetTitleText(Title title, BaseChart chart)
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

        private static string GetSubTitleText(Title title, BaseChart chart)
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