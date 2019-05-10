using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class Demo : MonoBehaviour
{
    private Theme m_SelectedTheme;
    private GameObject m_SelectedModule;

    private GameObject m_LineChartModule;
    private GameObject m_BarChartModule;
    private GameObject m_PieChartModule;
    private GameObject m_RadarChartModule;
    private GameObject m_OtherModule;

    private Button m_DefaultThemeButton;
    private Button m_LightThemeButton;
    private Button m_DarkThemeButton;

    private Button m_LineChartButton;
    private Button m_BarChartButton;
    private Button m_PieChartButton;
    private Button m_RadarChartButton;
    private Button m_OtherButton;

    private Text m_Title;
    private Color m_NormalColor;
    private Color m_SelectedColor;
    private Color m_HighlightColor;

    private ScrollRect m_ScrollRect;

    void Awake()
    {
        m_SelectedTheme = Theme.Default;

        m_NormalColor = ChartHelper.GetColor("#293C55FF");
        m_SelectedColor = ChartHelper.GetColor("#e43c59ff");
        m_HighlightColor = ChartHelper.GetColor("#0E151FFF");

        m_ScrollRect = transform.Find("chart_detail").GetComponent<ScrollRect>();

        m_LineChartModule = transform.Find("chart_detail/line_chart").gameObject;
        m_BarChartModule = transform.Find("chart_detail/bar_chart").gameObject;
        m_PieChartModule = transform.Find("chart_detail/pie_chart").gameObject;
        m_RadarChartModule = transform.Find("chart_detail/radar_chart").gameObject;
        m_OtherModule = transform.Find("chart_detail/other").gameObject;

        m_Title = transform.Find("chart_title/Text").GetComponent<Text>();

        InitChartButton();
        InitThemeButton();
    }

    void Update()
    {

    }

    void InitChartButton()
    {
        m_LineChartButton = transform.Find("chart_list/btn_linechart").GetComponent<Button>();
        m_BarChartButton = transform.Find("chart_list/btn_barchart").GetComponent<Button>();
        m_PieChartButton = transform.Find("chart_list/btn_piechart").GetComponent<Button>();
        m_RadarChartButton = transform.Find("chart_list/btn_radarchart").GetComponent<Button>();
        m_OtherButton = transform.Find("chart_list/btn_other").GetComponent<Button>();

        m_LineChartButton.onClick.AddListener(delegate () { SelectedModule(m_LineChartModule); });
        m_BarChartButton.onClick.AddListener(delegate () { SelectedModule(m_BarChartModule); });
        m_PieChartButton.onClick.AddListener(delegate () { SelectedModule(m_PieChartModule); });
        m_RadarChartButton.onClick.AddListener(delegate () { SelectedModule(m_RadarChartModule); });
        m_OtherButton.onClick.AddListener(delegate () { SelectedModule(m_OtherModule); });

        SelectedModule(m_LineChartModule);
    }

    void InitThemeButton()
    {
        m_DefaultThemeButton = transform.Find("chart_theme/btn_default").GetComponent<Button>();
        m_LightThemeButton = transform.Find("chart_theme/btn_light").GetComponent<Button>();
        m_DarkThemeButton = transform.Find("chart_theme/btn_dark").GetComponent<Button>();

        m_DefaultThemeButton.transform.Find("selected").gameObject.SetActive(m_SelectedTheme == Theme.Default);
        m_LightThemeButton.transform.Find("selected").gameObject.SetActive(m_SelectedTheme == Theme.Light);
        m_DarkThemeButton.transform.Find("selected").gameObject.SetActive(m_SelectedTheme == Theme.Dark);

        m_DefaultThemeButton.onClick.AddListener(delegate () { SelecteTheme(Theme.Default); });
        m_LightThemeButton.onClick.AddListener(delegate () { SelecteTheme(Theme.Light); });
        m_DarkThemeButton.onClick.AddListener(delegate () { SelecteTheme(Theme.Dark); });

        SelecteTheme(Theme.Default);
    }


    void SelectedModule(GameObject module)
    {
        m_SelectedModule = module;
        m_LineChartModule.SetActive(module == m_LineChartModule);
        m_BarChartModule.SetActive(module == m_BarChartModule);
        m_PieChartModule.SetActive(module == m_PieChartModule);
        m_RadarChartModule.SetActive(module == m_RadarChartModule);
        m_OtherModule.SetActive(module == m_OtherModule);

        SetButtonColor(m_LineChartButton, m_SelectedModule, m_LineChartModule);
        SetButtonColor(m_BarChartButton, m_SelectedModule, m_BarChartModule);
        SetButtonColor(m_PieChartButton, m_SelectedModule, m_PieChartModule);
        SetButtonColor(m_RadarChartButton, m_SelectedModule, m_RadarChartModule);
        SetButtonColor(m_OtherButton, m_SelectedModule, m_OtherModule);

        m_ScrollRect.content = m_SelectedModule.GetComponent<RectTransform>();

        if (module == m_LineChartModule)
        {
            m_Title.text = "折线图 Line";
        }
        else if (module == m_BarChartModule)
        {
            m_Title.text = "柱状图 Bar";
        }
        else if (module == m_PieChartModule)
        {
            m_Title.text = "饼图 Pie";
        }
        else if (module == m_RadarChartModule)
        {
            m_Title.text = "雷达图 Radar";
        }
        else if (module == m_OtherModule)
        {
            m_Title.text = "其他";
        }
    }

    void SetButtonColor(Button btn, GameObject selected, GameObject module)
    {
        var block = btn.colors;
        block.highlightedColor = selected == module ? m_SelectedColor : m_HighlightColor;
        block.normalColor = selected == module ? m_SelectedColor : m_NormalColor;
        btn.colors = block;
    }

    void SelecteTheme(Theme theme)
    {
        m_SelectedTheme = theme;
        m_DefaultThemeButton.transform.Find("selected").gameObject.SetActive(m_SelectedTheme == Theme.Default);
        m_LightThemeButton.transform.Find("selected").gameObject.SetActive(m_SelectedTheme == Theme.Light);
        m_DarkThemeButton.transform.Find("selected").gameObject.SetActive(m_SelectedTheme == Theme.Dark);
        var charts = transform.GetComponentsInChildren<BaseChart>();
        foreach (var chart in charts)
        {
            chart.UpdateTheme(theme);
        }
    }
}
