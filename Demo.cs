using UnityEngine;
using UnityEngine.UI;
using xcharts;

public class Demo : MonoBehaviour
{
    private LineChart lineChart;
    private float time;
    private int count;

    void Awake()
    {
        lineChart = transform.Find("xchart/line_chart").GetComponent<LineChart>();

        var xchart = transform.Find("xchart");
        GridLayoutGroup grid = xchart.GetComponent<GridLayoutGroup>();
        RectTransform rect = xchart.GetComponent<RectTransform>();
        var wid = rect.sizeDelta.x;
        int childNum = xchart.childCount;
        int numWid =(int) ((wid - grid.padding.left - grid.padding.right) / (grid.cellSize.x+grid.spacing.x));
        int numHig = (childNum + numWid - 1) / numWid;
        float hig = grid.padding.top + numHig * (grid.cellSize.y+ grid.spacing.y);
        rect.sizeDelta = new Vector2(wid,hig);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time = 0;
            count++;
            //lineChart.AddXAxisCategory("key" + count);
            //lineChart.AddData("line1", "key"+count, Random.Range(24.0f, 60.0f));
            //lineChart.AddData("line2", "key"+count, Random.Range(24.0f, 60.0f));
        }
    }
}
