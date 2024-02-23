using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class Example30_PieChart : MonoBehaviour
    {
        private PieChart chart;
        private Serie serie, serie1;
        private float m_RadiusSpeed = 100f;
        private float m_CenterSpeed = 1f;

        private void OnEnable()
        {
            StartCoroutine(PieDemo());
        }

        IEnumerator PieDemo()
        {
            while (true)
            {
                StartCoroutine(PieAdd());
                yield return new WaitForSeconds(2);
                StartCoroutine(PieShowLabel());
                yield return new WaitForSeconds(4);
                StartCoroutine(Doughnut());
                yield return new WaitForSeconds(3);
                StartCoroutine(DoublePie());
                yield return new WaitForSeconds(2);
                StartCoroutine(RosePie());
                yield return new WaitForSeconds(5);
            }
        }

        IEnumerator PieAdd()
        {
            chart = gameObject.GetComponent<PieChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<PieChart>();
                chart.Init();
            }
            yield return null;
            chart.GetChartComponent<Title>().text = "PieChart - 饼图";
            chart.GetChartComponent<Title>().subText = "基础饼图";

            var legend = chart.EnsureChartComponent<Legend>();
            legend.show = true;
            legend.location.align = Location.Align.TopLeft;
            legend.location.top = 60;
            legend.location.left = 2;
            legend.itemWidth = 70;
            legend.itemHeight = 20;
            legend.orient = Orient.Vertical;

            chart.RemoveData();
            serie = chart.AddSerie<Pie>("访问来源");
            serie.radius[0] = 0;
            serie.radius[1] = 110;
            serie.center[0] = 0.5f;
            serie.center[1] = 0.4f;
            chart.AddData(0, 335, "直接访问");
            chart.AddData(0, 310, "邮件营销");
            chart.AddData(0, 243, "联盟广告");
            chart.AddData(0, 135, "视频广告");
            chart.AddData(0, 1548, "搜索引擎");

            chart.onSerieClick = delegate (SerieEventData data)
            {

            };
            yield return new WaitForSeconds(1);
        }

        IEnumerator PieShowLabel()
        {
            chart.EnsureChartComponent<Title>().subText = "显示文本标签";

            serie.EnsureComponent<LabelStyle>();
            serie.label.show = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
            serie.labelLine.lineType = LabelLine.LineType.Curves;
            chart.RefreshChart();

            yield return new WaitForSeconds(1);
            serie.labelLine.lineType = LabelLine.LineType.HorizontalLine;
            chart.RefreshChart();

            yield return new WaitForSeconds(1);
            serie.labelLine.lineType = LabelLine.LineType.BrokenLine;
            chart.RefreshChart();

            yield return new WaitForSeconds(1);
            serie.labelLine.show = false;
            chart.RefreshChart();
        }

        IEnumerator Doughnut()
        {
            chart.EnsureChartComponent<Title>().subText = "圆环图";
            serie.radius[0] = 2f;
            while (serie.radius[0] < serie.radius[1] * 0.7f)
            {
                serie.radius[0] += m_RadiusSpeed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            serie.gap = 1f;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.data[0].selected = true;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);

            serie.gap = 0f;
            serie.data[0].selected = false;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator DoublePie()
        {
            chart.EnsureChartComponent<Title>().subText = "多图组合";
            serie1 = chart.AddSerie<Pie>("访问来源2");
            chart.AddData(1, 335, "直达");
            chart.AddData(1, 679, "营销广告");
            chart.AddData(1, 1548, "搜索引擎");
            serie1.radius[0] = 0;
            serie1.radius[1] = 2f;
            serie1.center[0] = 0.5f;
            serie1.center[1] = 0.4f;
            chart.RefreshChart();
            while (serie1.radius[1] < serie.radius[0] * 0.75f)
            {
                serie1.radius[1] += m_RadiusSpeed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            if (null == serie.label)
            {
                serie.EnsureComponent<LabelStyle>();
            }
            if (null == serie1.label)
            {
                serie1.EnsureComponent<LabelStyle>();
            }
            serie1.label.show = true;
            serie1.label.position = LabelStyle.Position.Inside;
            serie1.label.textStyle.color = Color.white;
            serie1.label.textStyle.fontSize = 14;

            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator RosePie()
        {
            chart.EnsureChartComponent<Title>().subText = "玫瑰图";
            chart.EnsureChartComponent<Legend>().show = false;
            serie1.ClearData();
            serie.ClearData();
            serie1.radius = serie.radius = new float[2] { 0, 80 };
            serie1.label.position = LabelStyle.Position.Outside;
            serie1.labelLine.lineType = LabelLine.LineType.Curves;
            serie1.label.textStyle.color = Color.clear;
            for (int i = 0; i < 2; i++)
            {
                chart.AddData(i, 10, "rose1");
                chart.AddData(i, 5, "rose2");
                chart.AddData(i, 15, "rose3");
                chart.AddData(i, 25, "rose4");
                chart.AddData(i, 20, "rose5");
                chart.AddData(i, 35, "rose6");
                chart.AddData(i, 30, "rose7");
                chart.AddData(i, 40, "rose8");
            }

            while (serie.center[0] > 0.25f || serie1.center[0] < 0.7f)
            {
                if (serie.center[0] > 0.25f) serie.center[0] -= m_CenterSpeed * Time.deltaTime;
                if (serie1.center[0] < 0.7f) serie1.center[0] += m_CenterSpeed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            yield return new WaitForSeconds(1);
            while (serie.radius[0] > 3f)
            {
                serie.radius[0] -= m_RadiusSpeed * Time.deltaTime;
                serie1.radius[0] -= m_RadiusSpeed * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }

            serie.radius[0] = 0;
            serie1.radius[0] = 0;
            serie.pieRoseType = RoseType.Area;
            serie1.pieRoseType = RoseType.Radius;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }
    }
}