using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace xcharts
{
    [System.Serializable]
    public enum ChartType
    {
        line,
        bar
    }

    [System.Serializable]
    public class Title
    {
        public bool show = true;
        public string text ="Chart Title";
        public Align align = Align.center;
        public float left;
        public float right;
        public float top = 5;
        public float bottom;
    }

    [System.Serializable]
    public enum Align
    {
        left,                       //左对齐
        right,                      //右对齐
        center                      //居中对齐
    }

    [System.Serializable]
    public enum Location
    {
        left,
        right,
        top,
        bottom,
        start,
        middle,
        center,
        end,
    }

    [System.Serializable]
    public class LegendData
    {
        public bool show = true;
        public ChartType type;
        public string key;
        public string text;
        public Button button { get; set; }
    }

    [System.Serializable]
    public class Legend
    {
        public bool show = true;
        public Location location = Location.right;
        public float dataWid = 50.0f;
        public float dataHig = 20.0f;
        public float dataSpace = 5;
        public float left;
        public float right = 5;
        public float top;
        public float bottom;
        public List<LegendData> dataList = new List<LegendData>();

        public bool IsShowSeries(int seriesIndex)
        {
            if (seriesIndex < 0 || seriesIndex >= dataList.Count) seriesIndex = 0;
            return dataList[seriesIndex].show;
        }
    }

    [System.Serializable]
    public class SeriesData
    {
        public string key;
        public float value;

        public SeriesData(string key, float value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [System.Serializable]
    public class Series
    {
        public string legendKey;
        public int showDataNumber = 0;
        public List<SeriesData> dataList = new List<SeriesData>();

        public float Max
        {
            get
            {
                float max = 0;
                foreach (var data in dataList)
                {
                    if (data.value > max)
                    {
                        max = data.value;
                    }
                }
                return max;
            }
        }

        public float Total
        {
            get
            {
                float total = 0;
                foreach (var data in dataList)
                {
                    total += data.value;
                }
                return total;
            }
        }

        public void AddData(string key, float value)
        {
            if (dataList.Count >= showDataNumber && showDataNumber != 0)
            {
                dataList.RemoveAt(0);
            }
            dataList.Add(new SeriesData(key, value));
        }
    }

    public class BaseChart : MaskableGraphic
    {
        private const string TILTE_TEXT = "title";
        private const string LEGEND_TEXT = "legend";
        [SerializeField]
        protected Theme theme = Theme.Dark;
        [SerializeField]
        protected ThemeInfo themeInfo = new ThemeInfo();
        [SerializeField]
        protected Title title = new Title();
        [SerializeField]
        protected Legend legend = new Legend();
        [SerializeField]
        protected List<Series> seriesList = new List<Series>();

        private Theme checkTheme = 0;
        private Title checkTitle = new Title();
        private Legend checkLegend = new Legend();

        protected Text titleText;
        protected List<Text> legendTextList = new List<Text>();
        protected float chartWid { get { return rectTransform.sizeDelta.x; } }
        protected float chartHig { get { return rectTransform.sizeDelta.y; } }
        
        protected override void Awake()
        {
            themeInfo = ThemeInfo.Dark;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.pivot = Vector2.zero;
            InitTitle();
            InitLegend();
        }

        protected virtual void Update()
        {
            CheckTheme();
            CheckTile();
            CheckLegend();
        }

        protected override void OnDestroy()
        {
            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        public void AddData(string legend, string key, float value)
        {
            for (int i = 0; i < seriesList.Count; i++)
            {
                if (seriesList[i].legendKey == legend)
                {
                    seriesList[i].AddData(key, value);
                    break;
                }
            }
            RefreshChart();
        }

        public void UpdateTheme(Theme theme)
        {
            this.theme = theme;
            OnThemeChanged();
            SetAllDirty();
        }

        protected void HideChild(string match = null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (match == null)
                    transform.GetChild(i).gameObject.SetActive(false);
                else
                {
                    var go = transform.GetChild(i);
                    if (go.name.StartsWith(match))
                    {
                        go.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void InitTitle()
        {
            TextAnchor anchor = TextAnchor.MiddleCenter;
            Vector2 anchorMin = new Vector2(0, 0);
            Vector2 anchorMax = new Vector2(0, 0);
            Vector3 titlePosition;
            float titleWid = 200;
            float titleHig = 20;
            switch (title.align)
            {
                case Align.left:
                    anchor = TextAnchor.MiddleLeft;
                    titlePosition = new Vector3(title.left, chartHig - title.top, 0);
                    break;
                case Align.right:
                    anchor = TextAnchor.MiddleRight;
                    titlePosition = new Vector3(chartWid - title.right - titleWid, chartHig - title.top, 0);
                    break;
                case Align.center:
                    anchor = TextAnchor.MiddleCenter;
                    titlePosition = new Vector3(chartWid / 2 - titleWid / 2, chartHig - title.top, 0);
                    break;
                default:
                    anchor = TextAnchor.MiddleCenter;
                    titlePosition = new Vector3(0, -title.top, 0);
                    break;
            }
            titleText = ChartUtils.AddTextObject(TILTE_TEXT, transform, themeInfo.font,
                        themeInfo.textColor, anchor, anchorMin, anchorMax, new Vector2(0, 1),
                        new Vector2(titleWid, titleHig), 16);
            titleText.alignment = anchor;
            titleText.gameObject.SetActive(title.show);
            titleText.transform.localPosition = titlePosition;
            titleText.text = title.text;
        }

        private void InitLegend()
        {
            for (int i = 0; i < legend.dataList.Count; i++)
            {
                LegendData data = legend.dataList[i];
                Button btn = ChartUtils.AddButtonObject(LEGEND_TEXT + i, transform, themeInfo.font,
                    themeInfo.textColor, Vector2.zero,Vector2.zero, Vector2.zero,
                    new Vector2(legend.dataWid, legend.dataHig));
                legend.dataList[i].button = btn;
                Color bcolor = data.show ? themeInfo.GetColor(i) : Color.grey;
                btn.gameObject.SetActive(legend.show);
                btn.transform.localPosition = GetLegendPosition(i);
                btn.GetComponent<Image>().color = bcolor;
                btn.GetComponentInChildren<Text>().text = data.text;
                btn.onClick.AddListener(delegate ()
                {
                    data.show = !data.show;
                    btn.GetComponent<Image>().color = data.show ? themeInfo.GetColor(i) : Color.grey;
                    OnYMaxValueChanged();
                    OnLegendButtonClicked();
                    RefreshChart();
                });
            }
        }

        private Vector3 GetLegendPosition(int i)
        {
            int legendCount = legend.dataList.Count;
            switch (legend.location)
            {
                case Location.bottom:
                case Location.top:
                    float startX = legend.left;
                    if (startX <= 0)
                    {
                        startX = (chartWid - (legendCount * legend.dataWid -
                            (legendCount - 1) * legend.dataSpace)) / 2;
                    }
                    float posY = legend.location == Location.bottom ?
                        legend.bottom : chartHig - legend.top - legend.dataHig;
                    return new Vector3(startX + i * (legend.dataWid + legend.dataSpace), posY, 0);
                case Location.left:
                case Location.right:
                    float startY = 0;
                    if (legend.top > 0)
                    {
                        startY = chartHig - legend.top - legend.dataHig;
                    }
                    else if (startY <= 0)
                    {
                        float offset = (chartHig - (legendCount * legend.dataHig - (legendCount - 1) * legend.dataSpace)) / 2;
                        startY = chartHig - offset - legend.dataHig;
                    }
                    float posX = legend.location == Location.left ? legend.left : chartWid - legend.right - legend.dataWid;
                    return new Vector3(posX, startY - i * (legend.dataHig + legend.dataSpace), 0);
                default: break;
            }
            return Vector3.zero;
        }

        protected float GetMaxValue()
        {
            float max = 0;
            for (int i = 0; i < seriesList.Count; i++)
            {
                if (legend.IsShowSeries(i) && seriesList[i].Max > max) max = seriesList[i].Max;
            }
            int bigger = (int)(max * 1.3f);
            return bigger < 10 ? bigger : bigger - bigger % 10;
        }

        private void CheckTheme()
        {
            if(checkTheme != theme)
            {
                checkTheme = theme;
                OnThemeChanged();
            }
        }

        private void CheckTile()
        {
            if (checkTitle.align != title.align ||
                checkTitle.left != title.left ||
                checkTitle.right != title.right ||
                checkTitle.top != title.top)
            {
                checkTitle.align = title.align;
                checkTitle.left = title.left;
                checkTitle.right = title.right;
                checkTitle.top = title.top;
                OnTitleChanged();
            }
        }

        private void CheckLegend()
        {
            if (checkLegend.dataWid != legend.dataWid ||
                checkLegend.dataHig != legend.dataHig ||
                checkLegend.dataSpace != legend.dataSpace ||
                checkLegend.left != legend.left ||
                checkLegend.right != legend.right ||
                checkLegend.bottom != legend.bottom ||
                checkLegend.top != legend.top ||
                checkLegend.location != legend.location ||
                checkLegend.show != legend.show)
            {
                checkLegend.dataWid = legend.dataWid;
                checkLegend.dataHig = legend.dataHig;
                checkLegend.dataSpace = legend.dataSpace;
                checkLegend.left = legend.left;
                checkLegend.right = legend.right;
                checkLegend.bottom = legend.bottom;
                checkLegend.top = legend.top;
                checkLegend.location = legend.location;
                checkLegend.show = legend.show;
                OnLegendChanged();
            }
        }

        protected virtual void OnThemeChanged()
        {
            switch (theme)
            {
                case Theme.Dark:
                    themeInfo.Copy(ThemeInfo.Dark);
                    break;
                case Theme.Default:
                    themeInfo.Copy(ThemeInfo.Default);
                    break;
                case Theme.Light:
                    themeInfo.Copy(ThemeInfo.Light);
                    break;
            }
            InitTitle();
            InitLegend();
        }

        protected virtual void OnTitleChanged()
        {
            InitTitle();
        }

        protected virtual void OnLegendChanged()
        {
            for (int i = 0; i < legend.dataList.Count; i++)
            {
                Button btn = legend.dataList[i].button;
                btn.GetComponent<RectTransform>().sizeDelta = new Vector2(legend.dataWid, legend.dataHig);
                Text txt = btn.GetComponentInChildren<Text>();
                txt.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(legend.dataWid, legend.dataHig);
                txt.transform.localPosition = Vector3.zero;
                btn.transform.localPosition = GetLegendPosition(i);
                btn.gameObject.SetActive(legend.show);
            }
        }

        protected virtual void OnYMaxValueChanged()
        {
        }

        protected virtual void OnLegendButtonClicked()
        {
        }

        protected void RefreshChart()
        {
            int tempWid = (int)chartWid;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tempWid - 1);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tempWid);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            DrawBackground(vh);
        }

        private void DrawBackground(VertexHelper vh)
        {
            // draw bg
            Vector3 p1 = new Vector3(0, chartHig);
            Vector3 p2 = new Vector3(chartWid, chartHig);
            Vector3 p3 = new Vector3(chartWid, 0);
            Vector3 p4 = new Vector3(0, 0);
            ChartUtils.DrawPolygon(vh, p1, p2, p3, p4, themeInfo.backgroundColor);
        }
    }
}