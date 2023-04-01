using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    //[ExecuteInEditMode]
    public class Example_Component : MonoBehaviour
    {
        BaseChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
        }

        void ModifyComponent()
        {
            var title = chart.EnsureChartComponent<Title>();
            title.text = "Simple LineChart";
            title.subText = "normal line";

            var serie1 = chart.AddSerie<Line>();
            //var serie2 = chart.GetSerie<Line>();

            serie1.EnsureComponent<AreaStyle>();
            var label = serie1.EnsureComponent<LabelStyle>();
            label.offset = new Vector3(0, 20, 0);

            var serieData = chart.AddData(0, 20);
            serieData.radius = 10;
            var itemStyle = serieData.EnsureComponent<ItemStyle>();
            itemStyle.color = Color.blue;
        }
    }
}