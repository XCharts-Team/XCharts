
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
            string marker, string itemFormatter, string numericFormatter,
            ref List<SerieParams> paramList, ref string title)
        {
            UpdateCoordSerieParams(ref paramList, ref title, dataIndex, showCategory, category,
                marker, itemFormatter, numericFormatter);
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (serie.IsUseCoord<PolarCoord>())
            {
                DrawPolarLine(vh, serie);
                DrawPolarLineSymbol(vh);
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

        public override void DrawTop(VertexHelper vh)
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
    }
}