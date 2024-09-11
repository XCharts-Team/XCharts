using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 图形样式。
    /// </summary>
    [System.Serializable]
    public class ItemStyle : ChildComponent, ISerieDataComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Color32 m_Color;
        [SerializeField] private Color32 m_Color0;
        [SerializeField] private Color32 m_ToColor;
        [SerializeField] private Color32 m_ToColor2;
        [SerializeField][Since("v3.6.0")] private Color32 m_MarkColor;
        [SerializeField] private Color32 m_BackgroundColor;
        [SerializeField] private float m_BackgroundWidth;
        [SerializeField] private Color32 m_CenterColor;
        [SerializeField] private float m_CenterGap;
        [SerializeField] private float m_BorderWidth = 0;
        [SerializeField] private float m_BorderGap = 0;
        [SerializeField] private Color32 m_BorderColor;
        [SerializeField] private Color32 m_BorderColor0;
        [SerializeField] private Color32 m_BorderToColor;
        [SerializeField][Range(0, 1)] private float m_Opacity = 1;
        [SerializeField] private string m_ItemMarker;
        [SerializeField] private string m_ItemFormatter;
        [SerializeField] private string m_NumericFormatter = "";
        [SerializeField] private float[] m_CornerRadius = new float[] { 0, 0, 0, 0 };

        public void Reset()
        {
            m_Show = false;
            m_Color = Color.clear;
            m_Color0 = Color.clear;
            m_ToColor = Color.clear;
            m_ToColor2 = Color.clear;
            m_MarkColor = Color.clear;
            m_BackgroundColor = Color.clear;
            m_BackgroundWidth = 0;
            m_CenterColor = Color.clear;
            m_CenterGap = 0;
            m_BorderWidth = 0;
            m_BorderGap = 0;
            m_BorderColor = Color.clear;
            m_BorderColor0 = Color.clear;
            m_BorderToColor = Color.clear;
            m_Opacity = 1;
            m_ItemFormatter = null;
            m_ItemMarker = null;
            m_NumericFormatter = "";
            if (m_CornerRadius == null)
            {
                m_CornerRadius = new float[] { 0, 0, 0, 0 };
            }
            else
            {
                for (int i = 0; i < m_CornerRadius.Length; i++)
                    m_CornerRadius[i] = 0;
            }
        }

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据项颜色。
        /// </summary>
        public Color32 color
        {
            get { return m_Color; }
            set { if (PropertyUtil.SetColor(ref m_Color, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据项颜色。
        /// </summary>
        public Color32 color0
        {
            get { return m_Color0; }
            set { if (PropertyUtil.SetColor(ref m_Color0, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Gradient color1.
        /// ||渐变色的颜色1。
        /// </summary>
        public Color32 toColor
        {
            get { return m_ToColor; }
            set { if (PropertyUtil.SetColor(ref m_ToColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Gradient color2.Only valid in line diagrams.
        /// ||渐变色的颜色2。只在折线图中有效。
        /// </summary>
        public Color32 toColor2
        {
            get { return m_ToColor2; }
            set { if (PropertyUtil.SetColor(ref m_ToColor2, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Serie's mark color. It is only used to display Legend and Tooltip, and does not affect the drawing color. The default value is clear.
        /// ||Serie的标识颜色。仅用于Legend和Tooltip的展示，不影响绘制颜色，默认为clear。
        /// </summary>
        public Color32 markColor
        {
            get { return m_MarkColor; }
            set { if (PropertyUtil.SetStruct(ref m_MarkColor, value)) { SetAllDirty(); } }
        }
        /// <summary>
        /// 数据项背景颜色。
        /// </summary>
        public Color32 backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据项背景宽度。
        /// </summary>
        public float backgroundWidth
        {
            get { return m_BackgroundWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BackgroundWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 中心区域颜色。
        /// </summary>
        public Color32 centerColor
        {
            get { return m_CenterColor; }
            set { if (PropertyUtil.SetColor(ref m_CenterColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 中心区域间隙。
        /// </summary>
        public float centerGap
        {
            get { return m_CenterGap; }
            set { if (PropertyUtil.SetStruct(ref m_CenterGap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框的颜色。
        /// </summary>
        public Color32 borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtil.SetColor(ref m_BorderColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框的颜色。
        /// </summary>
        public Color32 borderColor0
        {
            get { return m_BorderColor0; }
            set { if (PropertyUtil.SetColor(ref m_BorderColor0, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框的渐变色。
        /// </summary>
        public Color32 borderToColor
        {
            get { return m_BorderToColor; }
            set { if (PropertyUtil.SetColor(ref m_BorderToColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框宽。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 边框间隙。
        /// </summary>
        public float borderGap
        {
            get { return m_BorderGap; }
            set { if (PropertyUtil.SetStruct(ref m_BorderGap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
        /// </summary>
        public float opacity
        {
            get { return m_Opacity; }
            set { if (PropertyUtil.SetStruct(ref m_Opacity, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 提示框单项的字符串模版格式器。具体配置参考`Tooltip`的`formatter`
        /// </summary>
        public string itemFormatter
        {
            get { return m_ItemFormatter; }
            set { if (PropertyUtil.SetClass(ref m_ItemFormatter, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 提示框单项的字符标志。用在Tooltip中。
        /// </summary>
        public string itemMarker
        {
            get { return m_ItemMarker; }
            set { if (PropertyUtil.SetClass(ref m_ItemMarker, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Standard number and date format string. Used to format a Double value or a DateTime date as a string. 
        /// numericFormatter is used as an argument to either `Double.ToString ()` or `DateTime.ToString()`. <br />
        /// The number format uses the Axx format: A is a single-character format specifier that supports C currency, 
        /// D decimal, E exponent, F fixed-point number, G regular, N digit, P percentage, R round trip, and X hexadecimal. 
        /// xx is precision specification, from 0-99. E.g. F1, E2<br />
        /// Date format: Starts with `date`, which is used to format DateTime. Common date formats are: 
        /// yyyy year, MM month, dd day, HH hour, mm minute, ss second, fff millisecond. For example: date:yyyy-MM-dd HH:mm:ss<br />
        /// Time format: Starts with `time`, which is used to format TimeSpan. Common time formats are: 
        /// d day, HH hour, mm minute, ss second, fffffff fractional part. 
        /// Only the version of Unity2018 or later can support formatting, and the characters inside should be escaped.
        /// For example: time:HH\:mm\:ss<br />
        /// number format reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings<br/>
        /// date format reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings<br/>
        /// Note: The date and time formats are only supported by 'v3.12.0' or later.<br/>
        /// ||标准数字和日期格式字符串。用于将Double数值或DateTime日期格式化显示为字符串。numericFormatter用来作为Double.ToString()或DateTime.ToString()的参数。<br/>
        /// 数字格式使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。如：F1, E2<br/>
        /// 日期格式：以`date`开头，用来格式化DateTime，常见格式有：yyyy年，MM月，dd日，HH时，mm分，ss秒，fff毫秒。如：date:yyyy-MM-dd HH:mm:ss<br/>
        /// 时间格式：以`time`开头，用来格式化TimeSpan，常见格式有：d日，HH时，mm分，ss秒，fffffff小数部分。
        /// 需要Unity2018以上版本才支持格式化，并且里面的字符要转义。如：time:d\.HH\:mm\:ss<br/>
        /// 数值格式化参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings <br/>
        /// 日期格式化参考：https://learn.microsoft.com/zh-cn/dotnet/standard/base-types/standard-date-and-time-format-strings <br/>
        /// 时间格式化参考：https://learn.microsoft.com/zh-cn/dotnet/standard/base-types/standard-timespan-format-strings <br/>
        /// 注意：date和time格式需要`v3.12.0`以上版本才支持。
        /// </summary>
        public string numericFormatter
        {
            get { return m_NumericFormatter; }
            set { if (PropertyUtil.SetClass(ref m_NumericFormatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The radius of rounded corner. Its unit is px. Use array to respectively specify the 4 corner radiuses((clockwise upper left, upper right, bottom right and bottom left)).
        /// ||圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。
        /// </summary>
        public float[] cornerRadius
        {
            get { return m_CornerRadius; }
            set { if (PropertyUtil.SetClass(ref m_CornerRadius, value, true)) SetVerticesDirty(); }
        }

        public Color32 GetColor()
        {
            if (m_Opacity == 1 || m_Color.a == 0)
                return m_Color;

            var color = m_Color;
            color.a = (byte) (color.a * m_Opacity);
            return color;
        }

        public Color32 GetToColor()
        {
            if (m_Opacity == 1 || m_ToColor.a == 0)
                return m_ToColor;

            var color = m_ToColor;
            color.a = (byte) (color.a * m_Opacity);
            return color;
        }

        public Color32 GetColor0()
        {
            if (m_Opacity == 1 || m_Color0.a == 0)
                return m_Color0;

            var color = m_Color0;
            color.a = (byte) (color.a * m_Opacity);
            return color;
        }

        public Color32 GetColor(Color32 defaultColor)
        {
            var color = ChartHelper.IsClearColor(m_Color) ? defaultColor : m_Color;

            if (m_Opacity == 1 || color.a == 0)
                return color;

            color.a = (byte) (color.a * m_Opacity);
            return color;
        }

        public Color32 GetColor0(Color32 defaultColor)
        {
            var color = ChartHelper.IsClearColor(m_Color0) ? defaultColor : m_Color0;

            if (m_Opacity == 1 || color.a == 0)
                return color;

            color.a = (byte) (color.a * m_Opacity);
            return color;
        }

        public Color32 GetBorderColor(Color32 defaultColor)
        {
            var color = ChartHelper.IsClearColor(m_BorderColor) ? defaultColor : m_BorderColor;

            if (m_Opacity == 1 || color.a == 0)
                return color;

            color.a = (byte) (color.a * m_Opacity);
            return color;
        }

        public Color32 GetBorderColor0(Color32 defaultColor)
        {
            var color = ChartHelper.IsClearColor(m_BorderColor0) ? defaultColor : m_BorderColor0;

            if (m_Opacity == 1 || color.a == 0)
                return color;

            color.a = (byte) (color.a * m_Opacity);
            return color;
        }

        public bool IsNeedGradient()
        {
            return !ChartHelper.IsClearColor(m_ToColor) || !ChartHelper.IsClearColor(m_ToColor2);
        }

        public Color32 GetGradientColor(float value, Color32 defaultColor)
        {
            if (!IsNeedGradient())
                return ChartConst.clearColor32;

            value = Mathf.Clamp01(value);
            var startColor = ChartHelper.IsClearColor(m_Color) ? defaultColor : m_Color;
            Color32 color;

            if (!ChartHelper.IsClearColor(m_ToColor2))
            {
                if (value <= 0.5f)
                    color = Color32.Lerp(startColor, m_ToColor, 2 * value);
                else
                    color = Color32.Lerp(m_ToColor, m_ToColor2, 2 * (value - 0.5f));
            }
            else
            {
                color = Color32.Lerp(startColor, m_ToColor, value);
            }
            if (m_Opacity != 1)
            {
                color.a = (byte) (color.a * m_Opacity);
            }
            return color;
        }

        public bool IsNeedCorner()
        {
            if (m_CornerRadius == null) return false;
            foreach (var value in m_CornerRadius)
            {
                if (value != 0) return true;
            }
            return false;
        }
    }
}