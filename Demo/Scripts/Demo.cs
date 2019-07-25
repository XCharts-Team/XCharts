using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XCharts;


[System.Serializable]
public class ChartModule
{
    [SerializeField] private string m_Name;
    [SerializeField] private string m_Title;
    [SerializeField] private bool m_Selected;
    [SerializeField] private GameObject m_Panel;

    public string name { get { return m_Name; } set { m_Name = value; } }
    public string title { get { return m_Title; } set { m_Title = value; } }
    public bool select { get { return m_Selected; } set { m_Selected = value; } }
    public GameObject panel { get { return m_Panel; } set { m_Panel = value; } }
    public Button button { get; set; }
}

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class Demo : MonoBehaviour
{
    [SerializeField] private Color m_ButtonNormalColor;
    [SerializeField] private Color m_ButtonSelectedColor;
    [SerializeField] private Color m_ButtonHighlightColor;
    [SerializeField] private List<ChartModule> m_ChartModule;

    private GameObject m_BtnClone;
    private Theme m_SelectedTheme;
    private int m_LastSelectedModuleIndex;

    private Button m_DefaultThemeButton;
    private Button m_LightThemeButton;
    private Button m_DarkThemeButton;

    private Text m_Title;

    private ScrollRect m_ScrollRect;
    private Mask m_Mark;

    void Awake()
    {
        m_SelectedTheme = Theme.Default;

        m_ButtonNormalColor = ChartHelper.GetColor("#293C55FF");
        m_ButtonSelectedColor = ChartHelper.GetColor("#e43c59ff");
        m_ButtonHighlightColor = ChartHelper.GetColor("#0E151FFF");

        m_ScrollRect = transform.Find("chart_detail").GetComponent<ScrollRect>();
        m_Mark = transform.Find("chart_detail/Viewport").GetComponent<Mask>();
        m_Mark.enabled = true;
        m_Title = transform.Find("chart_title/Text").GetComponent<Text>();

        InitThemeButton();
        InitModuleButton();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (m_ChartModule.Count <= 0) return;
        int selectedModuleIndex = -1;
        for (int i = 0; i < m_ChartModule.Count; i++)
        {
            if (selectedModuleIndex >= 0 && i > selectedModuleIndex)
            {
                m_ChartModule[i].select = false;
            }
            else if (m_ChartModule[i].select)
            {
                selectedModuleIndex = i;
            }
        }
        if (selectedModuleIndex < 0) selectedModuleIndex = 0;
        if (selectedModuleIndex != m_LastSelectedModuleIndex)
        {
            InitModuleButton();
        }
#endif
    }

    void InitModuleButton()
    {
        var btnPanel = transform.Find("chart_list");
        m_BtnClone = transform.Find("btn_clone").gameObject;
        m_BtnClone.SetActive(false);
        ChartHelper.DestoryAllChilds(btnPanel);
        foreach (var module in m_ChartModule)
        {
            var btnName = "btn_" + module.name;
            GameObject btn;
            if (btnPanel.Find(btnName))
            {
                btn = btnPanel.Find(btnName).gameObject;
                btn.SetActive(true);
            }
            else
            {
                btn = GameObject.Instantiate(m_BtnClone);
                btn.SetActive(true);
                btn.name = btnName;
                btn.transform.SetParent(btnPanel);
                btn.transform.localPosition = Vector3.zero;
            }
            btn.transform.localScale = Vector3.one;
            module.button = btn.GetComponent<Button>();
            module.button.GetComponentInChildren<Text>().text = module.name;

            ChartHelper.AddEventListener(btn.gameObject, EventTriggerType.PointerDown, (data) =>
            {
                ClickModule(module);
            });
        }

        for (int i = 0; i < m_ChartModule.Count; i++)
        {
            var module = m_ChartModule[i];
            if (module.select)
            {
                ClickModule(module);
                m_LastSelectedModuleIndex = i;
                break;
            }
        }
    }

    void ClickModule(ChartModule selectedModule)
    {
        foreach (var module in m_ChartModule)
        {
            if (selectedModule != module)
            {
                var block = module.button.colors;
                block.highlightedColor = m_ButtonHighlightColor;
                block.normalColor = m_ButtonNormalColor;
                module.button.colors = block;
                module.panel.SetActive(false);
                module.select = false;
            }
            else
            {
                var block = module.button.colors;
                block.highlightedColor = m_ButtonSelectedColor;
                block.normalColor = m_ButtonSelectedColor;
                module.button.colors = block;
                module.panel.SetActive(true);
                module.select = true;
            }
        }
        m_ScrollRect.content = selectedModule.panel.GetComponent<RectTransform>();
        m_Title.text = string.IsNullOrEmpty(selectedModule.title) ?
            selectedModule.name : selectedModule.title;
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

        //SelecteTheme(Theme.Default);
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
