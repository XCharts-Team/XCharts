
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XCharts;

namespace XChartsDemo
{
    [System.Serializable]
    public class ChartModule
    {
        [SerializeField] private string m_Name = "Name";
        [SerializeField] private string m_SubName = "SubName";
        [SerializeField] private int m_Column = 3;

        [SerializeField] private string m_Title = "Title";
        [SerializeField] private bool m_Selected;
        [SerializeField] private GameObject m_Panel;
        [SerializeField] private List<GameObject> m_ChartPrefabs;

        public int column { get { return m_Column; } set { m_Column = value; } }
        public string name { get { return m_Name; } set { m_Name = value; } }
        public string subName { get { return m_SubName; } set { m_SubName = value; } }
        public string title { get { return m_Title; } set { m_Title = value; } }
        public bool select { get { return m_Selected; } set { m_Selected = value; } }
        public GameObject panel { get { return m_Panel; } set { m_Panel = value; } }
        public Button button { get; set; }
        public List<GameObject> chartPrefabs { get { return m_ChartPrefabs; } }
        public int index { get; internal set; }
    }

    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Demo : MonoBehaviour
    {
        [SerializeField] private float m_LeftWidth = 150;
        [SerializeField] private float m_LeftButtonHeight = 60;
        //[SerializeField] private Vector2 m_ChartSpacing = new Vector2(5, 5);
        //[SerializeField] private Vector2 m_ChartSizeRatio = new Vector2(5, 3);
        [SerializeField] private Color m_ButtonNormalColor;
        [SerializeField] private Color m_ButtonSelectedColor;
        [SerializeField] private Color m_ButtonHighlightColor;
        [SerializeField] public int lastSelectedModuleIndex = -1;
        [SerializeField] private List<ChartModule> m_ChartModule = new List<ChartModule>();

        private GameObject m_BtnClone;
        private GameObject m_ThumbClone;
        private ThemeType m_SelectedTheme;
        private GameObject m_SelectedPanel;
        private int m_LastSelectedModuleIndex;
        private float m_LastCheckLeftWidth;

        private Toggle m_DarkThemeToggle;

        private Text m_Title;

        private ScrollRect m_ScrollRect;
        private Mask m_Mark;
        private GameObject m_DetailRoot;
        private GameObject m_DetailChartRoot;

        public List<ChartModule> chartModules { get { return m_ChartModule; } }

        void Awake()
        {
            m_SelectedTheme = ThemeType.Default;

            m_ButtonNormalColor = ChartHelper.GetColor("#293C55FF");
            m_ButtonSelectedColor = ChartHelper.GetColor("#e43c59ff");
            m_ButtonHighlightColor = ChartHelper.GetColor("#0E151FFF");
            Init();
        }


        void Init()
        {
            m_ScrollRect = transform.Find("chart_list").GetComponent<ScrollRect>();
            m_Mark = transform.Find("chart_list/Viewport").GetComponent<Mask>();
            m_Mark.enabled = true;
            m_Title = transform.Find("chart_title/Text").GetComponent<Text>();
            m_ThumbClone = transform.Find("clone_thumb").gameObject;
            m_ThumbClone.SetActive(false);
            m_DetailRoot = transform.Find("chart_detail").gameObject;
            m_DetailChartRoot = transform.Find("chart_detail/chart").gameObject;
            m_DetailRoot.SetActive(false);
            InitThemeButton();
            InitModuleButton();
            InitChartList();
            InitSize();
        }

        void InitSize()
        {
            UIUtil.SetRectTransformWidth(transform, m_LeftWidth, "chart_btn");
            UIUtil.SetRectTransformLeft(transform, m_LeftWidth, "chart_list");
            UIUtil.SetRectTransformLeft(transform, m_LeftWidth, "chart_title");
            UIUtil.SetGridLayoutGroup(transform, new Vector2(m_LeftWidth, m_LeftButtonHeight), new Vector2(0, 2), "chart_btn");
            foreach (var module in m_ChartModule)
            {
                SetChartRootInfo(module);
            }
        }

        private void SetChartGridLayoutGroup(GridLayoutGroup grid, int column)
        {
//             if (grid == null) return;
//             var screenWidth = Screen.width;
//             var screenHeight = Screen.height;
// #if UNITY_EDITOR
//             screenWidth = Camera.main.pixelWidth;
// #endif
//             var chartWidth = (screenWidth - m_LeftWidth - m_ChartSpacing.x * (column + 1)) / column;

//             var rect = grid.gameObject.GetComponent<RectTransform>();
//             rect.anchorMin = new Vector2(0, 1);
//             rect.anchorMax = new Vector2(1, 1);
//             rect.pivot = new Vector2(0, 1);
//             rect.anchoredPosition = Vector2.zero;
//             rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);
//             grid.spacing = m_ChartSpacing;
//             grid.cellSize = new Vector2(chartWidth, chartWidth * m_ChartSizeRatio.y / m_ChartSizeRatio.x);
        }

        private void SetChartRootInfo(ChartModule module)
        {
            m_SelectedPanel = module.panel;
            if (m_SelectedPanel == null) return;
            var grid = m_SelectedPanel.GetComponent<GridLayoutGroup>();
            var hig = Mathf.CeilToInt(m_SelectedPanel.transform.childCount * 1f / module.column) * (grid.cellSize.y + grid.spacing.y);
            SetChartGridLayoutGroup(grid, module.column);
            UIUtil.SetRectTransformHeight(m_SelectedPanel.transform, hig);
        }

        void ResetParam()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                chart.RemoveChartObject();
            }
        }

        void Update()
        {
#if UNITY_EDITOR
            if (m_ChartModule.Count <= 0) return;
            if (!Application.isPlaying) m_Mark.enabled = false;

            if (m_LastCheckLeftWidth != m_LeftWidth)
            {
                m_LastCheckLeftWidth = m_LeftWidth;
                InitSize();
            }
#endif
        }

        public int GetSelectedModule()
        {
            for (int i = 0; i < m_ChartModule.Count; i++)
            {
                if (m_ChartModule[i].select) return i;
            }
            return -1;
        }

        public void InitModuleButton()
        {
            var btnPanel = transform.Find("chart_btn");
            m_BtnClone = transform.Find("clone_btn").gameObject;
            m_BtnClone.SetActive(false);
            ChartHelper.DestroyAllChildren(btnPanel.transform);
            for (int i = 0; i < m_ChartModule.Count; i++)
            {
                var module = m_ChartModule[i];
                module.index = i;
                var btnName = "btn_" + module.name;
                GameObject btn;
                if (btnPanel.Find(btnName))
                {
                    btn = btnPanel.Find(btnName).gameObject;
                    btn.name = btnName;
                    ChartHelper.SetActive(btn, true);
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
                module.button.transform.Find("Text").GetComponent<Text>().text = module.name.Replace("\\n", "\n");
                module.button.transform.Find("SubText").GetComponent<Text>().text = module.subName.Replace("\\n", "\n");

                ChartHelper.ClearEventListener(btn.gameObject);
                ChartHelper.AddEventListener(btn.gameObject, EventTriggerType.PointerDown, (data) =>
                {
                    ClickModule(module);
                });
            }
            for (int i = 0; i < m_ChartModule.Count; i++)
            {
                var module = m_ChartModule[i];
                module.index = i;
                if (module.select)
                {
                    ClickModule(module);
                    break;
                }
            }
        }

        public void InitChartList()
        {
            foreach (var module in m_ChartModule)
                InitChartList(module);
        }

        public void InitChartList(ChartModule module)
        {
            ChartHelper.DestroyAllChildren(module.panel.transform);
            for (int i = 0; i < module.chartPrefabs.Count; i++)
            {
                var obj = UIUtil.Instantiate(m_ThumbClone, module.panel.transform);
                var chartParent = obj.transform.Find("chart");
                var prefab = module.chartPrefabs[i];
                if (prefab != null)
                {
                    var names = prefab.name.Split('_');
                    var chart = UIUtil.Instantiate(prefab, chartParent.transform);
                    obj.name = prefab.name;
                    chart.name = prefab.name;
                    if (names.Length == 3)
                    {
                        UIUtil.SetText(obj, names[1], "desc/Text");
                        UIUtil.SetText(obj, names[2], "desc/Text2");
                    }
                    obj.transform.Find("btn").GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        m_SelectedPanel = m_DetailChartRoot;
                        ChartHelper.DestroyAllChildren(m_DetailChartRoot.transform);
                        UIUtil.Instantiate(prefab, m_DetailChartRoot.transform);
                        m_DetailRoot.SetActive(true);
                        module.panel.SetActive(false);
                        SelecteTheme(m_SelectedTheme);
                    });
                }
            }
        }

        void ClickModule(ChartModule selectedModule)
        {
            if (lastSelectedModuleIndex >= 0)
            {
                m_ChartModule[lastSelectedModuleIndex].select = false;
            }
            lastSelectedModuleIndex = selectedModule.index;
            m_DetailRoot.SetActive(false);
            foreach (var module in m_ChartModule)
            {
                if (module.index != lastSelectedModuleIndex)
                {
                    var block = module.button.colors;
                    block.highlightedColor = m_ButtonHighlightColor;
#if UNITY_2019
                    block.selectedColor = m_ButtonNormalColor;
#endif
                    block.pressedColor = m_ButtonNormalColor;
                    block.normalColor = m_ButtonNormalColor;
                    module.button.colors = block;
                    if (module.panel != null)
                        module.panel.SetActive(false);
                }
                else
                {
                    var block = module.button.colors;
                    block.highlightedColor = m_ButtonSelectedColor;
#if UNITY_2019
                    block.selectedColor = m_ButtonSelectedColor;
#endif
                    block.pressedColor = m_ButtonSelectedColor;
                    block.normalColor = m_ButtonSelectedColor;
                    module.button.colors = block;
                    if (module.panel != null)
                        module.panel.SetActive(true);
                }
            }
            if (selectedModule.panel != null)
                m_ScrollRect.content = selectedModule.panel.GetComponent<RectTransform>();
            SetChartRootInfo(selectedModule);
            m_Title.text = string.IsNullOrEmpty(selectedModule.title) ?
                selectedModule.name : selectedModule.title;
            SelecteTheme(m_SelectedTheme);
        }

        void InitThemeButton()
        {
            m_DarkThemeToggle = transform.Find("chart_theme/dark").GetComponent<Toggle>();
            m_DarkThemeToggle.isOn = m_SelectedTheme == ThemeType.Dark;

            m_DarkThemeToggle.onValueChanged.AddListener(delegate (bool flag)
            {
                m_SelectedTheme = flag ? ThemeType.Dark : ThemeType.Default;
                SelecteTheme(m_SelectedTheme);
            });
        }

        void SelecteTheme(ThemeType theme)
        {
            m_SelectedTheme = theme;
            var charts = (m_SelectedPanel == null ? transform : m_SelectedPanel.transform).GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                chart.UpdateTheme(theme);
            }
        }
    }
}
