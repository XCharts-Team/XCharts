using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/PieChart", 15)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class PieChart : BaseChart
    {
        private class PieTempData
        {
            public List<float> angleList = new List<float>();
            public Vector2 center;
            public float insideRadius;
            public float outsideRadius;
            public float dataMax;
            public float dataTotal;
        }

        [SerializeField] private Pie m_Pie = Pie.defaultPie;

        private bool isDrawPie;
        private bool m_IsEnterLegendButtom;
        private bool m_RefreshLabel;
        private List<PieTempData> m_PieTempDataList = new List<PieTempData>();

        public Pie pie { get { return m_Pie; } }

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Pie = Pie.defaultPie;
            m_Title.text = "PieChart";
            RemoveData();
            AddSerie("serie1", SerieType.Pie);
            AddData(0, 70, "pie1");
            AddData(0, 20, "pie2");
            AddData(0, 10, "pie3");
        }
#endif

        protected override void Update()
        {
            base.Update();
            if (!isDrawPie) RefreshChart();
        }

        Dictionary<string, int> serieNameSet = new Dictionary<string, int>();
        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            serieNameSet.Clear();
            int serieNameCount = -1;
            bool isClickOffset = false;
            bool isDataHighlight = false;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.series[i];
                serie.index = i;
                var data = serie.data;
                serie.animation.InitProgress(data.Count, 0, 360);
                if (!serie.show)
                {
                    continue;
                }
                if (!serie.animation.NeedAnimation(i)) break;
                bool isFinish = true;
                if (serie.pieClickOffset) isClickOffset = true;
                PieTempData tempData;
                if (i < m_PieTempDataList.Count)
                {
                    tempData = m_PieTempDataList[i];
                    tempData.angleList.Clear();
                }
                else
                {
                    tempData = new PieTempData();
                    m_PieTempDataList.Add(tempData);
                }
                tempData.angleList.Clear();
                tempData.dataMax = serie.yMax;
                tempData.dataTotal = serie.yTotal;
                UpdatePieCenter(serie);

                float totalDegree = 360;
                float startDegree = 0;
                int showdataCount = 0;

                foreach (var sd in serie.data)
                {
                    if (sd.show && serie.pieRoseType == RoseType.Area) showdataCount++;
                    sd.canShowLabel = false;
                }

                for (int n = 0; n < data.Count; n++)
                {
                    var serieData = data[n];
                    float value = serieData.data[1];
                    string dataName = serieData.name;
                    Color color;
                    if (string.IsNullOrEmpty(dataName))
                    {
                        serieNameCount++;
                        color = m_ThemeInfo.GetColor(serieNameCount);
                    }
                    else if (!serieNameSet.ContainsKey(dataName))
                    {
                        serieNameSet.Add(dataName, serieNameCount);
                        serieNameCount++;
                        color = m_ThemeInfo.GetColor(serieNameCount);
                    }
                    else
                    {
                        color = m_ThemeInfo.GetColor(serieNameSet[dataName]);
                    }
                    if (!serieData.show)
                    {
                        tempData.angleList.Add(0);
                        continue;
                    }
                    float degree = serie.pieRoseType == RoseType.Area ? (totalDegree / showdataCount) : (totalDegree * value / tempData.dataTotal);
                    float toDegree = startDegree + degree;

                    float outSideRadius = serie.pieRoseType > 0 ?
                        tempData.insideRadius + (tempData.outsideRadius - tempData.insideRadius) * value / tempData.dataMax :
                        tempData.outsideRadius;
                    if (serieData.highlighted)
                    {
                        isDataHighlight = true;
                        color *= 1.2f;
                        outSideRadius += m_Pie.tooltipExtraRadius;
                    }
                    var offset = serie.pieSpace;
                    if (serie.pieClickOffset && serieData.selected)
                    {
                        offset += m_Pie.selectedOffset;
                    }
                    var halfDegree = (toDegree - startDegree) / 2;
                    float currAngle = startDegree + halfDegree;
                    float currRad = currAngle * Mathf.Deg2Rad;
                    float currSin = Mathf.Sin(currRad);
                    float currCos = Mathf.Cos(currRad);
                    var center = tempData.center;

                    var currDegree = toDegree;
                    if (serie.animation.CheckDetailBreak(n, toDegree))
                    {
                        isFinish = false;
                        currDegree = serie.animation.GetCurrDetail();
                    }
                    if (offset > 0)
                    {
                        float offsetRadius = serie.pieSpace / Mathf.Sin(halfDegree * Mathf.Deg2Rad);
                        var insideRadius = tempData.insideRadius - offsetRadius;
                        var outsideRadius = outSideRadius - offsetRadius;
                        if (serie.pieClickOffset && serieData.selected)
                        {
                            offsetRadius += m_Pie.selectedOffset;
                            if (insideRadius > 0) insideRadius += m_Pie.selectedOffset;
                            outsideRadius += m_Pie.selectedOffset;
                        }

                        var offestCenter = new Vector3(center.x + offsetRadius * currSin,
                            center.y + offsetRadius * currCos);

                        ChartHelper.DrawDoughnut(vh, offestCenter, insideRadius, outsideRadius,
                            startDegree, currDegree, color);
                    }
                    else
                    {
                        ChartHelper.DrawDoughnut(vh, center, tempData.insideRadius, outSideRadius,
                            startDegree, currDegree, color);
                    }
                    serieData.canShowLabel = currDegree >= currAngle;
                    if (currDegree >= currAngle)
                    {
                        DrawLabelLine(vh, serie, tempData, outSideRadius, center, currAngle, color);
                    }
                    isDrawPie = true;
                    tempData.angleList.Add(toDegree);
                    startDegree = toDegree;
                    if (isFinish) serie.animation.SetDataFinish(n);
                    else
                    {
                        break;
                    }
                }
                if (!serie.animation.IsFinish())
                {
                    float duration = serie.animation.duration > 0 ? (float)serie.animation.duration / 1000 : 1;
                    float speed = 360 / duration;
                    float symbolSpeed = serie.symbol.size / duration;
                    serie.animation.CheckProgress(Time.deltaTime * speed);
                    serie.animation.CheckSymbol(Time.deltaTime * symbolSpeed, serie.symbol.size);
                    RefreshChart();
                }
            }
            DrawLabelBackground(vh);
            raycastTarget = isClickOffset && isDataHighlight;
        }

        private void DrawLabelBackground(VertexHelper vh)
        {
            foreach (var serie in m_Series.series)
            {
                if (serie.type == SerieType.Pie && serie.label.show)
                {
                    foreach (var serieData in serie.data)
                    {
                        if (serieData.canShowLabel)
                        {
                            DrawLabelBackground(vh, serie, serieData);
                        }
                    }
                }
            }
        }

        private void DrawLabelLine(VertexHelper vh, Serie serie, PieTempData tempData, float outSideRadius, Vector2 center, float currAngle, Color color)
        {
            if (serie.label.show
                && serie.label.position == SerieLabel.Position.Outside
                && serie.label.line)
            {
                if (serie.label.color != Color.clear) color = serie.label.color;
                float currSin = Mathf.Sin(currAngle * Mathf.Deg2Rad);
                float currCos = Mathf.Cos(currAngle * Mathf.Deg2Rad);
                var radius1 = outSideRadius;
                var radius2 = tempData.outsideRadius + serie.label.lineLength1;
                var pos1 = new Vector2(center.x + radius1 * currSin, center.y + radius1 * currCos);
                var pos2 = new Vector2(center.x + radius2 * currSin, center.y + radius2 * currCos);
                float tx, ty;
                Vector2 pos3;
                if (currAngle < 90)
                {
                    ty = serie.label.lineWidth * Mathf.Cos((90 - currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Sin((90 - currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector2(pos2.x - tx, pos2.y + ty - serie.label.lineWidth);
                }
                else if (currAngle < 180)
                {
                    ty = serie.label.lineWidth * Mathf.Sin((180 - currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Cos((180 - currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector2(pos2.x - tx, pos2.y - ty + serie.label.lineWidth);
                }
                else if (currAngle < 270)
                {
                    ty = serie.label.lineWidth * Mathf.Sin((180 + currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Cos((180 + currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector2(pos2.x + tx, pos2.y - ty + serie.label.lineWidth);
                }
                else
                {
                    ty = serie.label.lineWidth * Mathf.Cos((90 + currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Sin((90 + currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector2(pos2.x + tx, pos2.y + ty - serie.label.lineWidth);
                }
                var pos4 = new Vector2(currAngle > 180 ? pos3.x - serie.label.lineLength2 : pos3.x + serie.label.lineLength2, pos3.y);
                ChartHelper.DrawLine(vh, pos1, pos2, serie.label.lineWidth, color);
                ChartHelper.DrawLine(vh, pos3, pos4, serie.label.lineWidth, color);
            }
        }

        protected override void OnRefreshLabel()
        {
            serieNameSet.Clear();
            int serieNameCount = -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.series[i];
                serie.index = i;

                if (!serie.show)
                {
                    continue;
                }

                PieTempData tempData;
                if (i < m_PieTempDataList.Count)
                {
                    tempData = m_PieTempDataList[i];
                    tempData.angleList.Clear();
                }
                else
                {
                    tempData = new PieTempData();
                    m_PieTempDataList.Add(tempData);
                }
                tempData.angleList.Clear();
                tempData.dataMax = serie.yMax;
                tempData.dataTotal = serie.yTotal;
                UpdatePieCenter(serie);
                var data = serie.data;

                float totalDegree = 360;
                float startDegree = 0;
                int showdataCount = 0;
                if (serie.pieRoseType == RoseType.Area)
                {
                    foreach (var sd in serie.data)
                    {
                        if (sd.show) showdataCount++;
                    }
                }
                for (int n = 0; n < data.Count; n++)
                {
                    var serieData = data[n];
                    if (!serieData.canShowLabel)
                    {
                        serieData.SetLabelActive(false);
                        continue;
                    }
                    float value = serieData.data[1];
                    string dataName = serieData.name;
                    Color color;
                    if (string.IsNullOrEmpty(dataName))
                    {
                        serieNameCount++;
                        color = m_ThemeInfo.GetColor(serieNameCount);
                    }
                    else if (!serieNameSet.ContainsKey(dataName))
                    {
                        serieNameSet.Add(dataName, serieNameCount);
                        serieNameCount++;
                        color = m_ThemeInfo.GetColor(serieNameCount);
                    }
                    else
                    {
                        color = m_ThemeInfo.GetColor(serieNameSet[dataName]);
                    }
                    if (!serieData.show)
                    {
                        tempData.angleList.Add(0);
                        continue;
                    }
                    float degree = serie.pieRoseType == RoseType.Area ? (totalDegree / showdataCount) : (totalDegree * value / tempData.dataTotal);
                    float toDegree = startDegree + degree;

                    float outSideRadius = serie.pieRoseType > 0 ?
                        tempData.insideRadius + (tempData.outsideRadius - tempData.insideRadius) * value / tempData.dataMax :
                        tempData.outsideRadius;
                    if (serieData.highlighted)
                    {
                        outSideRadius += m_Pie.tooltipExtraRadius;
                    }
                    var offset = serie.pieSpace;
                    if (serie.pieClickOffset && serieData.selected)
                    {
                        offset += m_Pie.selectedOffset;
                    }
                    var halfDegree = (toDegree - startDegree) / 2;
                    float currAngle = startDegree + halfDegree;
                    if (offset > 0)
                    {
                        float offsetRadius = serie.pieSpace / Mathf.Sin(halfDegree * Mathf.Deg2Rad);
                        var insideRadius = tempData.insideRadius - offsetRadius;
                        var outsideRadius = outSideRadius - offsetRadius;
                        if (serie.pieClickOffset && serieData.selected)
                        {
                            offsetRadius += m_Pie.selectedOffset;
                            if (insideRadius > 0) insideRadius += m_Pie.selectedOffset;
                            outsideRadius += m_Pie.selectedOffset;
                        }
                        DrawLabel(serie, n, serieData, tempData, color, currAngle, offsetRadius, insideRadius, outsideRadius);
                    }
                    else
                    {
                        DrawLabel(serie, n, serieData, tempData, color, currAngle, 0, tempData.insideRadius, outSideRadius);
                    }
                    tempData.angleList.Add(toDegree);
                    startDegree = toDegree;
                }
            }
        }

        private void DrawLabel(Serie serie, int dataIndex, SerieData serieData, PieTempData tempData, Color serieColor,
            float currAngle, float offsetRadius, float insideRadius, float outsideRadius)
        {
            if (serieData.labelText == null) return;
            var isHighlight = (serieData.highlighted && serie.highlightLabel.show);
            if ((serie.label.show || isHighlight) && serieData.canShowLabel)
            {
                serieData.SetLabelActive(true);
                float rotate = 0;
                bool isInsidePosition = serie.label.position == SerieLabel.Position.Inside;
                if (serie.label.rotate > 0 && isInsidePosition)
                {
                    if (currAngle > 180) rotate += 270 - currAngle;
                    else rotate += -(currAngle - 90);
                }
                Color color = serieColor;
                if (isHighlight)
                {
                    if (serie.highlightLabel.color != Color.clear) color = serie.highlightLabel.color;
                }
                else if (serie.label.color != Color.clear)
                {
                    color = serie.label.color;
                }
                else
                {
                    color = isInsidePosition ? Color.white : serieColor;
                }
                var fontSize = isHighlight ? serie.highlightLabel.fontSize : serie.label.fontSize;
                var fontStyle = isHighlight ? serie.highlightLabel.fontStyle : serie.label.fontStyle;
                float currRad = currAngle * Mathf.Deg2Rad;

                serieData.labelText.color = color;
                serieData.labelText.fontSize = fontSize;
                serieData.labelText.fontStyle = fontStyle;

                serieData.labelRect.transform.localEulerAngles = new Vector3(0, 0, rotate);

                switch (serie.label.position)
                {
                    case SerieLabel.Position.Center:
                        serieData.labelPosition = tempData.center;
                        break;
                    case SerieLabel.Position.Inside:
                        var labelRadius = offsetRadius + insideRadius + (outsideRadius - insideRadius) / 2;
                        var labelCenter = new Vector2(tempData.center.x + labelRadius * Mathf.Sin(currRad),
                            tempData.center.y + labelRadius * Mathf.Cos(currRad));
                        serieData.labelPosition = labelCenter;
                        break;
                    case SerieLabel.Position.Outside:
                        labelRadius = tempData.outsideRadius + serie.label.lineLength1;
                        labelCenter = new Vector2(tempData.center.x + labelRadius * Mathf.Sin(currRad),
                            tempData.center.y + labelRadius * Mathf.Cos(currRad));
                        float labelWidth = serieData.labelText.preferredWidth;
                        if (currAngle > 180)
                        {
                            serieData.labelPosition = new Vector2(labelCenter.x - serie.label.lineLength2 - 5 - labelWidth / 2, labelCenter.y);
                        }
                        else
                        {
                            serieData.labelPosition = new Vector2(labelCenter.x + serie.label.lineLength2 + 5 + labelWidth / 2, labelCenter.y);
                        }
                        break;
                }
                serieData.SetLabelPosition(serieData.labelPosition);
            }
            else
            {
                serieData.SetLabelActive(false);
            }
        }

        protected override void OnLegendButtonClick(int index, string legendName, bool show)
        {
            bool active = CheckDataShow(legendName, show);
            var bgColor1 = active ? m_ThemeInfo.GetColor(index) : m_ThemeInfo.legendUnableColor;
            m_Legend.UpdateButtonColor(legendName, bgColor1);
            RefreshChart();
        }

        protected override void OnLegendButtonEnter(int index, string legendName)
        {
            m_IsEnterLegendButtom = true;
            CheckDataHighlighted(legendName, true);
            RefreshChart();
        }

        protected override void OnLegendButtonExit(int index, string legendName)
        {
            m_IsEnterLegendButtom = false;
            CheckDataHighlighted(legendName, false);
            RefreshChart();
        }

        private void UpdatePieCenter(Serie serie)
        {
            if (serie.pieCenter.Length < 2) return;
            var tempData = m_PieTempDataList[serie.index];
            var centerX = serie.pieCenter[0] <= 1 ? chartWidth * serie.pieCenter[0] : serie.pieCenter[0];
            var centerY = serie.pieCenter[1] <= 1 ? chartHeight * serie.pieCenter[1] : serie.pieCenter[1];
            tempData.center = new Vector2(centerX, centerY);
            var minWidth = Mathf.Min(chartWidth, chartHeight);
            tempData.insideRadius = serie.pieRadius[0] <= 1 ? minWidth * serie.pieRadius[0] : serie.pieRadius[0];
            tempData.outsideRadius = serie.pieRadius[1] <= 1 ? minWidth * serie.pieRadius[1] : serie.pieRadius[1];
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            if (m_IsEnterLegendButtom) return;
            m_Tooltip.dataIndex.Clear();
            bool selected = false;
            for (int i = 0; i < m_PieTempDataList.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                var tempData = m_PieTempDataList[i];
                int index = GetPosPieIndex(tempData, local);
                m_Tooltip.dataIndex.Add(index);

                bool refresh = false;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (serieData.highlighted != (j == index)) refresh = true;
                    serieData.highlighted = j == index;
                }

                if (index >= 0) selected = true;
                if (refresh) RefreshChart();
            }
            if (selected)
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
            }
            else
            {
                m_Tooltip.SetActive(false);
            }
        }

        private int GetPosPieIndex(PieTempData tempData, Vector2 local)
        {
            var dist = Vector2.Distance(local, tempData.center);
            if (dist < tempData.insideRadius || dist > tempData.outsideRadius) return -1;
            Vector2 dir = local - tempData.center;
            float angle = VectorAngle(Vector2.up, dir);
            var angleList = tempData.angleList;
            for (int i = angleList.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    if (angle <= angleList[i]) return 0;
                }
                else if (angle <= angleList[i] && angle > angleList[i - 1])
                {
                    return i;
                }
            }
            return -1;
        }

        float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;

            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            angle = cross.z > 0 ? -angle : angle;
            angle = (angle + 360) % 360;
            return angle;
        }

        StringBuilder sb = new StringBuilder();
        protected override void RefreshTooltip()
        {
            base.RefreshTooltip();
            bool showTooltip = false;
            for (int i = 0; i < m_PieTempDataList.Count; i++)
            {
                int index = m_Tooltip.dataIndex[i];
                if (index < 0) continue;
                showTooltip = true;
                var serie = m_Series.GetSerie(i);
                string key = serie.data[index].name;
                if (string.IsNullOrEmpty(key)) key = m_Legend.GetData(index);
                float value = serie.data[index].data[1];
                sb.Length = 0;
                if (!string.IsNullOrEmpty(serie.name))
                {
                    sb.Append(serie.name).Append("\n");
                }
                sb.Append("<color=#").Append(m_ThemeInfo.GetColorStr(index)).Append(">● </color>")
                    .Append(key).Append(": ").Append(ChartCached.FloatToStr(value));
                m_Tooltip.UpdateContentText(sb.ToString());

                var pos = m_Tooltip.GetContentPos();
                if (pos.x + m_Tooltip.width > chartWidth)
                {
                    pos.x = chartWidth - m_Tooltip.width;
                }
                if (pos.y - m_Tooltip.height < 0)
                {
                    pos.y = m_Tooltip.height;
                }
                m_Tooltip.UpdateContentPos(pos);
            }
            m_Tooltip.SetActive(showTooltip);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Vector2 local;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                eventData.position, canvas.worldCamera, out local))
            {
                return;
            }
            for (int i = 0; i < m_PieTempDataList.Count; i++)
            {
                var tempData = m_PieTempDataList[i];
                int index = GetPosPieIndex(tempData, local);
                if (index >= 0)
                {
                    var serie = m_Series.GetSerie(i);
                    for (int j = 0; j < serie.data.Count; j++)
                    {
                        if (j == index) serie.data[j].selected = !serie.data[j].selected;
                        else serie.data[j].selected = false;
                    }
                }
            }
            RefreshChart();
        }
    }
}
