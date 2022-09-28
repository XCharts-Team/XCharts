using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
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
            DrawParallelSerie(vh, serie);
        }

        private void UpdateSerieContext() { }

        private void DrawParallelSerie(VertexHelper vh, Parallel serie)
        {
            if (!serie.show) return;
            if (serie.animation.HasFadeOut()) return;

            var parallel = chart.GetChartComponent<ParallelCoord>(serie.parallelIndex);
            if (parallel == null)
                return;

            var axisCount = parallel.context.parallelAxes.Count;
            if (axisCount <= 0)
                return;

            var animationIndex = serie.animation.GetCurrIndex();
            var isHorizonal = parallel.orient == Orient.Horizonal;

            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);

            float currDetailProgress = !isHorizonal ?
                parallel.context.x :
                parallel.context.y;

            float totalDetailProgress = !isHorizonal ?
                parallel.context.x + parallel.context.width :
                parallel.context.y + parallel.context.height;

            serie.animation.InitProgress(currDetailProgress, totalDetailProgress);

            serie.containerIndex = parallel.index;
            serie.containterInstanceId = parallel.instanceId;

            var currProgress = serie.animation.GetCurrDetail();
            var isSmooth = serie.lineType == LineType.Smooth;
            foreach (var serieData in serie.data)
            {
                var count = Mathf.Min(axisCount, serieData.data.Count);
                var lp = Vector3.zero;
                var colorIndex = serie.colorByData?serieData.index : serie.context.colorIndex;
                var lineColor = SerieHelper.GetLineColor(serie, serieData, chart.theme, colorIndex);
                serieData.context.dataPoints.Clear();
                for (int i = 0; i < count; i++)
                {
                    if (animationIndex >= 0 && i > animationIndex) continue;
                    var pos = GetPos(parallel, i, serieData.data[i], isHorizonal);
                    if (!isHorizonal)
                    {
                        if (isSmooth)
                        {
                            serieData.context.dataPoints.Add(pos);
                        }
                        else if (pos.x <= currProgress)
                        {
                            serieData.context.dataPoints.Add(pos);
                        }
                        else
                        {
                            var currProgressStart = new Vector3(currProgress, parallel.context.y - 50);
                            var currProgressEnd = new Vector3(currProgress, parallel.context.y + parallel.context.height + 50);
                            var intersectionPos = Vector3.zero;

                            if (UGLHelper.GetIntersection(lp, pos, currProgressStart, currProgressEnd, ref intersectionPos))
                                serieData.context.dataPoints.Add(intersectionPos);
                            else
                                serieData.context.dataPoints.Add(pos);
                            break;
                        }
                    }
                    else
                    {
                        if (isSmooth)
                        {
                            serieData.context.dataPoints.Add(pos);
                        }
                        else if (pos.y <= currProgress)
                        {
                            serieData.context.dataPoints.Add(pos);
                        }
                        else
                        {
                            var currProgressStart = new Vector3(parallel.context.x - 50, currProgress);
                            var currProgressEnd = new Vector3(parallel.context.x + parallel.context.width + 50, currProgress);
                            var intersectionPos = Vector3.zero;

                            if (UGLHelper.GetIntersection(lp, pos, currProgressStart, currProgressEnd, ref intersectionPos))
                                serieData.context.dataPoints.Add(intersectionPos);
                            else
                                serieData.context.dataPoints.Add(pos);
                            break;
                        }
                    }
                    lp = pos;
                }
                if (isSmooth)
                    UGL.DrawCurves(vh, serieData.context.dataPoints, lineWidth, lineColor,
                        chart.settings.lineSmoothStyle,
                        chart.settings.lineSmoothness,
                        UGL.Direction.XAxis, currProgress, isHorizonal);
                else
                    UGL.DrawLine(vh, serieData.context.dataPoints, lineWidth, lineColor, isSmooth);
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress - currDetailProgress);
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