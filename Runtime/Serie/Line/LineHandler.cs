using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// For grid coord
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal sealed partial class LineHandler : SerieHandler<Line>
    {
        public override void Update()
        {
            base.Update();
            if (serie.IsUseCoord<GridCoord>())
                UpdateSerieGridContext();
            else if (serie.IsUseCoord<PolarCoord>())
                UpdateSeriePolarContext();
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            ref List<SerieParams> paramList, ref string title)
        {
            UpdateCoordSerieParams(ref paramList, ref title, dataIndex, showCategory, category,
                marker, itemFormatter, numericFormatter, ignoreDataDefaultContent);
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (serie.IsUseCoord<PolarCoord>())
            {
                DrawPolarLine(vh, serie);
                DrawPolarLineSymbol(vh);
                DrawPolarLineArrow(vh, serie);
            }
            else if (serie.IsUseCoord<GridCoord>())
            {
                DrawLineSerie(vh, serie);

                if (!SeriesHelper.IsStack(chart.series))
                {
                    DrawLinePoint(vh, serie);
                    DrawLineArrow(vh, serie);
                }
            }
        }

        public override void DrawUpper(VertexHelper vh)
        {
            if (serie.IsUseCoord<GridCoord>())
            {
                if (SeriesHelper.IsStack(chart.series))
                {
                    DrawLinePoint(vh, serie);
                    DrawLineArrow(vh, serie);
                }
            }
        }

        public override void RefreshEndLabelInternal()
        {
            base.RefreshEndLabelInternal();
            if (m_SerieGrid == null) return;
            if (!serie.animation.IsFinish()) return;
            var endLabelList = m_SerieGrid.context.endLabelList;
            if (endLabelList.Count <= 1) return;

            endLabelList.Sort(delegate(ChartLabel a, ChartLabel b)
            {
                if (a == null || b == null) return 1;
                return b.transform.position.y.CompareTo(a.transform.position.y);
            });
            var lastY = float.NaN;
            for (int i = 0; i < endLabelList.Count; i++)
            {
                var label = endLabelList[i];
                if (label == null) continue;
                if (!label.isAnimationEnd) continue;
                var labelPosition = label.transform.localPosition;
                if (float.IsNaN(lastY))
                {
                    lastY = labelPosition.y;
                }
                else
                {
                    var labelHeight = label.GetTextHeight();
                    if (labelPosition.y + labelHeight > lastY)
                    {
                        label.SetPosition(new Vector3(labelPosition.x, lastY - labelHeight, labelPosition.z));
                    }
                    lastY = label.transform.localPosition.y;
                }
            }
        }
    }
}