using UnityEngine;
using System;

namespace XCharts
{
    /// <summary>
    /// Title component, including main title and subtitle.
    /// 标题组件，包含主标题和副标题。
    /// </summary>
    [Serializable]
    public class Title : IPropertyChanged, IEquatable<Title>
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private string m_Text;
        [SerializeField] private int m_TextFontSize;
        [SerializeField] private string m_SubText;
        [SerializeField] private int m_SubTextFontSize;
        [SerializeField] private float m_ItemGap;
        [SerializeField] private Location m_Location;

        /// <summary>
        /// [default:true]
        /// Set this to false to prevent the title from showing.
        /// 是否显示标题组件。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// The main title text, supporting for \n for newlines.
        /// 主标题文本，支持使用 \n 换行。
        /// </summary>
        public string text { get { return m_Text; } set { m_Text = value; } }
        /// <summary>
        /// [default:16]
        /// main title font size.
        /// 主标题文字的字体大小。
        /// </summary>
        public int textFontSize { get { return m_TextFontSize; } set { m_TextFontSize = value; } }
        /// <summary>
        /// Subtitle text, supporting for \n for newlines.
        /// 副标题文本，支持使用 \n 换行。
        /// </summary>
        public string subText { get { return m_SubText; } set { m_Text = value; } }
        /// <summary>
        /// [default:14]
        /// subtitle font size.
        /// 副标题文字的字体大小。
        /// </summary>
        public int subTextFontSize { get { return m_SubTextFontSize; } set { m_SubTextFontSize = value; } }
        /// <summary>
        /// [default:14]
        /// The gap between the main title and subtitle.
        /// 主副标题之间的间距。
        /// </summary>
        public float itemGap { get { return m_ItemGap; } set { m_ItemGap = value; } }
        /// <summary>
        /// The location of title component.
        /// 标题显示位置。
        /// </summary>
        public Location location { get { return m_Location; } set { m_Location = value; } }

        public static Title defaultTitle
        {
            get
            {
                var title = new Title
                {
                    m_Show = true,
                    m_Text = "Chart Title",
                    m_TextFontSize = 16,
                    m_SubText = "",
                    m_SubTextFontSize = 14,
                    m_ItemGap = 14,
                    m_Location = Location.defaultTop
                };
                return title;
            }
        }
        public void Copy(Title title)
        {
            m_Show = title.show;
            m_Text = title.text;
            m_TextFontSize = title.textFontSize;
            m_SubText = title.subText;
            m_SubTextFontSize = title.subTextFontSize;
            m_ItemGap = title.itemGap;
            m_Location.Copy(title.location);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Title)
            {
                return Equals((Title)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Title other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return m_Show == other.show &&
                m_Text.Equals(other.text) &&
                m_TextFontSize == other.textFontSize &&
                m_SubText.Equals(other.subText) &&
                m_SubTextFontSize == other.subTextFontSize &&
                m_ItemGap == other.itemGap &&
                m_Location.Equals(other.location);
        }

        public static bool operator ==(Title left, Title right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }
            else if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }
            return Equals(left, right);
        }

        public static bool operator !=(Title left, Title right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void OnChanged()
        {
            m_Location.OnChanged();
        }
    }
}