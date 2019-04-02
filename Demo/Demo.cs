using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class Demo : MonoBehaviour
{
    public Theme theme = Theme.Dark;

    private BaseChart chart;
    private float time;
    private int count;
    private Theme checkTheme = Theme.Dark;

    void Awake()
    {
        var xchart = transform.Find("xchart");
        GridLayoutGroup grid = xchart.GetComponent<GridLayoutGroup>();
        RectTransform rect = transform.GetComponent<RectTransform>();
        var wid = rect.sizeDelta.x;
        int childNum = xchart.childCount;
        float hig = grid.padding.top + childNum * (grid.cellSize.y + grid.spacing.y);
        rect.sizeDelta = new Vector2(wid, hig);
        xchart.GetComponent<RectTransform>().sizeDelta = new Vector2(wid, hig);

        chart = xchart.gameObject.GetComponentInChildren<BaseChart>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time = 0;
            count++;
            chart.UpdateData(0, Random.RandomRange(60, 150));
        }
        if (checkTheme != theme)
        {
            checkTheme = theme;
            UpdateTheme(theme);
        }
    }

    void UpdateTheme(Theme theme)
    {
        var charts = transform.Find("xchart").GetComponentsInChildren<BaseChart>();
        foreach (var chart in charts)
        {
            chart.UpdateTheme(theme);
        }
    }
}
