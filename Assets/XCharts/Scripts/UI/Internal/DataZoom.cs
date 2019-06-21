using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [System.Serializable]
    public class DataZoom
    {
        public enum DataZoomType
        {
            Inside,
            Slider
        }
        public enum FilterMode
        {
            Filter,
            WeakFilter,
            Empty,
            None
        }
        public enum RangeMode
        {
            //Value,
            Percent
        }
        [SerializeField] private bool m_Show;
        [SerializeField] private DataZoomType m_Type;
        [SerializeField] private FilterMode m_FilterMode;
        [SerializeField] private Orient m_Orient;
        [SerializeField] private bool m_ShowDataShadow;
        [SerializeField] private bool m_ShowDetail;
        [SerializeField] private bool m_ZoomLock;
        [SerializeField] private bool m_Realtime;
        [SerializeField] private Color m_BackgroundColor;
        [SerializeField] private float m_Height;
        [SerializeField] private float m_Bottom;
        [SerializeField] private RangeMode m_RangeMode;
        [SerializeField] private float m_Start;
        [SerializeField] private float m_End;
        [SerializeField] private float m_StartValue;
        [SerializeField] private float m_EndValue;
        [Range(1f, 20f)]
        [SerializeField] private float m_ScrollSensitivity;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public DataZoomType type { get { return m_Type; } set { m_Type = value; } }
        public FilterMode filterMode { get { return m_FilterMode; } set { m_FilterMode = value; } }
        public Orient orient { get { return m_Orient; } set { m_Orient = value; } }
        public bool showDataShadow { get { return m_ShowDataShadow; } set { m_ShowDataShadow = value; } }
        public bool showDetail { get { return m_ShowDetail; } set { m_ShowDetail = value; } }
        public bool zoomLock { get { return m_ZoomLock; } set { m_ZoomLock = value; } }
        public bool realtime { get { return m_Realtime; } set { m_Realtime = value; } }
        public Color backgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; } }
        public float bottom { get { return m_Bottom; } set { m_Bottom = value; } }
        public float height { get { return m_Height; } set { m_Height = value; } }
        public RangeMode rangeMode { get { return m_RangeMode; } set { m_RangeMode = value; } }
        public float start { get { return m_Start; } set { m_Start = value; } }
        public float end { get { return m_End; } set { m_End = value; } }
        public float startValue { get { return m_StartValue; } set { m_StartValue = value; } }
        public float endValue { get { return m_EndValue; } set { m_EndValue = value; } }
        public float scrollSensitivity { get { return m_ScrollSensitivity; } set { m_ScrollSensitivity = value; } }

        public bool isDraging { get; set; }
        public Text startLabel { get; set; }
        public Text endLabel { get; set; }

        public static DataZoom defaultDataZoom
        {
            get
            {
                return new DataZoom()
                {
                    m_Type = DataZoomType.Slider,
                    filterMode = FilterMode.None,
                    orient = Orient.Horizonal,
                    showDataShadow = true,
                    showDetail = false,
                    zoomLock = false,
                    realtime = true,
                    m_Height = 50,
                    m_Bottom = 10,
                    rangeMode = RangeMode.Percent,
                    start = 30,
                    end = 70,
                    m_ScrollSensitivity = 10,
                };
            }
        }

        public bool IsInZoom(Vector2 pos, float startX, float width)
        {
            Rect rect = Rect.MinMaxRect(startX, m_Bottom, startX + width, m_Bottom + m_Height);
            return rect.Contains(pos);
        }

        public bool IsInSelectedZoom(Vector2 pos, float startX, float width)
        {
            var start = startX + width * m_Start / 100;
            var end = startX + width * m_End / 100;
            Rect rect = Rect.MinMaxRect(start, m_Bottom, end, m_Bottom + m_Height);
            return rect.Contains(pos);
        }

        public bool IsInStartZoom(Vector2 pos, float startX, float width)
        {
            var start = startX + width * m_Start / 100;
            Rect rect = Rect.MinMaxRect(start - 10, m_Bottom, start + 10, m_Bottom + m_Height);
            return rect.Contains(pos);
        }

        public bool IsInEndZoom(Vector2 pos, float startX, float width)
        {
            var end = startX + width * m_End / 100;
            Rect rect = Rect.MinMaxRect(end - 10, m_Bottom, end + 10, m_Bottom + m_Height);
            return rect.Contains(pos);
        }

        public void SetLabelActive(bool flag)
        {
            if (startLabel && startLabel.gameObject.activeInHierarchy != flag)
            {
                startLabel.gameObject.SetActive(flag);
            }
            if (endLabel && endLabel.gameObject.activeInHierarchy != flag)
            {
                endLabel.gameObject.SetActive(flag);
            }
        }

        public void SetStartLabelText(string text)
        {
            if (startLabel) startLabel.text = text;
        }

        public void SetEndLabelText(string text)
        {
            if (endLabel) endLabel.text = text;
        }
    }
}