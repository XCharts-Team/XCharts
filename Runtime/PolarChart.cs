/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/PolarChart", 21)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public partial class PolarChart : BaseChart
    {
        [SerializeField] Polar m_Polar = Polar.defaultPolar;
        [SerializeField] private RadiusAxis m_RadiusAxis = RadiusAxis.defaultRadiusAxis;
        [SerializeField] private AngleAxis m_AngleAxis = AngleAxis.defaultAngleAxis;

        private bool m_CheckMinMaxValue = false;

        protected override void Awake()
        {
            base.Awake();
            m_CheckMinMaxValue = false;
            CheckMinMaxValue();
            UpdateRuntimeValue();
            InitRadiusAxis(m_RadiusAxis);
            InitAngleAxis(m_AngleAxis);
            m_Tooltip.UpdateToTop();
        }


#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "PolarChart";
            m_Tooltip.type = Tooltip.Type.Line;
            RemoveData();
            ResetValuePolar();
            Awake();
        }

        private void ResetValuePolar()
        {
            m_AngleAxis.type = Axis.AxisType.Value;
            m_AngleAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            m_AngleAxis.min = 0;
            m_AngleAxis.max = 360;
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
            m_AngleAxis.type = Axis.AxisType.Category;
            AddSerie(SerieType.Bar, "line1");
            for (int i = 0; i <= 13; i++)
            {
                m_AngleAxis.AddData("bar" + i);
                AddData(0, Random.Range(0, 10));
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            m_RadiusAxis.SetAllDirty();
            m_AngleAxis.SetAllDirty();
            CheckMinMaxValue();
        }
#endif

        protected override void CheckComponent()
        {
            if (m_Polar.anyDirty)
            {
                if (m_Polar.componentDirty)
                {
                    m_AngleAxis.SetComponentDirty();
                    m_RadiusAxis.SetComponentDirty();
                }
                if (m_Polar.vertsDirty) RefreshChart();
                m_Polar.ClearDirty();
            }
            if (m_AngleAxis.anyDirty || m_RadiusAxis.anyDirty)
            {
                if (m_AngleAxis.componentDirty || m_RadiusAxis.componentDirty)
                {
                    UpdateRuntimeValue();
                    InitAngleAxis(m_AngleAxis);
                    InitRadiusAxis(m_RadiusAxis);
                }
                if (m_AngleAxis.vertsDirty || m_RadiusAxis.vertsDirty) RefreshChart();
                m_AngleAxis.ClearDirty();
                m_RadiusAxis.ClearDirty();
            }
            base.CheckComponent();
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();
            m_RadiusAxis.SetAllDirty();
            m_AngleAxis.SetAllDirty();
            UpdateRuntimeValue();
        }

        private void InitRadiusAxis(RadiusAxis axis)
        {
            PolarHelper.UpdatePolarCenter(m_Polar, m_ChartPosition, m_ChartWidth, m_ChartHeight);
            var radius = m_Polar.runtimeRadius;
            axis.axisLabelTextList.Clear();
            string objName = "axis_radius";
            var axisObj = ChartHelper.AddObject(objName, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(axis.show && axis.axisLabel.show);
            axisObj.hideFlags = chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var labelColor = ChartHelper.IsClearColor(axis.axisLabel.color) ?
                (Color)m_ThemeInfo.axisTextColor :
                axis.axisLabel.color;
            int splitNumber = AxisHelper.GetSplitNumber(axis, radius, null);
            float totalWidth = 0;
            var startAngle = m_AngleAxis.runtimeStartAngle;
            var cenPos = m_Polar.runtimeCenterPos;
            var txtHig = axis.axisLabel.fontSize + 2;
            var dire = ChartHelper.GetDire(startAngle, true).normalized;
            var tickVetor = ChartHelper.GetVertialDire(dire) * (m_RadiusAxis.axisTick.length + m_RadiusAxis.axisLabel.margin);
            for (int i = 0; i < splitNumber; i++)
            {
                float labelWidth = AxisHelper.GetScaleWidth(axis, radius, i, null);
                bool inside = axis.axisLabel.inside;
                Text txt = ChartHelper.AddTextObject(objName + i, axisObj.transform,
                    m_ThemeInfo.font, labelColor, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(labelWidth, txtHig),
                    axis.axisLabel.fontSize, axis.axisLabel.rotate, axis.axisLabel.fontStyle);
                if (i == 0) axis.axisLabel.SetRelatedText(txt, labelWidth);
                var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
                txt.text = AxisHelper.GetLabelName(axis, radius, i, axis.runtimeMinValue, axis.runtimeMaxValue, null,
                    isPercentStack);
                txt.gameObject.SetActive(axis.show &&
                    (axis.axisLabel.interval == 0 || i % (axis.axisLabel.interval + 1) == 0));
                var pos = ChartHelper.GetPos(cenPos, totalWidth, startAngle, true) + tickVetor;
                txt.transform.localPosition = pos;
                AxisHelper.AdjustRadiusAxisLabelPos(txt, pos, cenPos, txtHig, Vector3.zero);
                axis.axisLabelTextList.Add(txt);

                totalWidth += labelWidth;
            }
            if (m_Tooltip.runtimeGameObject)
            {
                Vector2 privot = new Vector2(0.5f, 1);
                var labelParent = m_Tooltip.runtimeGameObject.transform;
                GameObject labelObj = ChartHelper.AddTooltipLabel(ChartCached.GetAxisTooltipLabel(objName), labelParent, m_ThemeInfo.font, privot);
                axis.SetTooltipLabel(labelObj);
                axis.SetTooltipLabelColor(m_ThemeInfo.tooltipBackgroundColor, m_ThemeInfo.tooltipTextColor);
                axis.SetTooltipLabelActive(axis.show && m_Tooltip.show && m_Tooltip.type == Tooltip.Type.Corss);
            }
        }

        private void InitAngleAxis(AngleAxis axis)
        {
            PolarHelper.UpdatePolarCenter(m_Polar, m_ChartPosition, m_ChartWidth, m_ChartHeight);
            var radius = m_Polar.runtimeRadius;
            axis.axisLabelTextList.Clear();

            string objName = "axis_angle";
            var axisObj = ChartHelper.AddObject(objName, transform, chartAnchorMin,
                chartAnchorMax, chartPivot, new Vector2(chartWidth, chartHeight));
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(axis.show && axis.axisLabel.show);
            axisObj.hideFlags = chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var labelColor = ChartHelper.IsClearColor(axis.axisLabel.color) ?
                (Color)m_ThemeInfo.axisTextColor :
                axis.axisLabel.color;
            int splitNumber = AxisHelper.GetSplitNumber(axis, radius, null);
            float totalAngle = m_AngleAxis.runtimeStartAngle;
            var total = 360;
            var cenPos = m_Polar.runtimeCenterPos;
            var txtHig = m_AngleAxis.axisLabel.fontSize + 2;
            var margin = m_AngleAxis.axisLabel.margin;
            var isCategory = m_AngleAxis.IsCategory();
            for (int i = 0; i < splitNumber - 1; i++)
            {
                float scaleAngle = AxisHelper.GetScaleWidth(axis, total, i, null);
                bool inside = axis.axisLabel.inside;
                Text txt = ChartHelper.AddTextObject(objName + i, axisObj.transform,
                    m_ThemeInfo.font, labelColor, TextAnchor.MiddleCenter, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(scaleAngle, txtHig),
                    axis.axisLabel.fontSize, axis.axisLabel.rotate, axis.axisLabel.fontStyle);
                if (i == 0) axis.axisLabel.SetRelatedText(txt, scaleAngle);
                var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
                txt.text = AxisHelper.GetLabelName(axis, total, i, axis.runtimeMinValue, axis.runtimeMaxValue, null,
                    isPercentStack);
                txt.gameObject.SetActive(axis.show &&
                    (axis.axisLabel.interval == 0 || i % (axis.axisLabel.interval + 1) == 0));
                var pos = ChartHelper.GetPos(cenPos, radius + margin, isCategory ? (totalAngle + scaleAngle / 2) : totalAngle, true);
                AxisHelper.AdjustCircleLabelPos(txt, pos, cenPos, txtHig, Vector3.zero);
                axis.axisLabelTextList.Add(txt);

                totalAngle += scaleAngle;
            }
            if (m_Tooltip.runtimeGameObject)
            {
                Vector2 privot = new Vector2(0.5f, 1);
                var labelParent = m_Tooltip.runtimeGameObject.transform;
                GameObject labelObj = ChartHelper.AddTooltipLabel(ChartCached.GetAxisTooltipLabel(objName), labelParent, m_ThemeInfo.font, privot);
                axis.SetTooltipLabel(labelObj);
                axis.SetTooltipLabelColor(m_ThemeInfo.tooltipBackgroundColor, m_ThemeInfo.tooltipTextColor);
                axis.SetTooltipLabelActive(axis.show && m_Tooltip.show && m_Tooltip.type == Tooltip.Type.Corss);
            }
        }

        protected override void Update()
        {
            base.Update();
            CheckMinMaxValue();
        }

        private void CheckMinMaxValue()
        {

            if (m_RadiusAxis.IsCategory() && m_AngleAxis.IsCategory())
            {
                m_CheckMinMaxValue = true;
                return;
            }
            UpdateAxisMinMaxValue(0, m_RadiusAxis);
            UpdateAxisMinMaxValue(0, m_AngleAxis);
        }

        private void UpdateAxisMinMaxValue(int axisIndex, Axis axis, bool updateChart = true)
        {
            if (axis.IsCategory() || !axis.show) return;
            float tempMinValue = 0;
            float tempMaxValue = 0;
            if (axis is RadiusAxis)
            {
                SeriesHelper.GetXMinMaxValue(m_Series, null, axisIndex, true, axis.inverse, out tempMinValue, out tempMaxValue);
            }
            else
            {
                SeriesHelper.GetYMinMaxValue(m_Series, null, axisIndex, true, axis.inverse, out tempMinValue, out tempMaxValue);
            }
            AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
            if (tempMinValue != axis.runtimeMinValue || tempMaxValue != axis.runtimeMaxValue)
            {
                m_CheckMinMaxValue = true;
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
            float m_CoordinateWidth = axis is RadiusAxis ? m_Polar.runtimeRadius : 360;
            var isPercentStack = SeriesHelper.IsPercentStack(m_Series, SerieType.Bar);
            axis.UpdateLabelText(m_CoordinateWidth, null, isPercentStack, 500);
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            DrawPolar(vh);
            DrawAngleAxis(vh);
            DrawRadiusAxis(vh);
            DrawSerie(vh);
        }

        private void UpdateRuntimeValue()
        {
            PolarHelper.UpdatePolarCenter(m_Polar, m_ChartPosition, m_ChartWidth, m_ChartHeight);
            m_AngleAxis.runtimeStartAngle = 90 - m_AngleAxis.startAngle;
        }

        private void DrawPolar(VertexHelper vh)
        {
            UpdateRuntimeValue();
            if (!ChartHelper.IsClearColor(m_Polar.backgroundColor))
            {
                ChartDrawer.DrawCricle(vh, m_Polar.runtimeCenterPos, m_Polar.runtimeRadius, m_Polar.backgroundColor);
            }
        }

        private void DrawRadiusAxis(VertexHelper vh)
        {
            var startAngle = m_AngleAxis.runtimeStartAngle;
            var radius = m_Polar.runtimeRadius;
            var cenPos = m_Polar.runtimeCenterPos;
            var size = AxisHelper.GetScaleNumber(m_RadiusAxis, radius, null);
            var totalWidth = 0f;
            var dire = ChartHelper.GetDire(startAngle, true).normalized;
            var tickVetor = ChartHelper.GetVertialDire(dire) * m_RadiusAxis.axisTick.length;
            var tickWidth = AxisHelper.GetTickWidth(m_RadiusAxis);
            for (int i = 0; i < size; i++)
            {
                var scaleWidth = AxisHelper.GetScaleWidth(m_RadiusAxis, radius, i);
                var pos = ChartHelper.GetPos(cenPos, totalWidth, startAngle, true);
                if (m_RadiusAxis.show && m_RadiusAxis.splitLine.show)
                {
                    var outsideRaidus = totalWidth + m_RadiusAxis.splitLine.lineStyle.width * 2;
                    var splitLineColor = m_RadiusAxis.splitLine.GetColor(m_ThemeInfo);
                    ChartDrawer.DrawDoughnut(vh, cenPos, totalWidth, outsideRaidus, splitLineColor, Color.clear);
                }
                if (m_RadiusAxis.show && m_RadiusAxis.axisTick.show)
                {
                    ChartDrawer.DrawLine(vh, pos, pos + tickVetor, tickWidth, m_ThemeInfo.axisLineColor);
                }
                totalWidth += scaleWidth;
            }
            if (m_RadiusAxis.show && m_RadiusAxis.axisLine.show)
            {
                var lineStartPos = m_Polar.runtimeCenterPos - dire * m_RadiusAxis.axisTick.width;
                var lineEndPos = m_Polar.runtimeCenterPos + dire * (radius + m_RadiusAxis.axisTick.width);
                ChartDrawer.DrawLine(vh, lineStartPos, lineEndPos, m_RadiusAxis.axisLine.width, m_ThemeInfo.axisLineColor);
            }
        }

        private void DrawAngleAxis(VertexHelper vh)
        {
            var radius = m_Polar.runtimeRadius;
            var cenPos = m_Polar.runtimeCenterPos;
            var total = 360;
            var size = AxisHelper.GetScaleNumber(m_AngleAxis, total, null);
            var currAngle = m_AngleAxis.runtimeStartAngle;
            var tickWidth = AxisHelper.GetTickWidth(m_AngleAxis);
            for (int i = 0; i < size; i++)
            {
                var scaleWidth = AxisHelper.GetScaleWidth(m_AngleAxis, total, i);
                var pos = ChartHelper.GetPos(cenPos, radius, currAngle, true);
                if (m_AngleAxis.show && m_AngleAxis.splitLine.show)
                {
                    var splitLineColor = m_AngleAxis.splitLine.GetColor(m_ThemeInfo);
                    ChartDrawer.DrawLine(vh, cenPos, pos, m_AngleAxis.splitLine.lineStyle.width, splitLineColor);
                }
                if (m_AngleAxis.show && m_AngleAxis.axisTick.show)
                {
                    var tickPos = ChartHelper.GetPos(cenPos, radius + m_AngleAxis.axisTick.length, currAngle, true);
                    ChartDrawer.DrawLine(vh, pos, tickPos, tickWidth, m_ThemeInfo.axisLineColor);
                }
                currAngle += scaleWidth;
            }
            if (m_AngleAxis.show && m_AngleAxis.axisLine.show)
            {
                var outsideRaidus = radius + m_AngleAxis.axisLine.width * 2;
                ChartDrawer.DrawDoughnut(vh, cenPos, radius, outsideRaidus, m_ThemeInfo.axisLineColor, Color.clear);
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
            var startAngle = m_AngleAxis.runtimeStartAngle;
            var radius = m_Polar.runtimeRadius;
            var datas = serie.data;
            if (datas.Count <= 0) return;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            float min = m_RadiusAxis.GetCurrMinValue(dataChangeDuration);
            float max = m_RadiusAxis.GetCurrMaxValue(dataChangeDuration);

            var firstSerieData = datas[0];
            var startPos = GetPolarPos(firstSerieData, min, max, radius);
            var nextPos = Vector3.zero;
            var lineColor = SerieHelper.GetLineColor(serie, m_ThemeInfo, serie.index, serie.highlighted);
            var lineWidth = serie.lineStyle.width;
            float currDetailProgress = 0;
            float totalDetailProgress = datas.Count;
            serie.animation.InitProgress(serie.dataPoints.Count, currDetailProgress, totalDetailProgress);
            serie.animation.SetDataFinish(0);
            for (int i = 1; i < datas.Count; i++)
            {
                if (serie.animation.CheckDetailBreak(i)) break;
                var serieData = datas[i];
                nextPos = GetPolarPos(datas[i], min, max, radius);
                ChartDrawer.DrawLine(vh, startPos, nextPos, lineWidth, lineColor);
                startPos = nextPos;
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress);
                serie.animation.CheckSymbol(serie.symbol.size);
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
                    float symbolSize = highlight ? symbol.selectedSize : symbol.size;
                    var symbolColor = SerieHelper.GetItemColor(serie, serieData, m_ThemeInfo, n, highlight);
                    var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, m_ThemeInfo, n, highlight);
                    var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, highlight);
                    var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                    symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                    DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, serieData.runtimePosition, symbolColor,
                        symbolToColor, symbol.gap, cornerRadius);
                }
            }
        }

        protected override void DrawTooltip(VertexHelper vh)
        {
            if (m_Tooltip.runtimeAngle < 0) return;
            var lineColor = TooltipHelper.GetLineColor(tooltip, m_ThemeInfo);
            var cenPos = m_Polar.runtimeCenterPos;
            var sp = m_Polar.runtimeCenterPos;
            var tooltipAngle = m_Tooltip.runtimeAngle + m_AngleAxis.runtimeStartAngle;
            var ep = ChartHelper.GetPos(sp, m_Polar.runtimeRadius, tooltipAngle, true);
            ChartDrawer.DrawLineStyle(vh, m_Tooltip.lineStyle, sp, ep, lineColor);
            if (m_Tooltip.type == Tooltip.Type.Corss)
            {
                var dist = Vector2.Distance(pointerPos, cenPos);
                if (dist > m_Polar.runtimeRadius) dist = m_Polar.runtimeRadius;
                var outsideRaidus = dist + m_Tooltip.lineStyle.width * 2;
                ChartDrawer.DrawDoughnut(vh, cenPos, dist, outsideRaidus, lineColor, Color.clear);
            }
        }

        private Vector3 GetPolarPos(SerieData serieData, float min, float max, float polarRadius)
        {
            var angle = 0f;
            if (!m_AngleAxis.clockwise)
            {
                angle = m_AngleAxis.runtimeStartAngle - serieData.GetData(1);
            }
            else
            {
                angle = m_AngleAxis.runtimeStartAngle + serieData.GetData(1);
            }
            angle = (angle + 360) % 360;
            var value = serieData.GetData(0);
            var radius = (value - min) / (max - min) * polarRadius;
            serieData.runtimeAngle = angle;
            serieData.runtimePosition = ChartHelper.GetPos(m_Polar.runtimeCenterPos, radius, angle, true);
            return serieData.runtimePosition;
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            var dist = Vector2.Distance(local, m_Polar.runtimeCenterPos);
            if (dist > m_Polar.runtimeRadius)
            {
                m_Tooltip.runtimeAngle = -1;
                if (m_Tooltip.IsActive())
                {
                    foreach (var kv in m_Tooltip.runtimeSerieIndex)
                    {
                        var serie = m_Series.GetSerie(kv.Key);
                        foreach (var dataIndex in kv.Value)
                        {
                            serie.GetSerieData(dataIndex).highlighted = false;
                        }
                    }
                    m_Tooltip.ClearSerieDataIndex();
                    m_Tooltip.SetActive(false);
                    RefreshChart();
                }
                return;
            }
            m_Tooltip.ClearSerieDataIndex();
            Vector2 dir = local - new Vector2(m_Polar.runtimeCenterPos.x, m_Polar.runtimeCenterPos.y);
            float angle = ChartHelper.GetAngle360(Vector2.up, dir);

            foreach (var serie in m_Series.list)
            {
                switch (serie.type)
                {
                    case SerieType.Line:
                        bool refresh = false;
                        var count = serie.data.Count;
                        SerieHelper.GetDimensionMinMaxData(serie, 1, -1);
                        var diff = (serie.runtimeDataMax - serie.runtimeDataMin) / (count - 1);
                        for (int j = 0; j < count; j++)
                        {
                            var serieData = serie.data[j];
                            var flag = Mathf.Abs(serieData.runtimeAngle - angle) < Mathf.Abs(diff / 2);
                            if (serieData.highlighted != flag)
                            {
                                refresh = true;
                            }
                            serieData.highlighted = flag;
                            if (flag)
                            {
                                m_Tooltip.runtimeAngle = (serieData.runtimeAngle - m_AngleAxis.runtimeStartAngle + 360) % 360;
                                m_Tooltip.AddSerieDataIndex(serie.index, j);
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
            m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
            UpdateTooltip();
            if (m_Tooltip.type == Tooltip.Type.Corss)
            {
                RefreshChart();
            }
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
            var showTooltip = m_Tooltip.isAnySerieDataIndex();
            if (showTooltip)
            {
                var content = TooltipHelper.GetPolarFormatterContent(m_Tooltip, m_Series, m_ThemeInfo, m_AngleAxis);
                TooltipHelper.SetContentAndPosition(tooltip, content, chartRect);
            }
            m_Tooltip.SetActive(showTooltip);
        }
    }
}
