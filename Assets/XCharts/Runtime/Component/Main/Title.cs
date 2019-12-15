/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using System;

namespace XCharts
{
    /// <summary>
    /// Title component, including main title and subtitle.
    /// 标题组件，包含主标题和副标题。
    /// </summary>
    [Serializable]
    public class Title : MainComponent, IPropertyChanged, IEquatable<Title>
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private string m_Text;
        [SerializeField] private TextStyle m_TextStyle = new TextStyle(16);
        [SerializeField] private string m_SubText;
        [SerializeField] private TextStyle m_SubTextStyle = new TextStyle(14);
        [SerializeField] private float m_ItemGap = 8;
        [SerializeField] private Location m_Location = Location.defaultTop;

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
        [Obsolete("use textStyle instead.", false)]
        public int textFontSize { get { return m_TextStyle.fontSize; } set { m_TextStyle.fontSize = value; } }
        /// <summary>
        /// 主标题文本样式。
        /// </summary>
        public TextStyle textStyle { get { return m_TextStyle; } set { m_TextStyle = value; } }
        /// <summary>
        /// Subtitle text, supporting for \n for newlines.
        /// 副标题文本，支持使用 \n 换行。
        /// </summary>
        public string subText { get { return m_SubText; } set { m_SubText = value; } }
        /// <summary>
        /// 副标题文本样式。
        /// </summary>
        public TextStyle subTextStyle { get { return m_SubTextStyle; } set { m_SubTextStyle = value; } }
        /// <summary>
        /// [default:14]
        /// subtitle font size.
        /// 副标题文字的字体大小。
        /// </summary>
        [Obsolete("use subTextStyle instead.", false)]
        public int subTextFontSize { get { return m_SubTextStyle.fontSize; } set { m_SubTextStyle.fontSize = value; } }
        /// <summary>
        /// [default:8]
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
                    m_TextStyle = new TextStyle(16),
                    m_SubText = "",
                    m_SubTextStyle = new TextStyle(14),
                    m_ItemGap = 8,
                    m_Location = Location.defaultTop
                };
                return title;
            }
        }
        public void Copy(Title title)
        {
            m_Show = title.show;
            m_Text = title.text;
            m_TextStyle.Copy(title.textStyle);
            m_SubTextStyle.Copy(title.subTextStyle);
            m_SubText = title.subText;
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
                m_TextStyle.Equals(other.textStyle) &&
                m_SubText.Equals(other.subText) &&
                m_SubTextStyle.Equals(other.subTextStyle) &&
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