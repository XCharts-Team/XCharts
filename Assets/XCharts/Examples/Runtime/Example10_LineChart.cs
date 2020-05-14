/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections;
using UnityEngine;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    public class Example10_LineChart : MonoBehaviour
    {
        private LineChart chart;
        private Serie serie;
        private int m_DataNum = 8;

        void Awake()
        {
            LoopDemo();
        }

        private void OnEnable()
        {
            LoopDemo();
        }

        void LoopDemo()
        {
            StopAllCoroutines();
            StartCoroutine(PieDemo());
        }

        IEnumerator PieDemo()
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
            LoopDemo();
        }

        IEnumerator AddSimpleLine()
        {
            chart = gameObject.GetComponent<LineChart>();
            if (chart == null) chart = gameObject.AddComponent<LineChart>();
            chart.title.text = "LineChart - 折线图";
            chart.title.subText = "普通折线图";

            chart.yAxis0.minMaxType = Axis.AxisMinMaxType.Custom;
            chart.yAxis0.min = 0;
            chart.yAxis0.max = 100;

            chart.RemoveData();
            serie = chart.AddSerie(SerieType.Line, "Line");

            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddXAxisData("x" + (i + 1));
                chart.AddData(0, UnityEngine.Random.Range(30, 90));
            }
            yield return new WaitForSeconds(1);
        }

        IEnumerator ChangeLineType()
        {
            chart.title.subText = "LineTyle - 曲线图";
            serie.lineType = LineType.Smooth;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "LineTyle - 阶梯线图";
            serie.lineType = LineType.StepStart;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.lineType = LineType.StepMiddle;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.lineType = LineType.StepEnd;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "LineTyle - 虚线";
            serie.lineType = LineType.Dash;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "LineTyle - 点线";
            serie.lineType = LineType.Dot;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "LineTyle - 点划线";
            serie.lineType = LineType.DashDot;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "LineTyle - 双点划线";
            serie.lineType = LineType.DashDotDot;
            chart.RefreshChart();

            serie.lineType = LineType.Normal;
            chart.RefreshChart();
        }

        IEnumerator LineAreaStyleSettings()
        {
            chart.title.subText = "AreaStyle 面积图";

            serie.areaStyle.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);

            chart.title.subText = "AreaStyle 面积图";
            serie.lineType = LineType.Smooth;
            serie.areaStyle.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1f);

            chart.title.subText = "AreaStyle 面积图 - 调整透明度";
            while (serie.areaStyle.opacity > 0.4)
            {
                serie.areaStyle.opacity -= 0.6f * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            yield return new WaitForSeconds(1);

            chart.title.subText = "AreaStyle 面积图 - 渐变";
            serie.areaStyle.toColor = Color.white;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator LineArrowSettings()
        {
            chart.title.subText = "LineArrow 头部箭头";
            serie.lineArrow.show = true;
            serie.lineArrow.position = LineArrow.Position.Start;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "LineArrow 尾部箭头";
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
            chart.title.subText = "SerieSymbol 图形标记";
            while (serie.symbol.size < 5)
            {
                serie.symbol.size += 2.5f * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            chart.title.subText = "SerieSymbol 图形标记 - 空心圆";
            yield return new WaitForSeconds(1);

            chart.title.subText = "SerieSymbol 图形标记 - 实心圆";
            serie.symbol.type = SerieSymbolType.Circle;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "SerieSymbol 图形标记 - 三角形";
            serie.symbol.type = SerieSymbolType.Triangle;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "SerieSymbol 图形标记 - 正方形";
            serie.symbol.type = SerieSymbolType.Rect;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "SerieSymbol 图形标记 - 菱形";
            serie.symbol.type = SerieSymbolType.Diamond;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            chart.title.subText = "SerieSymbol 图形标记";
            serie.symbol.type = SerieSymbolType.EmptyCircle;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        /// <summary>
        /// SerieLabel相关配置
        /// </summary>
        /// <returns></returns>
        IEnumerator LineLabelSettings()
        {
            chart.title.subText = "SerieLabel 文本标签";
            serie.label.show = true;
            serie.label.border = false;
            chart.RefreshChart();
            while (serie.label.offset[1] < 20)
            {
                serie.label.offset = new Vector3(serie.label.offset.x, serie.label.offset.y + 20f * Time.deltaTime);
                chart.RefreshChart();
                yield return null;
            }
            yield return new WaitForSeconds(1);

            serie.label.border = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.label.color = Color.white;
            serie.label.backgroundColor = Color.grey;
            chart.RefreshLabel();
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
            chart.title.subText = "多系列";
            var serie2 = chart.AddSerie(SerieType.Line, "Line2");
            serie2.lineType = LineType.Normal;
            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddData(1, UnityEngine.Random.Range(30, 90));
            }
            yield return new WaitForSeconds(1);

            var serie3 = chart.AddSerie(SerieType.Line, "Line3");
            serie3.lineType = LineType.Normal;
            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddData(2, UnityEngine.Random.Range(30, 90));
            }
            yield return new WaitForSeconds(1);

            chart.yAxis0.minMaxType = Axis.AxisMinMaxType.Default;
            chart.title.subText = "多系列 - 堆叠";
            serie.stack = "samename";
            serie2.stack = "samename";
            serie3.stack = "samename";
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }
    }
}