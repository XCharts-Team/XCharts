using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class CommentHander : MainComponentHandler<Comment>
    {
        private static readonly string s_CommentObjectName = "comment";

        public override void InitComponent()
        {
            var comment = component;
            comment.OnChanged();
            comment.painter = null;
            comment.refreshComponent = delegate ()
            {
                var objName = ChartCached.GetComponentObjectName(comment);
                var commentObj = ChartHelper.AddObject(objName,
                    chart.transform,
                    chart.chartMinAnchor,
                    chart.chartMaxAnchor,
                    chart.chartPivot,
                    chart.chartSizeDelta, -1, chart.childrenNodeNames);
                var siblingIndex = comment.layer == CommentLayer.Upper
                    ? chart.topPainter.transform.GetSiblingIndex() - 1
                    : chart.painter.transform.GetSiblingIndex() + 1;

                commentObj.SetActive(comment.show);
                commentObj.transform.SetSiblingIndex(siblingIndex);
                commentObj.hideFlags = chart.chartHideFlags;
                ChartHelper.HideAllObject(commentObj);
                for (int i = 0; i < comment.items.Count; i++)
                {
                    var item = comment.items[i];
                    var labelStyle = comment.GetLabelStyle(i);
                    item.location.OnChanged();
                    var labelPos = chart.chartPosition + item.location.GetPosition(chart.chartWidth, chart.chartHeight);
                    var label = ChartHelper.AddChartLabel(s_CommentObjectName + i, commentObj.transform, labelStyle, chart.theme.common,
                        GetContent(item), Color.clear, TextAnchor.MiddleCenter);
                    label.SetActive(comment.show && item.show, true);
                    label.SetPosition(labelPos + labelStyle.offset);
                    item.labelObject = label;
                }
            };
            comment.refreshComponent();
        }

        private string GetContent(CommentItem item)
        {
            if (item.content.IndexOf("{") >= 0)
            {
                var content = item.content;
                FormatterHelper.ReplaceContent(ref content, -1, item.labelStyle.numericFormatter, null, chart);
                return content;
            }
            else
            {
                return item.content;
            }
        }

        public override void DrawUpper(VertexHelper vh)
        {
            for (int i = 0; i < component.items.Count; i++)
            {
                var item = component.items[i];
                var markStyle = component.GetMarkStyle(i);
                if (markStyle == null || !markStyle.show) continue;
                var color = ChartHelper.IsClearColor(markStyle.lineStyle.color) ?
                    chart.theme.axis.splitLineColor :
                    markStyle.lineStyle.color;
                var width = markStyle.lineStyle.width == 0 ? 1 : markStyle.lineStyle.width;
                UGL.DrawBorder(vh, item.markRect, width, color);
            }
        }
    }
}