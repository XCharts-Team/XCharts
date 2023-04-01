using System.Collections;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class Example20_BarChart : MonoBehaviour
    {
        private BarChart chart;
        private Serie serie, serie2;
        private int m_DataNum = 5;

        private void OnEnable()
        {
            StartCoroutine(PieDemo());
        }

        IEnumerator PieDemo()
        {
            while (true)
            {
                StartCoroutine(AddSimpleBar());
                yield return new WaitForSeconds(2);
                StartCoroutine(BarMutilSerie());
                yield return new WaitForSeconds(3);
                StartCoroutine(ZebraBar());
                yield return new WaitForSeconds(3);
                StartCoroutine(SameBarAndNotStack());
                yield return new WaitForSeconds(3);
                StartCoroutine(SameBarAndStack());
                yield return new WaitForSeconds(3);
                StartCoroutine(SameBarAndPercentStack());
                yield return new WaitForSeconds(10);
            }
        }

        IEnumerator AddSimpleBar()
        {
            chart = gameObject.GetComponent<BarChart>();
            if (chart == null) chart = gameObject.AddComponent<BarChart>();
            chart.EnsureChartComponent<Title>().text = "BarChart - 柱状图";
            chart.EnsureChartComponent<Title>().subText = "普通柱状图";

            var yAxis = chart.EnsureChartComponent<YAxis>();
            yAxis.minMaxType = Axis.AxisMinMaxType.Default;

            chart.RemoveData();
            serie = chart.AddSerie<Bar>("Bar1");

            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddXAxisData("x" + (i + 1));
                chart.AddData(0, UnityEngine.Random.Range(30, 90));
            }
            yield return new WaitForSeconds(1);
        }

        IEnumerator BarMutilSerie()
        {
            chart.EnsureChartComponent<Title>().subText = "多条柱状图";

            float now = serie.barWidth - 0.35f;
            while (serie.barWidth > 0.35f)
            {
                serie.barWidth -= now * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }

            serie2 = chart.AddSerie<Bar>("Bar2");
            serie2.lineType = LineType.Normal;
            serie2.barWidth = 0.35f;
            for (int i = 0; i < m_DataNum; i++)
            {
                chart.AddData(1, UnityEngine.Random.Range(20, 90));
            }
            yield return new WaitForSeconds(1);
        }

        IEnumerator ZebraBar()
        {
            chart.EnsureChartComponent<Title>().subText = "斑马柱状图";
            serie.barType = BarType.Zebra;
            serie2.barType = BarType.Zebra;
            serie.barZebraWidth = serie.barZebraGap = 4;
            serie2.barZebraWidth = serie2.barZebraGap = 4;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator SameBarAndNotStack()
        {
            chart.EnsureChartComponent<Title>().subText = "非堆叠同柱";
            serie.barType = serie2.barType = BarType.Normal;
            serie.stack = "";
            serie2.stack = "";
            serie.barGap = -1;
            serie2.barGap = -1;
            yield return new WaitForSeconds(1);
        }

        IEnumerator SameBarAndStack()
        {
            chart.EnsureChartComponent<Title>().subText = "堆叠同柱";
            serie.barType = serie2.barType = BarType.Normal;
            serie.stack = "samename";
            serie2.stack = "samename";
            yield return new WaitForSeconds(1);
            float now = 0.6f - serie.barWidth;
            while (serie.barWidth < 0.6f)
            {
                serie.barWidth += now * Time.deltaTime;
                serie2.barWidth += now * Time.deltaTime;
                chart.RefreshChart();
                yield return null;
            }
            serie.barWidth = serie2.barWidth;
            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }

        IEnumerator SameBarAndPercentStack()
        {
            chart.EnsureChartComponent<Title>().subText = "百分比堆叠同柱";
            serie.barType = serie2.barType = BarType.Normal;
            serie.stack = "samename";
            serie2.stack = "samename";

            serie.barPercentStack = true;
            if (null == serie.label)
            {
                serie.EnsureComponent<LabelStyle>();
            }
            serie.label.show = true;
            serie.label.position = LabelStyle.Position.Center;
            serie.label.textStyle.color = Color.white;
            serie.label.formatter = "{d:f0}%";

            if (null == serie2.label)
            {
                serie2.EnsureComponent<LabelStyle>();
            }
            serie2.label.show = true;
            serie2.label.position = LabelStyle.Position.Center;
            serie2.label.textStyle.color = Color.white;
            serie2.label.formatter = "{d:f0}%";
            serie2.labelDirty = true;

            chart.RefreshChart();
            yield return new WaitForSeconds(1);
        }
    }
}