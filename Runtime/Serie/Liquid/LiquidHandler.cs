
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class LiquidHandler : SerieHandler<Liquid>
    {
        private bool m_UpdateLabelText = false;
        private float m_WaveSpeed;

        public override void Update()
        {
            if (m_UpdateLabelText)
            {
                m_UpdateLabelText = false;
                foreach (var serie in chart.series)
                {
                    if (serie is Liquid)
                    {
                        var colorIndex = chart.m_LegendRealShowName.IndexOf(serie.serieName);
                        SerieLabelHelper.SetLiquidLabelText(serie, chart.theme, colorIndex);
                    }
                }
            }
        }

        public override void DrawSerie(VertexHelper vh)
        {
            UpdateRuntimeData(serie);
            DrawVesselBackground(vh, serie);
            DrawLiquid(vh, serie);
            DrawVessel(vh, serie);
        }

        private void UpdateRuntimeData(Liquid serie)
        {
            Vessel vessel;
            if (chart.TryGetChartComponent<Vessel>(out vessel, serie.vesselIndex))
            {
                VesselHelper.UpdateVesselCenter(vessel, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            }
        }

        private void DrawVesselBackground(VertexHelper vh, Liquid serie)
        {
            var vessel = chart.GetChartComponent<Vessel>(serie.vesselIndex);
            if (vessel != null)
            {
                if (vessel.backgroundColor.a != 0)
                {
                    switch (vessel.shape)
                    {
                        case Vessel.Shape.Circle:
                            var cenPos = vessel.context.center;
                            var radius = vessel.context.radius;
                            UGL.DrawCricle(vh, cenPos, vessel.context.innerRadius + vessel.gap, vessel.backgroundColor,
                                chart.settings.cicleSmoothness);
                            UGL.DrawDoughnut(vh, cenPos, vessel.context.innerRadius, vessel.context.innerRadius + vessel.gap,
                                vessel.backgroundColor, Color.clear, chart.settings.cicleSmoothness);
                            break;
                        case Vessel.Shape.Rect:
                            UGL.DrawRectangle(vh, vessel.context.center, vessel.context.width / 2, vessel.context.height / 2,
                                vessel.backgroundColor);
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        private void DrawVessel(VertexHelper vh, Liquid serie)
        {
            var vessel = chart.GetChartComponent<Vessel>(serie.vesselIndex);
            if (vessel != null)
            {
                switch (vessel.shape)
                {
                    case Vessel.Shape.Circle:
                        DrawCirleVessel(vh, vessel);
                        break;
                    case Vessel.Shape.Rect:
                        DrawRectVessel(vh, vessel);
                        break;
                    default:
                        DrawCirleVessel(vh, vessel);
                        break;
                }
            }
        }

        private void DrawCirleVessel(VertexHelper vh, Vessel vessel)
        {
            var cenPos = vessel.context.center;
            var radius = vessel.context.radius;
            var serie = SeriesHelper.GetSerieByVesselIndex(chart.series, vessel.index);
            var vesselColor = VesselHelper.GetColor(vessel, serie, chart.theme, chart.m_LegendRealShowName);
            if (vessel.gap != 0)
            {
                UGL.DrawDoughnut(vh, cenPos, vessel.context.innerRadius, vessel.context.innerRadius + vessel.gap,
                        vessel.backgroundColor, Color.clear, chart.settings.cicleSmoothness);
            }
            UGL.DrawDoughnut(vh, cenPos, radius - vessel.shapeWidth, radius, vesselColor, Color.clear,
                chart.settings.cicleSmoothness);
        }

        private void DrawRectVessel(VertexHelper vh, Vessel vessel)
        {
            var serie = SeriesHelper.GetSerieByVesselIndex(chart.series, vessel.index);
            var vesselColor = VesselHelper.GetColor(vessel, serie, chart.theme, chart.m_LegendRealShowName);
            if (vessel.gap != 0)
            {
                UGL.DrawBorder(vh, vessel.context.center, vessel.context.width,
                    vessel.context.height, vessel.gap, vessel.backgroundColor, 0, vessel.cornerRadius);
            }
            UGL.DrawBorder(vh, vessel.context.center, vessel.context.width + 2 * vessel.gap,
                vessel.context.height + 2 * vessel.gap, vessel.shapeWidth, vesselColor, 0, vessel.cornerRadius);
        }

        private void DrawLiquid(VertexHelper vh, Liquid serie)
        {
            if (!serie.show) return;
            if (serie.animation.HasFadeOut()) return;
            var vessel = chart.GetChartComponent<Vessel>(serie.vesselIndex);
            if (vessel == null) return;
            switch (vessel.shape)
            {
                case Vessel.Shape.Circle:
                    DrawCirleLiquid(vh, serie, vessel);
                    break;
                case Vessel.Shape.Rect:
                    DrawRectLiquid(vh, serie, vessel);
                    break;
                default:
                    DrawCirleLiquid(vh, serie, vessel);
                    break;
            }
        }

        private void DrawCirleLiquid(VertexHelper vh, Liquid serie, Vessel vessel)
        {
            var cenPos = vessel.context.center;
            var radius = vessel.context.innerRadius;
            var serieData = serie.GetSerieData(0);
            if (serieData == null) return;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var value = serieData.GetCurrData(1, dataChangeDuration);
            if (serie.context.checkValue != value)
            {
                serie.context.checkValue = value;
                m_UpdateLabelText = true;
            }
            if (serieData.context.labelPosition != cenPos)
            {
                serieData.context.labelPosition = cenPos;
                m_UpdateLabelText = true;
            }
            if (value <= 0) return;
            var colorIndex = chart.m_LegendRealShowName.IndexOf(serie.serieName);

            var realHig = (float)((value - serie.min) / (serie.max - serie.min) * radius * 2);
            serie.animation.InitProgress(0, realHig);

            var hig = serie.animation.IsFinish() ? realHig : serie.animation.GetCurrDetail();
            var a = Mathf.Abs(radius - hig + (hig > radius ? serie.waveHeight : -serie.waveHeight));
            var diff = Mathf.Sqrt(radius * radius - Mathf.Pow(a, 2));

            var color = SerieHelper.GetItemColor(serie, serieData, chart.theme, colorIndex, false);
            var toColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, colorIndex, false);
            var isNeedGradient = !ChartHelper.IsValueEqualsColor(color, toColor);
            var isFull = hig >= 2 * radius;
            if (hig >= 2 * radius) hig = 2 * radius;
            if (isFull && !isNeedGradient)
            {
                UGL.DrawCricle(vh, cenPos, radius, toColor, chart.settings.cicleSmoothness);
            }
            else
            {
                var startY = cenPos.y - radius + hig;
                var waveStartPos = new Vector3(cenPos.x - diff, startY);
                var waveEndPos = new Vector3(cenPos.x + diff, startY);
                var startX = hig > radius ? cenPos.x - radius : waveStartPos.x;
                var endX = hig > radius ? cenPos.x + radius : waveEndPos.x;

                var step = vessel.smoothness;
                if (step < 0.5f) step = 0.5f;
                var lup = hig > radius ? new Vector3(cenPos.x - radius, cenPos.y) : waveStartPos;
                var ldp = lup;
                var nup = Vector3.zero;
                var ndp = Vector3.zero;
                var angle = 0f;
                m_WaveSpeed += serie.waveSpeed * Time.deltaTime;
                var isStarted = false;
                var isEnded = false;
                var waveHeight = isFull ? 0 : serie.waveHeight;
                while (startX < endX)
                {
                    startX += step;
                    if (startX > endX) startX = endX;
                    if (startX > waveStartPos.x && !isStarted)
                    {
                        startX = waveStartPos.x;
                        isStarted = true;
                    }
                    if (startX > waveEndPos.x && !isEnded)
                    {
                        startX = waveEndPos.x;
                        isEnded = true;
                    }
                    var py = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(Mathf.Abs(cenPos.x - startX), 2));
                    if (startX < waveStartPos.x || startX > waveEndPos.x)
                    {
                        nup = new Vector3(startX, cenPos.y + py);
                    }
                    else
                    {
                        var py2 = waveHeight * Mathf.Sin(1 / serie.waveLength * angle + m_WaveSpeed + serie.waveOffset);
                        var nupY = waveStartPos.y + py2;
                        if (nupY > cenPos.y + py) nupY = cenPos.y + py;
                        else if (nupY < cenPos.y - py) nupY = cenPos.y - py;
                        nup = new Vector3(startX, nupY);
                        angle += step;
                    }
                    ndp = new Vector3(startX, cenPos.y - py);
                    if (!ChartHelper.IsValueEqualsColor(color, toColor))
                    {
                        var colorMin = cenPos.y - radius;
                        var colorMax = startY + serie.waveHeight;
                        var tcolor1 = Color32.Lerp(color, toColor, 1 - (lup.y - colorMin) / (colorMax - colorMin));
                        var tcolor2 = Color32.Lerp(color, toColor, 1 - (ldp.y - colorMin) / (colorMax - colorMin));
                        UGL.DrawQuadrilateral(vh, lup, nup, ndp, ldp, tcolor1, tcolor2);
                    }
                    else
                    {
                        UGL.DrawQuadrilateral(vh, lup, nup, ndp, ldp, color);
                    }
                    lup = nup;
                    ldp = ndp;
                }
            }

            if (serie.waveSpeed != 0 && Application.isPlaying && !isFull)
            {
                chart.RefreshPainter(serie);
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(realHig);
                chart.RefreshPainter(serie);
            }
        }

        private void DrawRectLiquid(VertexHelper vh, Liquid serie, Vessel vessel)
        {
            var cenPos = vessel.context.center;
            var serieData = serie.GetSerieData(0);
            if (serieData == null) return;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var value = serieData.GetCurrData(1, dataChangeDuration);
            if (serie.context.checkValue != value)
            {
                serie.context.checkValue = value;
                m_UpdateLabelText = true;
            }
            if (serieData.context.labelPosition != cenPos)
            {
                serieData.context.labelPosition = cenPos;
                m_UpdateLabelText = true;
            }
            if (value <= 0) return;
            var colorIndex = chart.m_LegendRealShowName.IndexOf(serie.serieName);

            var realHig = (value - serie.min) / (serie.max - serie.min) * vessel.context.height;
            serie.animation.InitProgress(0, (float)realHig);
            var hig = serie.animation.IsFinish() ? realHig : serie.animation.GetCurrDetail();
            var color = SerieHelper.GetItemColor(serie, serieData, chart.theme, colorIndex, false);
            var toColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, colorIndex, false);
            var isNeedGradient = !ChartHelper.IsValueEqualsColor(color, toColor);
            var isFull = hig >= vessel.context.height;
            if (hig >= vessel.context.height) hig = vessel.context.height;
            if (isFull && !isNeedGradient)
            {
                UGL.DrawRectangle(vh, cenPos, vessel.context.width / 2, vessel.context.height / 2, toColor);
            }
            else
            {
                var startY = (float)(cenPos.y - vessel.context.height / 2 + hig);
                var waveStartPos = new Vector3(cenPos.x - vessel.context.width / 2, startY);
                var waveEndPos = new Vector3(cenPos.x + vessel.context.width / 2, startY);
                var startX = waveStartPos.x;
                var endX = waveEndPos.x;

                var step = vessel.smoothness;
                if (step < 0.5f) step = 0.5f;
                var lup = waveStartPos;
                var ldp = new Vector3(startX, vessel.context.center.y - vessel.context.height / 2);
                var nup = Vector3.zero;
                var ndp = Vector3.zero;
                var angle = 0f;
                var isStarted = false;
                var isEnded = false;
                var waveHeight = isFull ? 0 : serie.waveHeight;
                m_WaveSpeed += serie.waveSpeed * Time.deltaTime;
                while (startX < endX)
                {
                    startX += step;
                    if (startX > endX) startX = endX;
                    if (startX > waveStartPos.x && !isStarted)
                    {
                        startX = waveStartPos.x;
                        isStarted = true;
                    }
                    if (startX > waveEndPos.x && !isEnded)
                    {
                        startX = waveEndPos.x;
                        isEnded = true;
                    }
                    var py = Mathf.Sqrt(Mathf.Pow(vessel.context.height, 2) - Mathf.Pow(Mathf.Abs(cenPos.x - startX), 2));
                    if (startX < waveStartPos.x || startX > waveEndPos.x)
                    {
                        nup = new Vector3(startX, cenPos.y + py);
                    }
                    else
                    {
                        var py2 = waveHeight * Mathf.Sin(1 / serie.waveLength * angle + m_WaveSpeed + serie.waveOffset);
                        var nupY = waveStartPos.y + py2;
                        nup = new Vector3(startX, nupY);
                        angle += step;
                    }

                    ndp = new Vector3(startX, cenPos.y - vessel.context.height / 2);
                    if (nup.y > cenPos.y + vessel.context.height / 2)
                    {
                        nup.y = cenPos.y + vessel.context.height / 2;
                    }
                    if (nup.y < cenPos.y - vessel.context.height / 2)
                    {
                        nup.y = cenPos.y - vessel.context.height / 2;
                    }
                    if (!ChartHelper.IsValueEqualsColor(color, toColor))
                    {
                        var colorMin = cenPos.y - vessel.context.height;
                        var colorMax = startY + serie.waveHeight;
                        var tcolor1 = Color32.Lerp(color, toColor, 1 - (lup.y - colorMin) / (colorMax - colorMin));
                        var tcolor2 = Color32.Lerp(color, toColor, 1 - (ldp.y - colorMin) / (colorMax - colorMin));
                        UGL.DrawQuadrilateral(vh, lup, nup, ndp, ldp, tcolor1, tcolor2);
                    }
                    else
                    {
                        UGL.DrawQuadrilateral(vh, lup, nup, ndp, ldp, color);
                    }
                    lup = nup;
                    ldp = ndp;
                }
            }
            if (serie.waveSpeed != 0 && Application.isPlaying && !isFull)
            {
                chart.RefreshPainter(serie);
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(realHig);
                chart.RefreshPainter(serie);
            }
        }
    }
}