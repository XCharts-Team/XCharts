using System.Linq;

/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// 水位图
    /// </summary>
    [AddComponentMenu("XCharts/LiquidChart", 22)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public partial class LiquidChart : BaseChart
    {
        [SerializeField] private List<Vessel> m_Vessels = new List<Vessel>();
        private bool m_UpdateLabelText = false;

        protected override void Awake()
        {
            base.Awake();
            UpdateRuntimeValue();
            SerieLabelHelper.UpdateLabelText(m_Series, m_ThemeInfo, m_LegendRealShowName);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "LiquidChart";
            RemoveData();
            RemoveVessel();
            AddVessel(Vessel.defaultVessel);
            var serie = AddSerie(SerieType.Liquid, "serie1");
            serie.min = 0;
            serie.max = 100;
            serie.label.show = true;
            serie.label.fontSize = 40;
            serie.label.formatter = "{d}%";
            serie.label.color = new Color32(70, 70, 240, 255);
            AddData(0, UnityEngine.Random.Range(0, 100));
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            RefreshChart();
        }
#endif

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            UpdateRuntimeValue();
            m_UpdateLabelText = true;
        }

        protected override void Update()
        {
            base.Update();
            if (m_UpdateLabelText)
            {
                m_UpdateLabelText = false;
                SerieLabelHelper.UpdateLabelText(m_Series, m_ThemeInfo, m_LegendRealShowName);
            }
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            UpdateRuntimeValue();
            DrawVesselBackground(vh);
            DrawSeries(vh);
            DrawVessel(vh);
        }

        private void UpdateRuntimeValue()
        {
            for (int i = 0; i < m_Vessels.Count; i++)
            {
                var vessel = m_Vessels[i];
                vessel.index = i;
                VesselHelper.UpdateVesselCenter(vessel, m_ChartPosition, m_ChartWidth, m_ChartHeight);
            }
        }

        private void DrawVesselBackground(VertexHelper vh)
        {
            for (int i = 0; i < m_Vessels.Count; i++)
            {
                var vessel = m_Vessels[i];
                if (vessel.backgroundColor.a != 0)
                {
                    var cenPos = vessel.runtimeCenterPos;
                    var radius = vessel.runtimeRadius;
                    var serie = SeriesHelper.GetSerieByVesselIndex(m_Series, vessel.index);
                    ChartDrawer.DrawCricle(vh, cenPos, vessel.runtimeInnerRadius, vessel.backgroundColor, m_Settings.cicleSmoothness);
                }
            }
        }

        private void DrawVessel(VertexHelper vh)
        {
            for (int i = 0; i < m_Vessels.Count; i++)
            {
                var vessel = m_Vessels[i];
                vessel.index = i;
                DrawCirleVessel(vh, vessel);
            }
        }

        private void DrawCirleVessel(VertexHelper vh, Vessel vessel)
        {
            var cenPos = vessel.runtimeCenterPos;
            var radius = vessel.runtimeRadius;
            var serie = SeriesHelper.GetSerieByVesselIndex(m_Series, vessel.index);
            var vesselColor = VesselHelper.GetColor(vessel, serie, m_ThemeInfo, m_LegendRealShowName);
            ChartDrawer.DrawDoughnut(vh, cenPos, radius - vessel.shapeWidth, radius, vesselColor, Color.clear, m_Settings.cicleSmoothness);
        }

        private void DrawSeries(VertexHelper vh)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                if (!serie.show) continue;
                DrawSerie(vh, serie);
            }
        }

        private void DrawSerie(VertexHelper vh, Serie serie)
        {
            var vessel = GetVessel(serie.vesselIndex);
            if (vessel == null) return;
            var cenPos = vessel.runtimeCenterPos;
            var radius = vessel.runtimeInnerRadius;
            var serieData = serie.GetSerieData(0);
            if (serieData == null) return;

            var value = serieData.GetData(1);
            if (serie.runtimeCheckValue != value)
            {
                serie.runtimeCheckValue = value;
                m_UpdateLabelText = true;
            }
            if (serieData.labelPosition != cenPos)
            {
                serieData.labelPosition = cenPos;
                m_UpdateLabelText = true;
            }
            if (value == 0) return;
            var colorIndex = m_LegendRealShowName.IndexOf(serie.name);

            var realHig = (value - serie.min) / (serie.max - serie.min) * radius * 2;
            serie.animation.InitProgress(1, 0, realHig);

            var hig = serie.animation.IsFinish() ? realHig : serie.animation.GetCurrDetail();
            var a = Mathf.Abs(radius - hig + (hig > radius ? serie.waveHeight : -serie.waveHeight));
            var diff = Mathf.Sqrt(radius * radius - Mathf.Pow(a, 2));

            var color = SerieHelper.GetItemColor(serie, serieData, m_ThemeInfo, colorIndex, false);
            var toColor = SerieHelper.GetItemToColor(serie, serieData, m_ThemeInfo, colorIndex, false);
            var isNeedGradient = !ChartHelper.IsValueEqualsColor(color, toColor);
            var isFull = hig >= 2 * radius;
            if (hig >= 2 * radius) hig = 2 * radius;
            if (isFull && !isNeedGradient)
            {
                ChartDrawer.DrawCricle(vh, cenPos, radius, toColor, m_Settings.cicleSmoothness);
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
                serie.runtimeWaveSpeed += serie.waveSpeed * Time.deltaTime;
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
                        var py2 = waveHeight * Mathf.Sin(1 / serie.waveLength * angle + serie.runtimeWaveSpeed + serie.waveOffset);
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
                        var tcolor1 = Color.Lerp(color, toColor, 1 - (lup.y - colorMin) / (colorMax - colorMin));
                        var tcolor2 = Color.Lerp(color, toColor, 1 - (ldp.y - colorMin) / (colorMax - colorMin));
                        ChartDrawer.DrawPolygon(vh, lup, nup, ndp, ldp, tcolor1, tcolor2);
                    }
                    else
                    {
                        ChartDrawer.DrawPolygon(vh, lup, nup, ndp, ldp, color);
                    }
                    lup = nup;
                    ldp = ndp;
                }
            }

            if (serie.waveSpeed != 0 && Application.isPlaying && !isFull)
            {
                RefreshChart();
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(realHig);
                m_IsPlayingAnimation = true;
                RefreshChart();
            }
        }
    }
}
