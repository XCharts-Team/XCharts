using UnityEngine;
using System.Collections;
using System;

namespace XCharts
{
    [Serializable]
    public class Title: IPropertyChanged,IEquatable<Title>
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private string m_Text;
        [SerializeField] private int m_TextFontSize;
        [SerializeField] private string m_SubText;
        [SerializeField] private int m_SubTextFontSize;
        [SerializeField] private float m_ItemGap;
        [SerializeField] private Location m_Location;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public string text { get { return m_Text; } set { m_Text = value; } }
        public int textFontSize { get { return m_TextFontSize; }set { m_TextFontSize = value; } }
        public string subText { get { return m_SubText; } set { m_Text = value; } }
        public int subTextFontSize { get { return m_SubTextFontSize; } set { m_SubTextFontSize = value; } }
        public float itemGap { get { return m_ItemGap; } set { m_ItemGap = value; } }
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
            if (!(obj is Title)) return false;
            return Equals((Title)obj);
        }

        public bool Equals(Title other)
        {
            return m_Show == other.show &&
                m_Text.Equals(other.text) &&
                m_TextFontSize == other.textFontSize &&
                m_SubText.Equals(other.subText) &&
                m_SubTextFontSize == other.subTextFontSize &&
                m_ItemGap == other.itemGap &&
                m_Location.Equals(other.location);
        }

        public static bool operator == (Title point1, Title point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator != (Title point1, Title point2)
        {
            return !point1.Equals(point2);
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