using System.Collections;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class Example10_LineChart : MonoBehaviour
    {
        private LineChart chart;
        private Serie serie;
        private int m_DataNum = 8;

        private void OnEnable()
        {
            StartCoroutine(PieDemo());
        }

        IEnumerator PieDemo()
        {
            while (true)
            {
                StartCoroutine(AddSimpleLine());
                yield return new WaitForSeconds(2);
                StartCoroutine(ChangeLineType());
                yield return new WaitForSeconds(8);
                StartCoroutine(LineAreaStyleSettings());
                yield return new WaitForSeconds(5);
                StartCoroutine(LineArrowSettings());
                yield return new WaitForSeconds(2);
                StartCoroutine(LineSymbolSettings());
                yield return new WaitForSeconds(7);
                StartCoroutine(LineLabelSettings());
                yield return new WaitForSeconds(3);
                StartCoroutine(LineMutilSerie());
                yield return new WaitForSeconds(5);
            }
        }

        IEnumerator AddSimpleLine()
        {
            chart = gameObject.GetComponent<LineChart>();
            if (chart == null){
                chart = gameObject.AddComponent<LineChart>();
                chart.Init();
            }
            chart.GetChartComponent<Title>().text = "LineChart - 折线图";
            chart.GetChartComponent<Title>().subText = "普通折线图";

            var yAxis = chart.GetChartComponent<YAxis>();
            yAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            yAxis.min = 0;
            yAxis.max = 100;

            chart.RemoveData();
            serie = chart.AddSerie<Line>("Line");

            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddXAxisData("x" + (i + 1));
                chart.AddData(0, UnityEngine.Random.Range(30, 90));
            }
            yield return new WaitForSeconds(1);
        }

        IEnumerator ChangeLineType()
        {
            chart.GetChartComponent<Title>().subText = "LineTyle - 曲线图";
            serie.lineType = LineType.Smooth;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "LineTyle - 阶梯线图";
            serie.lineType = LineType.StepStart;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.lineType = LineType.StepMiddle;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.lineType = LineType.StepEnd;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "LineTyle - 虚线";
            serie.lineStyle.type = LineStyle.Type.Dashed;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "LineTyle - 点线";
            serie.lineStyle.type = LineStyle.Type.Dotted;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "LineTyle - 点划线";
            serie.lineStyle.type = LineStyle.Type.DashDot;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "LineTyle - 双点划线";
            serie.lineStyle.type = LineStyle.Type.DashDotDot;
            chart.RefreshChart();

            serie.lineType = LineType.Normal;
            chart.RefreshChart();
        }

        IEnumerator LineAreaStyleSettings()
        {
            chart.GetChartComponent<Title>().subText = "AreaStyle 面积图";

            serie.EnsureComponent<AreaStyle>();
            serie.areaStyle.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);

            chart.GetChartComponent<Title>().subText = "AreaStyle 面积图";
            serie.lineType = LineType.Smooth;
            serie.areaStyle.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);

            chart.GetChartComponent<Title>().subText = "AreaStyle 面积图 - 调整透明度";
            while (serie.areaStyle.opacity > 0.4)
            {
                serie.areaStyle.opacity -= 0.6f * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "AreaStyle 面积图 - 渐变";
            serie.areaStyle.toColor = Color.white;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator LineArrowSettings()
        {
            chart.GetChartComponent<Title>().subText = "LineArrow 头部箭头";
            chart.GetSerie(0).EnsureComponent<LineArrow>();
            serie.lineArrow.show = true;
            serie.lineArrow.position = LineArrow.Position.Start;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "LineArrow 尾部箭头";
            serie.lineArrow.position = LineArrow.Position.End;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
            serie.lineArrow.show = false;
        }

        /// <summary>
        /// SerieSymbol 相关设置
        /// </summary>
        /// <returns></returns>
        IEnumerator LineSymbolSettings()
        {
            chart.GetChartComponent<Title>().subText = "SerieSymbol 图形标记";
            while (serie.symbol.size < 5)
            {
                serie.symbol.size += 2.5f * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            chart.GetChartComponent<Title>().subText = "SerieSymbol 图形标记 - 空心圆";
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "SerieSymbol 图形标记 - 实心圆";
            serie.symbol.type = SymbolType.Circle;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "SerieSymbol 图形标记 - 三角形";
            serie.symbol.type = SymbolType.Triangle;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "SerieSymbol 图形标记 - 正方形";
            serie.symbol.type = SymbolType.Rect;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "SerieSymbol 图形标记 - 菱形";
            serie.symbol.type = SymbolType.Diamond;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.GetChartComponent<Title>().subText = "SerieSymbol 图形标记";
            serie.symbol.type = SymbolType.EmptyCircle;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        /// <summary>
        /// SerieLabel相关配置
        /// </summary>
        /// <returns></returns>
        IEnumerator LineLabelSettings()
        {
            chart.GetChartComponent<Title>().subText = "SerieLabel 文本标签";
            serie.EnsureComponent<LabelStyle>();
            chart.RefreshChart();
            while (serie.label.offset[1] < 20)
            {
                serie.label.offset = new Vector3(serie.label.offset.x, serie.label.offset.y + 20f * Time.deltaTime);
                chart.RefreshChart();
                yield return null;
            }
            yield return new WaitForSeconds(1);

            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.label.textStyle.color = Color.white;
            serie.label.background.color = Color.grey;
            serie.labelDirty = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.label.show = false;
            chart.RefreshChart();
        }

        /// <summary>
        /// 添加多条线图
        /// </summary>
        /// <returns></returns>
        IEnumerator LineMutilSerie()
        {
            chart.GetChartComponent<Title>().subText = "多系列";
            var serie2 = chart.AddSerie<Line>("Line2");
            serie2.lineType = LineType.Normal;
            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddData(1, UnityEngine.Random.Range(30, 90));
            }
            yield return new WaitForSeconds(1);

            var serie3 = chart.AddSerie<Line>("Line3");
            serie3.lineType = LineType.Normal;
            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddData(2, UnityEngine.Random.Range(30, 90));
            }
            yield return new WaitForSeconds(1);

            var yAxis = chart.GetChartComponent<YAxis>();
            yAxis.minMaxType = Axis.AxisMinMaxType.Default;
            chart.GetChartComponent<Title>().subText = "多系列 - 堆叠";
            serie.stack = "samename";
            serie2.stack = "samename";
            serie3.stack = "samename";
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }
    }
}