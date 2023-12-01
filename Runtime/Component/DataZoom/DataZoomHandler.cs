using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class DataZoomHandler : MainComponentHandler<DataZoom>
    {
        private static readonly string s_DefaultDataZoom = "datazoom";
        private Vector2 m_LastTouchPos0;
        private Vector2 m_LastTouchPos1;
        private bool m_CheckDataZoomLabel;
        private float m_DataZoomLastStartIndex;
        private float m_DataZoomLastEndIndex;

        public override void InitComponent()
        {
            var dataZoom = component;
            dataZoom.painter = chart.m_PainterUpper;
            dataZoom.refreshComponent = delegate ()
            {
                var dataZoomObject = ChartHelper.AddObject(s_DefaultDataZoom + dataZoom.index, chart.transform,
                    chart.chartMinAnchor, chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                dataZoom.gameObject = dataZoomObject;
                dataZoomObject.hideFlags = chart.chartHideFlags;
                ChartHelper.HideAllObject(dataZoomObject);

                var startLabel = ChartHelper.AddChartLabel(s_DefaultDataZoom + "start", dataZoomObject.transform,
                    dataZoom.labelStyle, chart.theme.dataZoom, "", Color.clear, TextAnchor.MiddleRight);
                startLabel.gameObject.SetActive(true);

                var endLabel = ChartHelper.AddChartLabel(s_DefaultDataZoom + "end", dataZoomObject.transform,
                    dataZoom.labelStyle, chart.theme.dataZoom, "", Color.clear, TextAnchor.MiddleLeft);
                endLabel.gameObject.SetActive(true);

                dataZoom.SetStartLabel(startLabel);
                dataZoom.SetEndLabel(endLabel);
                dataZoom.SetLabelActive(false);

                foreach (var index in dataZoom.xAxisIndexs)
                {
                    var xAxis = chart.GetChartComponent<XAxis>(index);
                    if (xAxis != null)
                    {
                        xAxis.UpdateFilterData(dataZoom);
                    }
                }

                foreach (var serie in chart.series)
                {
                    SerieHelper.UpdateFilterData(serie, dataZoom);
                }
            };
            dataZoom.refreshComponent();
        }
        public override void Update()
        {
            CheckDataZoomScale(component);
            CheckDataZoomLabel(component);
        }

        public override void DrawUpper(VertexHelper vh)
        {
            if (chart == null)
                return;

            var dataZoom = component;
            switch (dataZoom.orient)
            {
                case Orient.Horizonal:
                    DrawHorizonalDataZoomSlider(vh, dataZoom);
                    DrawMarquee(vh, dataZoom);
                    break;
                case Orient.Vertical:
                    DrawVerticalDataZoomSlider(vh, dataZoom);
                    DrawMarquee(vh, dataZoom);
                    break;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (chart == null)
                return;

            if (Input.touchCount > 1)
                return;

            var dataZoom = component;
            if (!dataZoom.enable)
                return;

            Vector2 pos;
            if (!chart.ScreenPointToChartPoint(eventData.position, out pos))
                return;

            var grid = chart.GetGridOfDataZoom(dataZoom);
            if (dataZoom.supportInside && dataZoom.supportInsideDrag)
            {
                if (grid.Contains(pos))
                {
                    dataZoom.context.isCoordinateDrag = true;
                }
            }
            if (dataZoom.supportMarquee)
            {
                dataZoom.context.isMarqueeDrag = true;
                dataZoom.context.marqueeStartPos = pos;
                dataZoom.context.marqueeEndPos = pos;

                if (dataZoom.marqueeStyle.realRect)
                    dataZoom.context.marqueeRect = new Rect(pos.x, pos.y, 0, 0);
                else
                    dataZoom.context.marqueeRect = new Rect(pos.x, grid.context.y, 0, grid.context.height);

                if (dataZoom.marqueeStyle.onStart != null)
                {
                    dataZoom.marqueeStyle.onStart(dataZoom);
                }
                return;
            }
            if (dataZoom.supportSlider)
            {
                if (!dataZoom.zoomLock)
                {
                    if (dataZoom.IsInStartZoom(pos))
                    {
                        dataZoom.context.isStartDrag = true;
                    }
                    else if (dataZoom.IsInEndZoom(pos))
                    {
                        dataZoom.context.isEndDrag = true;
                    }
                    else if (dataZoom.IsInSelectedZoom(pos))
                    {
                        dataZoom.context.isDrag = true;
                    }
                }
                else if (dataZoom.IsInSelectedZoom(pos))
                {
                    dataZoom.context.isDrag = true;
                }
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (chart == null)
                return;
            if (Input.touchCount > 1)
                return;

            var dataZoom = component;
            var grid = chart.GetGridOfDataZoom(dataZoom);
            if (dataZoom.supportMarquee)
            {
                Vector2 pos;
                if (!chart.ScreenPointToChartPoint(eventData.position, out pos))
                    return;

                dataZoom.context.marqueeEndPos = pos;
                var oldRect = dataZoom.context.marqueeRect;
                var rectWidth = pos.x - dataZoom.context.marqueeStartPos.x;
                if (dataZoom.marqueeStyle.realRect)
                    dataZoom.context.marqueeRect = Rect.MinMaxRect(dataZoom.context.marqueeStartPos.x, pos.y, pos.x, dataZoom.context.marqueeStartPos.y);
                else
                    dataZoom.context.marqueeRect = new Rect(oldRect.x, oldRect.y, rectWidth, oldRect.height);

                dataZoom.SetVerticesDirty();
                if (dataZoom.marqueeStyle.onGoing != null)
                    dataZoom.marqueeStyle.onGoing(dataZoom);
                return;
            }
            else
            {
                switch (dataZoom.orient)
                {
                    case Orient.Horizonal:
                        var deltaPercent = eventData.delta.x / grid.context.width * 100;
                        OnDragInside(dataZoom, deltaPercent);
                        OnDragSlider(dataZoom, deltaPercent);
                        break;
                    case Orient.Vertical:
                        deltaPercent = eventData.delta.y / grid.context.height * 100;
                        OnDragInside(dataZoom, deltaPercent);
                        OnDragSlider(dataZoom, deltaPercent);
                        break;
                }
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (chart == null)
                return;

            var dataZoom = component;

            if (dataZoom.supportMarquee)
            {
                dataZoom.context.isMarqueeDrag = false;
                if (dataZoom.marqueeStyle.apply)
                {
                    var grid = chart.GetGridOfDataZoom(dataZoom);
                    var start = (dataZoom.context.marqueeRect.x - grid.context.x) / grid.context.width * 100;
                    var end = (dataZoom.context.marqueeRect.x - grid.context.x + dataZoom.context.marqueeRect.width) / grid.context.width * 100;
                    UpdateDataZoomRange(dataZoom, start, end);
                }
                if (dataZoom.marqueeStyle.onEnd != null)
                {
                    dataZoom.marqueeStyle.onEnd(dataZoom);
                }
                return;
            }
            if (dataZoom.context.isDrag || dataZoom.context.isStartDrag || dataZoom.context.isEndDrag ||
                dataZoom.context.isCoordinateDrag)
            {
                chart.RefreshChart();
            }
            dataZoom.context.isDrag = false;
            dataZoom.context.isCoordinateDrag = false;
            dataZoom.context.isStartDrag = false;
            dataZoom.context.isEndDrag = false;
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (chart == null)
                return;
            if (Input.touchCount > 1)
                return;

            Vector2 localPos;
            if (!chart.ScreenPointToChartPoint(eventData.position, out localPos))
                return;

            var dataZoom = component;
            var grid = chart.GetGridOfDataZoom(dataZoom);
            if (dataZoom.IsInStartZoom(localPos) ||
                dataZoom.IsInEndZoom(localPos))
            {
                return;
            }

            if (dataZoom.IsInZoom(localPos) &&
                !dataZoom.IsInSelectedZoom(localPos))
            {
                var pointerX = localPos.x;
                var selectWidth = grid.context.width * (dataZoom.end - dataZoom.start) / 100;
                var startX = pointerX - selectWidth / 2;
                var endX = pointerX + selectWidth / 2;
                if (startX < grid.context.x)
                {
                    startX = grid.context.x;
                    endX = grid.context.x + selectWidth;
                }
                else if (endX > grid.context.x + grid.context.width)
                {
                    endX = grid.context.x + grid.context.width;
                    startX = grid.context.x + grid.context.width - selectWidth;
                }
                var start = (startX - grid.context.x) / grid.context.width * 100;
                var end = (endX - grid.context.x) / grid.context.width * 100;
                UpdateDataZoomRange(dataZoom, start, end);
            }
        }

        public override void OnScroll(PointerEventData eventData)
        {
            if (chart == null)
                return;
            if (Input.touchCount > 1)
                return;

            var dataZoom = component;
            if (!dataZoom.enable || dataZoom.zoomLock)
                return;

            Vector2 pos;
            if (!chart.ScreenPointToChartPoint(eventData.position, out pos))
                return;

            var grid = chart.GetGridOfDataZoom(dataZoom);
            if ((dataZoom.supportInside && dataZoom.supportInsideScroll && grid.Contains(pos)) ||
                dataZoom.IsInZoom(pos))
            {
                ScaleDataZoom(dataZoom, eventData.scrollDelta.y * dataZoom.scrollSensitivity);
            }
        }

        private void OnDragInside(DataZoom dataZoom, float deltaPercent)
        {
            if (deltaPercent == 0)
                return;
            if (Input.touchCount > 1)
                return;
            if (!dataZoom.supportInside || !dataZoom.supportInsideDrag)
                return;
            if (!dataZoom.context.isCoordinateDrag)
                return;

            var diff = dataZoom.end - dataZoom.start;
            if (deltaPercent > 0)
            {
                if (dataZoom.start > 0)
                {
                    var start = dataZoom.start - deltaPercent;
                    if (start < 0) start = 0;
                    var end = start + diff;
                    UpdateDataZoomRange(dataZoom, start, end);
                }
            }
            else
            {
                if (dataZoom.end < 100)
                {
                    var end = dataZoom.end - deltaPercent;
                    if (end > 100) end = 100;
                    var start = end - diff;
                    UpdateDataZoomRange(dataZoom, start, end);
                }
            }
        }

        private void OnDragSlider(DataZoom dataZoom, float deltaPercent)
        {
            if (Input.touchCount > 1)
                return;
            if (!dataZoom.supportSlider)
                return;

            if (dataZoom.context.isStartDrag)
            {
                var start = dataZoom.start + deltaPercent;
                if (start > dataZoom.end)
                {
                    start = dataZoom.end;
                    dataZoom.context.isEndDrag = true;
                    dataZoom.context.isStartDrag = false;
                }
                UpdateDataZoomRange(dataZoom, start, dataZoom.end);
            }
            else if (dataZoom.context.isEndDrag)
            {
                var end = dataZoom.end + deltaPercent;
                if (end < dataZoom.start)
                {
                    end = dataZoom.start;
                    dataZoom.context.isStartDrag = true;
                    dataZoom.context.isEndDrag = false;
                }
                UpdateDataZoomRange(dataZoom, dataZoom.start, end);
            }
            else if (dataZoom.context.isDrag)
            {
                if (deltaPercent > 0)
                {
                    if (dataZoom.end + deltaPercent > 100) deltaPercent = 100 - dataZoom.end;
                }
                else
                {
                    if (dataZoom.start + deltaPercent < 0) deltaPercent = -dataZoom.start;
                }
                UpdateDataZoomRange(dataZoom, dataZoom.start + deltaPercent, dataZoom.end + deltaPercent);
            }
        }

        private void ScaleDataZoom(DataZoom dataZoom, float delta)
        {
            var grid = chart.GetGridOfDataZoom(dataZoom);
            var deltaPercent = dataZoom.orient == Orient.Horizonal ?
                Mathf.Abs(delta / grid.context.width * 100) :
                Mathf.Abs(delta / grid.context.height * 100);
            if (delta > 0)
            {
                if (dataZoom.end <= dataZoom.start)
                    return;
                UpdateDataZoomRange(dataZoom, dataZoom.start + deltaPercent, dataZoom.end - deltaPercent);
            }
            else
            {
                UpdateDataZoomRange(dataZoom, dataZoom.start - deltaPercent, dataZoom.end + deltaPercent);
            }
        }

        public void UpdateDataZoomRange(DataZoom dataZoom, float start, float end)
        {
            if (end > 100)
                end = 100;

            if (start < 0)
                start = 0;

            if (end < start)
                end = start;
            if (dataZoom.startEndFunction != null)
                dataZoom.startEndFunction(ref start, ref end);

            if (!dataZoom.startLock)
                dataZoom.start = start;
            if (!dataZoom.endLock)
                dataZoom.end = end;
            if (dataZoom.realtime)
            {
                chart.OnDataZoomRangeChanged(dataZoom);
                chart.RefreshChart();
            }
        }

        public void RefreshDataZoomLabel()
        {
            m_CheckDataZoomLabel = true;
        }

        private void CheckDataZoomScale(DataZoom dataZoom)
        {
            if (!dataZoom.enable || dataZoom.zoomLock || !dataZoom.supportInside || !dataZoom.supportInsideDrag)
                return;

            if (Input.touchCount == 2)
            {
                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Began)
                {
                    m_LastTouchPos0 = touch0.position;
                    m_LastTouchPos1 = touch1.position;
                }
                else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    var tempPos0 = touch0.position;
                    var tempPos1 = touch1.position;
                    var currDist = Vector2.Distance(tempPos0, tempPos1);
                    var lastDist = Vector2.Distance(m_LastTouchPos0, m_LastTouchPos1);
                    var delta = (currDist - lastDist);
                    ScaleDataZoom(dataZoom, delta / dataZoom.scrollSensitivity);
                    m_LastTouchPos0 = tempPos0;
                    m_LastTouchPos1 = tempPos1;
                }
            }
        }

        private void CheckDataZoomLabel(DataZoom dataZoom)
        {
            if (dataZoom.enable && dataZoom.supportSlider && dataZoom.showDetail)
            {
                Vector2 local;
                if (!chart.ScreenPointToChartPoint(Input.mousePosition, out local))
                {
                    dataZoom.SetLabelActive(false);
                    return;
                }
                if (dataZoom.IsInSelectedZoom(local) ||
                    dataZoom.IsInStartZoom(local) ||
                    dataZoom.IsInEndZoom(local))
                {
                    dataZoom.SetLabelActive(true);
                    RefreshDataZoomLabel();
                }
                else
                {
                    dataZoom.SetLabelActive(false);
                }
            }
            if (m_CheckDataZoomLabel && dataZoom.xAxisIndexs.Count > 0)
            {
                m_CheckDataZoomLabel = false;
                var xAxis = chart.GetChartComponent<XAxis>(dataZoom.xAxisIndexs[0]);
                var startIndex = (int)((xAxis.data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((xAxis.data.Count - 1) * dataZoom.end / 100);

                if (m_DataZoomLastStartIndex != startIndex || m_DataZoomLastEndIndex != endIndex)
                {
                    m_DataZoomLastStartIndex = startIndex;
                    m_DataZoomLastEndIndex = endIndex;
                    if (xAxis.data.Count > 0)
                    {
                        dataZoom.SetStartLabelText(xAxis.data[startIndex]);
                        dataZoom.SetEndLabelText(xAxis.data[endIndex]);
                    }
                    else if (xAxis.IsTime())
                    {
                        //TODO:
                        dataZoom.SetStartLabelText("");
                        dataZoom.SetEndLabelText("");
                    }
                    xAxis.SetAllDirty();
                }
                var start = dataZoom.context.x + dataZoom.context.width * dataZoom.start / 100;
                var end = dataZoom.context.x + dataZoom.context.width * dataZoom.end / 100;
                var hig = dataZoom.context.height;
                dataZoom.UpdateStartLabelPosition(new Vector3(start - 10, chart.chartY + dataZoom.bottom + hig / 2));
                dataZoom.UpdateEndLabelPosition(new Vector3(end + 10, chart.chartY + dataZoom.bottom + hig / 2));
            }
        }

        private void DrawHorizonalDataZoomSlider(VertexHelper vh, DataZoom dataZoom)
        {
            if (!dataZoom.enable || !dataZoom.supportSlider)
                return;
            var p1 = new Vector3(dataZoom.context.x, dataZoom.context.y);
            var p2 = new Vector3(dataZoom.context.x, dataZoom.context.y + dataZoom.context.height);
            var p3 = new Vector3(dataZoom.context.x + dataZoom.context.width, dataZoom.context.y + dataZoom.context.height);
            var p4 = new Vector3(dataZoom.context.x + dataZoom.context.width, dataZoom.context.y);

            var lineColor = dataZoom.lineStyle.GetColor(chart.theme.dataZoom.dataLineColor);
            var lineWidth = dataZoom.lineStyle.GetWidth(chart.theme.dataZoom.dataLineWidth);
            var borderWidth = dataZoom.borderWidth == 0 ? chart.theme.dataZoom.borderWidth : dataZoom.borderWidth;
            var borderColor = dataZoom.GetBorderColor(chart.theme.dataZoom.borderColor);
            var backgroundColor = dataZoom.GetBackgroundColor(chart.theme.dataZoom.backgroundColor);
            var areaColor = dataZoom.areaStyle.GetColor(chart.theme.dataZoom.dataAreaColor);

            UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, backgroundColor);

            var centerPos = new Vector3(dataZoom.context.x + dataZoom.context.width / 2,
                dataZoom.context.y + dataZoom.context.height / 2);
            UGL.DrawBorder(vh, centerPos, dataZoom.context.width, dataZoom.context.height, borderWidth, borderColor);
            if (dataZoom.showDataShadow && chart.series.Count > 0)
            {
                Serie serie = chart.series[0];
                Axis axis = chart.GetChartComponent<YAxis>(0);
                var showData = serie.GetDataList(null);
                float scaleWid = dataZoom.context.width / (showData.Count - 1);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                double minValue = 0;
                double maxValue = 0;
                SeriesHelper.GetYMinMaxValue(chart, 0, axis.inverse, out minValue, out maxValue, false, false);
                AxisHelper.AdjustMinMaxValue(axis, ref minValue, ref maxValue, true);

                int rate = 1;
                var sampleDist = serie.sampleDist < 2 ? 2 : serie.sampleDist;
                var maxCount = showData.Count;
                if (sampleDist > 0)
                    rate = (int)((maxCount - serie.minShow) / (dataZoom.context.width / sampleDist));
                if (rate < 1)
                    rate = 1;

                var totalAverage = serie.sampleAverage > 0 ? serie.sampleAverage :
                    DataHelper.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
                var dataChanging = false;
                var animationDuration = serie.animation.GetChangeDuration();
                var dataAddDuration = serie.animation.GetAdditionDuration();
                var unscaledTime = serie.animation.unscaledTime;

                for (int i = 0; i < maxCount; i += rate)
                {
                    double value = DataHelper.SampleValue(ref showData, serie.sampleType, rate, serie.minShow, maxCount, totalAverage, i,
                        dataAddDuration, animationDuration, ref dataChanging, axis, unscaledTime);
                    float pX = dataZoom.context.x + i * scaleWid;
                    float dataHig = (float)((maxValue - minValue) == 0 ? 0 :
                        (value - minValue) / (maxValue - minValue) * dataZoom.context.height);
                    np = new Vector3(pX, chart.chartY + dataZoom.bottom + dataHig);
                    if (i > 0)
                    {
                        UGL.DrawLine(vh, lp, np, lineWidth, lineColor);
                        Vector3 alp = new Vector3(lp.x, lp.y - lineWidth);
                        Vector3 anp = new Vector3(np.x, np.y - lineWidth);

                        Vector3 tnp = new Vector3(np.x, chart.chartY + dataZoom.bottom + lineWidth);
                        Vector3 tlp = new Vector3(lp.x, chart.chartY + dataZoom.bottom + lineWidth);
                        UGL.DrawQuadrilateral(vh, alp, anp, tnp, tlp, areaColor);
                    }
                    lp = np;
                }
                if (dataChanging)
                {
                    chart.RefreshTopPainter();
                }
            }
            switch (dataZoom.rangeMode)
            {
                case DataZoom.RangeMode.Percent:
                    var start = dataZoom.context.x + dataZoom.context.width * dataZoom.start / 100;
                    var end = dataZoom.context.x + dataZoom.context.width * dataZoom.end / 100;
                    var fillerColor = dataZoom.GetFillerColor(chart.theme.dataZoom.fillerColor);

                    p1 = new Vector2(start, dataZoom.context.y);
                    p2 = new Vector2(start, dataZoom.context.y + dataZoom.context.height);
                    p3 = new Vector2(end, dataZoom.context.y + dataZoom.context.height);
                    p4 = new Vector2(end, dataZoom.context.y);
                    UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, fillerColor);
                    UGL.DrawLine(vh, p1, p2, lineWidth, fillerColor);
                    UGL.DrawLine(vh, p3, p4, lineWidth, fillerColor);
                    break;
            }
        }

        private void DrawVerticalDataZoomSlider(VertexHelper vh, DataZoom dataZoom)
        {
            if (!dataZoom.enable || !dataZoom.supportSlider)
                return;

            var p1 = new Vector3(dataZoom.context.x, dataZoom.context.y);
            var p2 = new Vector3(dataZoom.context.x, dataZoom.context.y + dataZoom.context.height);
            var p3 = new Vector3(dataZoom.context.x + dataZoom.context.width, dataZoom.context.y + dataZoom.context.height);
            var p4 = new Vector3(dataZoom.context.x + dataZoom.context.width, dataZoom.context.y);
            var lineColor = dataZoom.lineStyle.GetColor(chart.theme.dataZoom.dataLineColor);
            var lineWidth = dataZoom.lineStyle.GetWidth(chart.theme.dataZoom.dataLineWidth);
            var borderWidth = dataZoom.borderWidth == 0 ? chart.theme.dataZoom.borderWidth : dataZoom.borderWidth;
            var borderColor = dataZoom.GetBorderColor(chart.theme.dataZoom.borderColor);
            var backgroundColor = dataZoom.GetBackgroundColor(chart.theme.dataZoom.backgroundColor);
            var areaColor = dataZoom.areaStyle.GetColor(chart.theme.dataZoom.dataAreaColor);

            UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, backgroundColor);
            var centerPos = new Vector3(dataZoom.context.x + dataZoom.context.width / 2,
                dataZoom.context.y + dataZoom.context.height / 2);
            UGL.DrawBorder(vh, centerPos, dataZoom.context.width, dataZoom.context.height, borderWidth, borderColor);

            if (dataZoom.showDataShadow && chart.series.Count > 0)
            {
                Serie serie = chart.series[0];
                Axis axis = chart.GetChartComponent<YAxis>(0);
                var showData = serie.GetDataList(null);
                float scaleWid = dataZoom.context.height / (showData.Count - 1);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                double minValue = 0;
                double maxValue = 0;
                SeriesHelper.GetYMinMaxValue(chart, 0, axis.inverse, out minValue, out maxValue);
                AxisHelper.AdjustMinMaxValue(axis, ref minValue, ref maxValue, true);

                int rate = 1;
                var sampleDist = serie.sampleDist < 2 ? 2 : serie.sampleDist;
                var maxCount = showData.Count;
                if (sampleDist > 0)
                    rate = (int)((maxCount - serie.minShow) / (dataZoom.context.height / sampleDist));
                if (rate < 1)
                    rate = 1;

                var totalAverage = serie.sampleAverage > 0 ? serie.sampleAverage :
                    DataHelper.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
                var dataChanging = false;
                var animationDuration = serie.animation.GetChangeDuration();
                var dataAddDuration = serie.animation.GetAdditionDuration();
                var unscaledTime = serie.animation.unscaledTime;

                for (int i = 0; i < maxCount; i += rate)
                {
                    double value = DataHelper.SampleValue(ref showData, serie.sampleType, rate, serie.minShow, maxCount, totalAverage, i,
                        dataAddDuration, animationDuration, ref dataChanging, axis, unscaledTime);
                    float pY = dataZoom.context.y + i * scaleWid;
                    float dataHig = (maxValue - minValue) == 0 ? 0 :
                        (float)((value - minValue) / (maxValue - minValue) * dataZoom.context.width);
                    np = new Vector3(chart.chartX + chart.chartWidth - dataZoom.right - dataHig, pY);
                    if (i > 0)
                    {
                        UGL.DrawLine(vh, lp, np, lineWidth, lineColor);
                        Vector3 alp = new Vector3(lp.x, lp.y - lineWidth);
                        Vector3 anp = new Vector3(np.x, np.y - lineWidth);

                        Vector3 tnp = new Vector3(np.x, chart.chartY + dataZoom.bottom + lineWidth);
                        Vector3 tlp = new Vector3(lp.x, chart.chartY + dataZoom.bottom + lineWidth);
                        UGL.DrawQuadrilateral(vh, alp, anp, tnp, tlp, areaColor);
                    }
                    lp = np;
                }
                if (dataChanging)
                {
                    chart.RefreshTopPainter();
                }
            }
            switch (dataZoom.rangeMode)
            {
                case DataZoom.RangeMode.Percent:
                    var start = dataZoom.context.y + dataZoom.context.height * dataZoom.start / 100;
                    var end = dataZoom.context.y + dataZoom.context.height * dataZoom.end / 100;
                    var fillerColor = dataZoom.GetFillerColor(chart.theme.dataZoom.fillerColor);

                    p1 = new Vector2(dataZoom.context.x, start);
                    p2 = new Vector2(dataZoom.context.x + dataZoom.context.width, start);
                    p3 = new Vector2(dataZoom.context.x + dataZoom.context.width, end);
                    p4 = new Vector2(dataZoom.context.x, end);
                    UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, fillerColor);
                    UGL.DrawLine(vh, p1, p2, lineWidth, fillerColor);
                    UGL.DrawLine(vh, p3, p4, lineWidth, fillerColor);
                    break;
            }
        }

        private void DrawMarquee(VertexHelper vh, DataZoom dataZoom)
        {
            if (!dataZoom.enable || !dataZoom.supportMarquee)
                return;
            var areaColor = dataZoom.marqueeStyle.areaStyle.GetColor(chart.theme.dataZoom.dataAreaColor);
            UGL.DrawRectangle(vh, dataZoom.context.marqueeRect, areaColor);
        }
    }
}