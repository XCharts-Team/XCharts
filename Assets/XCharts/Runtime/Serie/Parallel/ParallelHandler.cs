/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class ParallelHandler : SerieHandler<Parallel>
    {
        public override void Update()
        {
            base.Update();
            UpdateSerieContext();
        }

        public override void DrawSerie(VertexHelper vh)
        {
            var colorIndex = chart.GetLegendRealShowNameIndex(serie.legendName);
            DrawParallelSerie(vh, colorIndex, serie);
        }

        private void UpdateSerieContext()
        {
            if (!chart.isPointerInChart)
                return;

            var themeSymbolSize = chart.theme.serie.lineSymbolSize;

            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = false;

            foreach (var serieData in serie.data)
            {
                var dist = Vector3.Distance(chart.pointerPos, serieData.runtimePosition);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);

                if (dist <= symbolSize)
                {
                    serie.context.pointerItemDataIndex = serieData.index;
                    serie.context.pointerEnter = true;
                    serieData.highlighted = true;
                    chart.RefreshTopPainter();
                }
                else
                {
                    serieData.highlighted = false;
                }
            }
        }

        private void DrawParallelSerie(VertexHelper vh, int colorIndex, Parallel serie)
        {
            if (serie.animation.HasFadeOut()) return;

            var parallel = chart.GetChartComponent<ParallelCoord>(serie.parallelIndex);
            if (parallel == null)
                return;

            var axisCount = parallel.context.parallelAxes.Count;
            if (axisCount <= 0)
                return;

            var animationIndex = serie.animation.GetCurrIndex();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            var isHorizonal = parallel.orient == Orient.Horizonal;
            var lineColor = SerieHelper.GetLineColor(serie, chart.theme, colorIndex, false);
            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);

            float currDetailProgress = parallel.context.x;
            float totalDetailProgress = parallel.context.x + parallel.context.width;
            if (serie.animation.alongWithLinePath)
            {
                //TODO:
            }
            serie.animation.InitProgress(0, currDetailProgress, totalDetailProgress);
            serie.animation.SetDataFinish(0);

            serie.dataPoints.Clear();
            serie.containerIndex = parallel.index;
            serie.containterInstanceId = parallel.instanceId;

            Vector3 sp, np;
            foreach (var serieData in serie.data)
            {
                sp = GetPos(parallel, 0, serieData.data[0], isHorizonal);
                var count = Mathf.Min(axisCount, serieData.data.Count);
                for (int i = 1; i < count; i++)
                {
                    np = GetPos(parallel, i, serieData.data[i], isHorizonal);
                    UGL.DrawLine(vh, sp, np, lineWidth, lineColor);
                    sp = np;
                }
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress - currDetailProgress);
                chart.m_IsPlayingAnimation = true;
                chart.RefreshPainter(serie);
            }
        }

        private static ParallelAxis GetAxis(ParallelCoord parallel, int index)
        {
            if (index >= 0 && index < parallel.context.parallelAxes.Count)
                return parallel.context.parallelAxes[index];
            else
                return null;
        }

        private static Vector3 GetPos(ParallelCoord parallel, int axisIndex, double dataValue, bool isHorizonal)
        {
            var axis = GetAxis(parallel, axisIndex);
            if (axis == null)
                return Vector3.zero;

            var sValueDist = axis.GetDistance(dataValue, axis.context.width);
            return new Vector3(
                isHorizonal ? axis.context.x + sValueDist : axis.context.x,
                isHorizonal ? axis.context.y : axis.context.y + sValueDist);
        }
    }
}