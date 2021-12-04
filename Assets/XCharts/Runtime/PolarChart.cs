/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [AddComponentMenu("XCharts/PolarChart", 21)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public partial class PolarChart : BaseChart
    {
        public Polar GetPolar(int index)
        {
            if (index >= 0 && index < m_Polars.Count) return m_Polars[index];
            else return null;
        }

        protected override void InitComponent()
        {
            base.InitComponent();
            if (m_Polars.Count == 0) m_Polars = new List<Polar>() { Polar.defaultPolar };
            if (m_RadiusAxes.Count == 0) m_RadiusAxes = new List<RadiusAxis>() { RadiusAxis.defaultRadiusAxis };
            if (m_AngleAxes.Count == 0) m_AngleAxes = new List<AngleAxis>() { AngleAxis.defaultAngleAxis };
            CheckMinMaxValue();
            UpdateRuntimeValue();
            InitPolars();
            InitRadiusAxes();
            InitAngleAxes();
            tooltip.UpdateToTop();
            tooltip.runtimeAngle = -1;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Polars.Clear();
            m_RadiusAxes.Clear();
            m_AngleAxes.Clear();
            InitComponent();
            title.text = "PolarChart";
            tooltip.type = Tooltip.Type.Corss;
            RemoveData();
            ResetValuePolar();
            Awake();
        }

        private void ResetValuePolar()
        {
            m_AngleAxes[0].type = Axis.AxisType.Value;
            m_AngleAxes[0].minMaxType = Axis.AxisMinMaxType.Custom;
            m_AngleAxes[0].min = 0;
            m_AngleAxes[0].max = 360;
            AddSerie(SerieType.Line, "line1");
            for (int i = 0; i <= 360; i++)
            {
                var t = i / 180f * Mathf.PI;
                var r = Mathf.Sin(2 * t) * Mathf.Cos(2 * t) * 2;
                AddData(0, Mathf.Abs(r), i);
            }
        }

        private void ResetCategoryPolar()
        {
            m_AngleAxes[0].type = Axis.AxisType.Category;
            AddSerie(SerieType.Bar, "line1");
            for (int i = 0; i <= 13; i++)
            {
                m_AngleAxes[0].AddData("bar" + i);
                AddData(0, UnityEngine.Random.Range(0, 10));
            }
        }
#endif

        protected override void SetAllComponentDirty()
        {
            base.SetAllComponentDirty();
            foreach (var axis in m_RadiusAxes) axis.SetAllDirty();
            foreach (var axis in m_AngleAxes) axis.SetAllDirty();
            CheckMinMaxValue();
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            foreach (var axis in m_RadiusAxes) axis.SetAllDirty();
            foreach (var axis in m_AngleAxes) axis.SetAllDirty();
            UpdateRuntimeValue();
        }

        private void InitPolars()
        {
            for (int i = 0; i < m_Polars.Count; i++)
            {
                m_Polars[i].index = i;
            }
        }

        private void InitRadiusAxes()
        {
            for (int i = 0; i < m_RadiusAxes.Count; i++)
            {
                var radiusAxis = m_RadiusAxes[i];
                radiusAxis.index = i;
                InitRadiusAxis(radiusAxis);
            }
        }

        private void InitRadiusAxis(RadiusAxis axis)
        {
            var m_Polar = GetPolar(axis.index);
            if (m_Polars == null) return;
            var m_AngleAxis = GetAngleAxis(m_Polar.index);
            if (m_AngleAxis == null) return;
            PolarHelper.UpdatePolarCenter(m_Polar, m_ChartPosition, m_ChartWidth, m_ChartHeight);
            axis.runtimeAxisLabelList.Clear();
            var radius = m_Polar.runtimeRadius;
            var objName = "axis_radius" + axis.index;
            var axisObj = ChartHelper.AddObject(objName, transform, graphAnchorMin,
                graphAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(axis.show && axis.axisLabel.show);
            axisObj.hideFlags = chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var textStyle = axis.axisLabel.textStyle;
            var splitNumber = AxisHelper.GetSplitNumber(axis, radius, null);
            var totalWidth = 0f;
            var startAngle = m_AngleAxis.runtimeStartAngle;
            var cenPos = m_Polar.runtimeCenterPos;
            var txtHig = textStyle.GetFontSize(m_Theme.axis) + 2;
            var dire = ChartHelper.GetDire(startAngle, true).normalized;
            var tickWidth = axis.axisTick.GetLength(m_Theme.radiusAxis.tickWidth);
            var tickVetor = ChartHelper.GetVertialDire(dire)
                * (tickWidth + axis.axisLabel.margin);
            for (int i = 0; i < splitNumber; i++)
            {
                var labelWidth = AxisHelper.GetScaleWidth(axis, radius, i, null);
                var inside = axis.axisLabel.inside;
                var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
                var labelName = AxisHelper.GetLabelName(axis, radius, i, axis.runtimeMinValue, axis.runtimeMaxValue,
                    null, isPercentStack);
                var label = ChartHelper.AddAxisLabelObject(splitNumber, i, objName + i, axisObj.transform, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(labelWidth, txtHig), axis, theme.axis,
                    labelName);
                if (i == 0) axis.axisLabel.SetRelatedText(label.label, labelWidth);
                label.label.SetAlignment(textStyle.GetAlignment(TextAnchor.MiddleCenter));
                label.SetText(labelName);
                var pos = ChartHelper.GetPos(cenPos, totalWidth, startAngle, true) + tickVetor;
                label.SetPosition(pos);
                AxisHelper.AdjustRadiusAxisLabelPos(label.label, pos, cenPos, txtHig, Vector3.zero);
                axis.runtimeAxisLabelList.Add(label);

                totalWidth += labelWidth;
            }
            if (tooltip.runtimeGameObject)
            {
                Vector2 privot = new Vector2(0.5f, 1);
                var labelParent = tooltip.runtimeGameObject.transform;
                var labelName = ChartCached.GetAxisTooltipLabel(objName);
                GameObject labelObj = ChartHelper.AddTooltipLabel(labelName, labelParent, m_Theme, privot);
                axis.SetTooltipLabel(labelObj);
                axis.SetTooltipLabelColor(m_Theme.tooltip.labelBackgroundColor, m_Theme.tooltip.labelTextColor);
                axis.SetTooltipLabelActive(axis.show && tooltip.show && tooltip.type == Tooltip.Type.Corss);
            }
        }

        private AngleAxis GetAngleAxis(int polarIndex)
        {
            foreach (var axis in m_AngleAxes)
            {
                if (axis.polarIndex == polarIndex) return axis;
            }
            return null;
        }
        private RadiusAxis GetRadiusAxis(int polarIndex)
        {
            foreach (var axis in m_RadiusAxes)
            {
                if (axis.polarIndex == polarIndex) return axis;
            }
            return null;
        }

        private void InitAngleAxes()
        {
            for (int i = 0; i < m_AngleAxes.Count; i++)
            {
                var angleAxis = m_AngleAxes[i];
                angleAxis.index = i;
                InitAngleAxis(angleAxis);
            }
        }

        private void InitAngleAxis(AngleAxis axis)
        {
            var m_Polar = GetPolar(axis.polarIndex);
            if (m_Polars == null) return;
            PolarHelper.UpdatePolarCenter(m_Polar, m_ChartPosition, m_ChartWidth, m_ChartHeight);
            var radius = m_Polar.runtimeRadius;
            axis.runtimeAxisLabelList.Clear();

            string objName = "axis_angle" + axis.index;
            var axisObj = ChartHelper.AddObject(objName, transform, graphAnchorMin,
                graphAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(axis.show);
            axisObj.hideFlags = chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var splitNumber = AxisHelper.GetSplitNumber(axis, radius, null);
            var totalAngle = axis.runtimeStartAngle;
            var total = 360;
            var cenPos = m_Polar.runtimeCenterPos;
            var txtHig = axis.axisLabel.textStyle.GetFontSize(m_Theme.axis) + 2;
            var margin = axis.axisLabel.margin;
            var isCategory = axis.IsCategory();
            var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
            for (int i = 0; i < splitNumber; i++)
            {
                float scaleAngle = AxisHelper.GetScaleWidth(axis, total, i, null);
                bool inside = axis.axisLabel.inside;
                var labelName = AxisHelper.GetLabelName(axis, total, i, axis.runtimeMinValue, axis.runtimeMaxValue,
                    null, isPercentStack);
                var label = ChartHelper.AddAxisLabelObject(splitNumber, i, objName + i, axisObj.transform, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(scaleAngle, txtHig), axis,
                    theme.axis, labelName);
                label.label.SetAlignment(axis.axisLabel.textStyle.GetAlignment(TextAnchor.MiddleCenter));
                var pos = ChartHelper.GetPos(cenPos, radius + margin,
                    isCategory ? (totalAngle + scaleAngle / 2) : totalAngle, true);
                AxisHelper.AdjustCircleLabelPos(label, pos, cenPos, txtHig, Vector3.zero);
                if (i == 0) axis.axisLabel.SetRelatedText(label.label, scaleAngle);
                axis.runtimeAxisLabelList.Add(label);

                totalAngle += scaleAngle;
            }
            if (tooltip.runtimeGameObject)
            {
                Vector2 privot = new Vector2(0.5f, 0.5f);
                var labelParent = tooltip.runtimeGameObject.transform;
                GameObject labelObj = ChartHelper.AddTooltipLabel(ChartCached.GetAxisTooltipLabel(objName), labelParent,
                    m_Theme, privot, privot, privot, new Vector2(10, txtHig));
                axis.SetTooltipLabel(labelObj);
                axis.SetTooltipLabelColor(m_Theme.tooltip.labelBackgroundColor, m_Theme.tooltip.labelTextColor);
                axis.SetTooltipLabelActive(axis.show && tooltip.show && tooltip.type == Tooltip.Type.Corss);
            }
        }

        protected override void Update()
        {
            base.Update();
            CheckMinMaxValue();
        }

        private void CheckMinMaxValue()
        {
            foreach (var axis in m_RadiusAxes) UpdateAxisMinMaxValue(axis);
            foreach (var axis in m_AngleAxes) UpdateAxisMinMaxValue(axis);
        }

        private void UpdateAxisMinMaxValue(Axis axis, bool updateChart = true)
        {
            if (axis.IsCategory() || !axis.show) return;
            double tempMinValue = 0;
            double tempMaxValue = 0;
            if (axis is RadiusAxis)
            {
                SeriesHelper.GetXMinMaxValue(m_Series, null, axis.polarIndex, true, axis.inverse, out tempMinValue,
                    out tempMaxValue, true);
            }
            else
            {
                SeriesHelper.GetYMinMaxValue(m_Series, null, axis.polarIndex, true, axis.inverse, out tempMinValue,
                    out tempMaxValue, true);
            }
            AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
            if (tempMinValue != axis.runtimeMinValue || tempMaxValue != axis.runtimeMaxValue)
            {
                m_IsPlayingAnimation = true;
                var needCheck = !m_IsPlayingAnimation && axis.runtimeLastCheckInverse == axis.inverse;
                axis.UpdateMinValue(tempMinValue, needCheck);
                axis.UpdateMaxValue(tempMaxValue, needCheck);
                axis.runtimeZeroXOffset = 0;
                axis.runtimeZeroYOffset = 0;
                axis.runtimeLastCheckInverse = axis.inverse;
                if (updateChart)
                {
                    UpdateAxisLabelText(axis);
                    RefreshChart();
                }
            }
            if (axis.IsValueChanging(500) && !m_IsPlayingAnimation)
            {
                UpdateAxisLabelText(axis);
                RefreshChart();
            }
        }

        protected void UpdateAxisLabelText(Axis axis)
        {
            var polar = GetPolar(axis.polarIndex);
            var runtimeWidth = axis is RadiusAxis ? polar.runtimeRadius : 360;
            var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
            axis.UpdateLabelText(runtimeWidth, null, isPercentStack, 500);
        }

        protected override void DrawPainterBase(VertexHelper vh)
        {
            base.DrawPainterBase(vh);
            DrawPolar(vh);
            DrawAngleAxis(vh);
            DrawRadiusAxis(vh);
            DrawSerie(vh);
        }

        private void UpdateRuntimeValue()
        {
            foreach (var polar in m_Polars)
                PolarHelper.UpdatePolarCenter(polar, m_ChartPosition, m_ChartWidth, m_ChartHeight);
            foreach (var axis in m_AngleAxes)
                axis.runtimeStartAngle = 90 - axis.startAngle;
        }

        private void DrawPolar(VertexHelper vh)
        {
            UpdateRuntimeValue();
            foreach (var polar in m_Polars)
            {
                if (!ChartHelper.IsClearColor(polar.backgroundColor))
                {
                    UGL.DrawCricle(vh, polar.runtimeCenterPos, polar.runtimeRadius, polar.backgroundColor);
                }
            }
        }

        private void DrawRadiusAxis(VertexHelper vh)
        {
            foreach (var radiusAxis in m_RadiusAxes)
            {
                var polar = GetPolar(radiusAxis.polarIndex);
                if (polar == null) continue;
                var angleAxis = GetAngleAxis(polar.index);
                if (angleAxis == null) continue;
                var startAngle = angleAxis.runtimeStartAngle;
                var radius = polar.runtimeRadius;
                var cenPos = polar.runtimeCenterPos;
                var size = AxisHelper.GetScaleNumber(radiusAxis, radius, null);
                var totalWidth = 0f;
                var dire = ChartHelper.GetDire(startAngle, true).normalized;
                var tickWidth = radiusAxis.axisTick.GetLength(m_Theme.radiusAxis.tickWidth);
                var tickLength = radiusAxis.axisTick.GetLength(m_Theme.radiusAxis.tickLength);
                var tickVetor = ChartHelper.GetVertialDire(dire) * tickLength;
                for (int i = 0; i < size - 1; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(radiusAxis, radius, i);
                    var pos = ChartHelper.GetPos(cenPos, totalWidth, startAngle, true);
                    if (radiusAxis.show && radiusAxis.splitLine.show)
                    {
                        var outsideRaidus = totalWidth + radiusAxis.splitLine.GetWidth(m_Theme.radiusAxis.splitLineWidth) * 2;
                        var splitLineColor = radiusAxis.splitLine.GetColor(m_Theme.radiusAxis.splitLineColor);
                        UGL.DrawDoughnut(vh, cenPos, totalWidth, outsideRaidus, splitLineColor, Color.clear);
                    }
                    if (radiusAxis.show && radiusAxis.axisTick.show)
                    {
                        UGL.DrawLine(vh, pos, pos + tickVetor, tickWidth, m_Theme.axis.lineColor);
                    }
                    totalWidth += scaleWidth;
                }
                if (radiusAxis.show && radiusAxis.axisLine.show)
                {
                    var lineStartPos = polar.runtimeCenterPos - dire * tickWidth;
                    var lineEndPos = polar.runtimeCenterPos + dire * (radius + tickWidth);
                    var lineWidth = radiusAxis.axisLine.GetWidth(m_Theme.polar.lineWidth);
                    UGL.DrawLine(vh, lineStartPos, lineEndPos, lineWidth, m_Theme.axis.lineColor);
                }
            }
        }

        private void DrawAngleAxis(VertexHelper vh)
        {
            foreach (var m_AngleAxis in m_AngleAxes)
            {
                var m_Polar = GetPolar(m_AngleAxis.polarIndex);
                var radius = m_Polar.runtimeRadius;
                var cenPos = m_Polar.runtimeCenterPos;
                var total = 360;
                var size = AxisHelper.GetScaleNumber(m_AngleAxis, total, null);
                var currAngle = m_AngleAxis.runtimeStartAngle;
                var tickWidth = m_AngleAxis.axisTick.GetWidth(m_Theme.angleAxis.tickWidth);
                var tickLength = m_AngleAxis.axisTick.GetLength(m_Theme.angleAxis.tickLength);
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(m_AngleAxis, total, i);
                    var pos = ChartHelper.GetPos(cenPos, radius, currAngle, true);
                    if (m_AngleAxis.show && m_AngleAxis.splitLine.show)
                    {
                        var splitLineColor = m_AngleAxis.splitLine.GetColor(m_Theme.angleAxis.splitLineColor);
                        var lineWidth = m_AngleAxis.splitLine.GetWidth(m_Theme.angleAxis.splitLineWidth);
                        UGL.DrawLine(vh, cenPos, pos, lineWidth, splitLineColor);
                    }
                    if (m_AngleAxis.show && m_AngleAxis.axisTick.show)
                    {
                        var tickY = radius + tickLength;
                        var tickPos = ChartHelper.GetPos(cenPos, tickY, currAngle, true);
                        UGL.DrawLine(vh, pos, tickPos, tickWidth, m_Theme.axis.lineColor);
                    }
                    currAngle += scaleWidth;
                }
                if (m_AngleAxis.show && m_AngleAxis.axisLine.show)
                {
                    var lineWidth = m_AngleAxis.axisLine.GetWidth(m_Theme.angleAxis.lineWidth);
                    var outsideRaidus = radius + lineWidth * 2;
                    UGL.DrawDoughnut(vh, cenPos, radius, outsideRaidus, m_Theme.axis.lineColor, Color.clear);
                }
            }
        }

        private void DrawSerie(VertexHelper vh)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                serie.index = i;
                if (!serie.show) continue;
                switch (serie.type)
                {
                    case SerieType.Line:
                        DrawPolarLine(vh, serie);
                        break;
                    case SerieType.Bar:
                        break;
                    case SerieType.Scatter:
                    case SerieType.EffectScatter:
                        break;
                }

            }
            DrawPolarLineSymbol(vh);
        }

        private void DrawPolarLine(VertexHelper vh, Serie serie)
        {
            var m_Polar = GetPolar(serie.polarIndex);
            if (m_Polar == null) return;
            var m_AngleAxis = GetAngleAxis(m_Polar.index);
            var m_RadiusAxis = GetRadiusAxis(m_Polar.index);
            if (m_AngleAxis == null || m_RadiusAxis == null) return;
            var startAngle = m_AngleAxis.runtimeStartAngle;
            var radius = m_Polar.runtimeRadius;
            var datas = serie.data;
            if (datas.Count <= 0) return;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double min = m_RadiusAxis.GetCurrMinValue(dataChangeDuration);
            double max = m_RadiusAxis.GetCurrMaxValue(dataChangeDuration);
            var firstSerieData = datas[0];
            var startPos = GetPolarPos(m_Polar, m_AngleAxis, firstSerieData, min, max, radius);
            var nextPos = Vector3.zero;
            var lineColor = SerieHelper.GetLineColor(serie, m_Theme, serie.index, serie.highlighted);
            var lineWidth = serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
            float currDetailProgress = 0;
            float totalDetailProgress = datas.Count;
            serie.animation.InitProgress(serie.dataPoints.Count, currDetailProgress, totalDetailProgress);
            serie.animation.SetDataFinish(0);
            for (int i = 1; i < datas.Count; i++)
            {
                if (serie.animation.CheckDetailBreak(i)) break;
                var serieData = datas[i];
                nextPos = GetPolarPos(m_Polar, m_AngleAxis, datas[i], min, max, radius);
                UGL.DrawLine(vh, startPos, nextPos, lineWidth, lineColor);
                startPos = nextPos;
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, m_Theme.serie.lineSymbolSize));
                m_IsPlayingAnimation = true;
                RefreshChart();
            }
        }

        private void DrawPolarBar(VertexHelper vh, Serie serie)
        {
        }

        private void DrawPolarLineSymbol(VertexHelper vh)
        {
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series.GetSerie(n);
                if (!serie.show) continue;
                if (serie.type != SerieType.Line) continue;
                var count = serie.dataCount;
                for (int i = 0; i < count; i++)
                {
                    var serieData = serie.GetSerieData(i);
                    var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                    if (ChartHelper.IsIngore(serieData.runtimePosition)) continue;
                    bool highlight = serieData.highlighted || serie.highlighted;
                    if ((!symbol.show || !symbol.ShowSymbol(i, count) || serie.IsPerformanceMode()) && !serieData.highlighted) continue;
                    float symbolSize = highlight
                        ? symbol.GetSelectedSize(serieData.data, m_Theme.serie.lineSymbolSize)
                        : symbol.GetSize(serieData.data, m_Theme.serie.lineSymbolSize);
                    var symbolColor = SerieHelper.GetItemColor(serie, serieData, m_Theme, n, highlight);
                    var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, m_Theme, n, highlight);
                    var backgroundColor = SerieHelper.GetItemBackgroundColor(serie, serieData, m_Theme, n, highlight, false);
                    var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, m_Theme, highlight);
                    var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                    symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                    DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, serieData.runtimePosition, symbolColor,
                        symbolToColor, backgroundColor, symbol.gap, cornerRadius);
                }
            }
        }

        protected override void DrawTooltip(VertexHelper vh)
        {
            if (tooltip.runtimeAngle < 0) return;
            var m_Polar = GetPolar(tooltip.runtimePolarIndex);
            var m_AngleAxis = GetAngleAxis(m_Polar.index);
            var lineColor = TooltipHelper.GetLineColor(tooltip, m_Theme);
            var lineType = tooltip.lineStyle.GetType(m_Theme.tooltip.lineType);
            var lineWidth = tooltip.lineStyle.GetWidth(m_Theme.tooltip.lineWidth);
            var cenPos = m_Polar.runtimeCenterPos;
            var radius = m_Polar.runtimeRadius;
            var sp = m_Polar.runtimeCenterPos;
            var tooltipAngle = tooltip.runtimeAngle + m_AngleAxis.runtimeStartAngle;
            var ep = ChartHelper.GetPos(sp, radius, tooltipAngle, true);

            switch (tooltip.type)
            {
                case Tooltip.Type.Corss:
                    ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                    var dist = Vector2.Distance(pointerPos, cenPos);
                    if (dist > radius) dist = radius;
                    var outsideRaidus = dist + tooltip.lineStyle.GetWidth(m_Theme.tooltip.lineWidth) * 2;
                    UGL.DrawDoughnut(vh, cenPos, dist, outsideRaidus, lineColor, Color.clear);
                    break;
                case Tooltip.Type.Line:
                    ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                    break;
                case Tooltip.Type.Shadow:
                    UGL.DrawSector(vh, cenPos, radius, lineColor, tooltipAngle - 2, tooltipAngle + 2, settings.cicleSmoothness);
                    break;
            }
        }

        private Vector3 GetPolarPos(Polar m_Polar, AngleAxis m_AngleAxis, SerieData serieData, double min, double max, float polarRadius)
        {
            var angle = 0f;
            if (!m_AngleAxis.clockwise)
            {
                angle = m_AngleAxis.runtimeStartAngle - (float)serieData.GetData(1);
            }
            else
            {
                angle = m_AngleAxis.runtimeStartAngle + (float)serieData.GetData(1);
            }
            angle = (angle + 360) % 360;
            var value = serieData.GetData(0);
            var radius = (float)((value - min) / (max - min) * polarRadius);
            serieData.runtimeAngle = angle;
            serieData.runtimePosition = ChartHelper.GetPos(m_Polar.runtimeCenterPos, radius, angle, true);
            return serieData.runtimePosition;
        }

        protected override void CheckTootipArea(Vector2 local, bool isActivedOther)
        {
            if (isActivedOther) return;
            tooltip.runtimePolarIndex = GetPointerInPoloar(local);
            if (tooltip.runtimePolarIndex < 0)
            {
                tooltip.runtimeAngle = -1;
                if (tooltip.IsActive())
                {
                    foreach (var kv in tooltip.runtimeSerieIndex)
                    {
                        var serie = m_Series.GetSerie(kv.Key);
                        foreach (var dataIndex in kv.Value)
                        {
                            serie.GetSerieData(dataIndex).highlighted = false;
                        }
                    }
                    tooltip.ClearSerieDataIndex();
                    tooltip.SetActive(false);
                    foreach (var axis in m_AngleAxes) axis.SetTooltipLabelActive(false);
                    foreach (var axis in m_RadiusAxes) axis.SetTooltipLabelActive(false);
                    RefreshChart();
                }
                return;
            }
            var m_Polar = GetPolar(tooltip.runtimePolarIndex);
            var m_AngleAxis = GetAngleAxis(m_Polar.index);
            tooltip.ClearSerieDataIndex();
            Vector2 dir = local - new Vector2(m_Polar.runtimeCenterPos.x, m_Polar.runtimeCenterPos.y);
            float angle = ChartHelper.GetAngle360(Vector2.up, dir);

            foreach (var serie in m_Series.list)
            {
                switch (serie.type)
                {
                    case SerieType.Line:
                        bool refresh = false;
                        var count = serie.data.Count;
                        SerieHelper.UpdateMinMaxData(serie, 1, -1);
                        var diff = (serie.runtimeDataMax - serie.runtimeDataMin) / (count - 1);
                        for (int j = 0; j < count; j++)
                        {
                            var serieData = serie.data[j];
                            var flag = Math.Abs(serieData.runtimeAngle - angle) < Math.Abs(diff / 2);
                            if (serieData.highlighted != flag)
                            {
                                refresh = true;
                            }
                            serieData.highlighted = flag;
                            if (flag)
                            {
                                tooltip.runtimeAngle = (serieData.runtimeAngle - m_AngleAxis.runtimeStartAngle + 360) % 360;
                                tooltip.AddSerieDataIndex(serie.index, j);
                            }
                        }
                        if (refresh) RefreshChart();
                        break;
                    case SerieType.Bar:
                        break;
                    case SerieType.Scatter:
                    case SerieType.EffectScatter:
                        break;
                }
            }
            tooltip.UpdateContentPos(local + tooltip.offset);
            UpdateTooltip();
            if (tooltip.type == Tooltip.Type.Corss)
            {
                RefreshChart();
            }
        }

        private int GetPointerInPoloar(Vector2 local)
        {
            for (int i = 0; i < m_Polars.Count; i++)
            {
                var polar = m_Polars[i];
                polar.index = i;
                var dist = Vector2.Distance(local, polar.runtimeCenterPos);
                if (dist <= polar.runtimeRadius)
                {
                    return polar.index;
                }
            }
            return -1;
        }

        private float GetAngleDiff(SerieData nextData, SerieData serieData, float angle)
        {
            var nextAngle = nextData.runtimeAngle;
            var lastAngle = serieData.runtimeAngle;
            var diff = 0f;
            if (nextAngle > 270 && lastAngle < 90)
            {
                diff = 360 - nextAngle + lastAngle;
            }
            else
            {
                diff = nextAngle - lastAngle;
            }
            return Mathf.Abs(diff);
        }

        protected override void UpdateTooltip()
        {
            base.UpdateTooltip();
            var showTooltip = tooltip.isAnySerieDataIndex();
            if (showTooltip)
            {
                var m_AngleAxis = GetAngleAxis(tooltip.runtimePolarIndex);
                var content = TooltipHelper.GetPolarFormatterContent(tooltip, this, m_AngleAxis);
                TooltipHelper.SetContentAndPosition(tooltip, content, chartRect);
                UdpateTooltipLabel();
            }
            tooltip.SetActive(showTooltip);
        }

        private void UdpateTooltipLabel()
        {
            if (tooltip.type != Tooltip.Type.Corss) return;
            var m_Polar = GetPolar(tooltip.runtimePolarIndex);
            if (m_Polar == null) return;
            var m_AngleAxis = GetAngleAxis(m_Polar.index);
            var m_RadiusAxis = GetRadiusAxis(m_Polar.index);
            var cenPos = m_Polar.runtimeCenterPos;
            var radius = m_Polar.runtimeRadius;
            m_AngleAxis.SetTooltipLabelActive(true);
            m_RadiusAxis.SetTooltipLabelActive(true);
            m_AngleAxis.UpdateTooptipLabelText(ChartCached.FloatToStr(tooltip.runtimeAngle));
            var tooltipAngle = tooltip.runtimeAngle + m_AngleAxis.runtimeStartAngle;
            var ep = ChartHelper.GetPos(cenPos, radius + 5, tooltipAngle, true);
            m_AngleAxis.UpdateTooltipLabelPos(ep);

            var dist = Vector2.Distance(pointerPos, cenPos);
            if (dist > radius) dist = radius;
            double min = m_RadiusAxis.runtimeMinValue;
            double max = m_RadiusAxis.runtimeMaxValue;
            var value = min + dist / radius * m_RadiusAxis.runtimeMinMaxRange;
            m_RadiusAxis.UpdateTooptipLabelText(ChartCached.FloatToStr(value));
            m_RadiusAxis.UpdateTooltipLabelPos(ChartHelper.GetPos(cenPos, dist, m_AngleAxis.runtimeStartAngle, true));
        }
    }
}
;