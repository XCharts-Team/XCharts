using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [System.Serializable]
    public class Legend : JsonDataSupport, IPropertyChanged, IEquatable<Legend>
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Orient m_Orient = Orient.Horizonal;
        [SerializeField] private Location m_Location = Location.defaultRight;
        [SerializeField] private float m_ItemWidth = 50.0f;
        [SerializeField] private float m_ItemHeight = 20.0f;
        [SerializeField] private float m_ItemGap = 5;
        [SerializeField] private int m_ItemFontSize = 18;
        [SerializeField] private List<string> m_Data = new List<string>();

        private Dictionary<string, Button> m_DataBtnList = new Dictionary<string, Button>();

        public bool show { get { return m_Show; } set { m_Show = value; } }

        public Orient orient { get { return m_Orient; } set { m_Orient = value; } }

        public Location location { get { return m_Location; } set { m_Location = value; } }

        public float itemWidth { get { return m_ItemWidth; } set { m_ItemWidth = value; } }

        public float itemHeight { get { return m_ItemHeight; } set { m_ItemHeight = value; } }

        public float itemGap { get { return m_ItemGap; } set { m_ItemGap = value; } }

        public int itemFontSize { get { return m_ItemFontSize; } set { m_ItemFontSize = value; } }

        public List<string> data { get { return m_Data; } }

        public static Legend defaultLegend
        {
            get
            {
                var legend = new Legend
                {
                    m_Show = false,
                    m_Orient = Orient.Horizonal,
                    m_Location = Location.defaultTop,
                    m_ItemWidth = 60.0f,
                    m_ItemHeight = 20.0f,
                    m_ItemGap = 5,
                    m_ItemFontSize = 16
                };
                legend.location.top = 30;
                return legend;
            }
        }
        public void Copy(Legend legend)
        {
            m_Show = legend.show;
            m_Orient = legend.orient;
            m_Location.Copy(legend.location);
            m_ItemWidth = legend.itemWidth;
            m_ItemHeight = legend.itemHeight;
            m_ItemGap = legend.itemGap;
            m_ItemFontSize = legend.itemFontSize;
            m_Data.Clear();
            foreach (var d in legend.data) m_Data.Add(d);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Legend)
            {
                return Equals((Legend)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Legend other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return show == other.show &&
                orient == other.orient &&
                location == other.location &&
                itemWidth == other.itemWidth &&
                itemHeight == other.itemHeight &&
                itemGap == other.itemGap &&
                itemFontSize == other.itemFontSize &&
                ChartHelper.IsValueEqualsList<string>(m_Data, other.data);
        }

        public static bool operator ==(Legend left, Legend right)
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

        public static bool operator !=(Legend left, Legend right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void ClearData()
        {
            m_Data.Clear();
        }

        public bool ContainsData(string name)
        {
            return m_Data.Contains(name);
        }

        public void RemoveData(string name)
        {
            if (m_Data.Contains(name))
            {
                m_Data.Remove(name);
            }
        }

        public void AddData(string name)
        {
            if (!m_Data.Contains(name) && !string.IsNullOrEmpty(name))
            {
                m_Data.Add(name);
            }
        }

        public string GetData(int index)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                return m_Data[index];
            }
            return null;
        }

        public int GetIndex(string legendName)
        {
            return m_Data.IndexOf(legendName);
        }

        public void RemoveButton()
        {
            m_DataBtnList.Clear();
        }

        public void SetButton(string name, Button btn, int total)
        {
            int index = m_DataBtnList.Values.Count;
            btn.transform.localPosition = GetButtonLocationPosition(total, index);
            m_DataBtnList[name] = btn;
            btn.gameObject.SetActive(show);
            btn.GetComponentInChildren<Text>().text = name;
        }

        public void UpdateButtonColor(string name, Color color)
        {
            if (m_DataBtnList.ContainsKey(name))
            {
                m_DataBtnList[name].GetComponent<Image>().color = color;
            }
        }

        public void OnChanged()
        {
            m_Location.OnChanged();
        }

        private Vector2 GetButtonLocationPosition(int size, int index)
        {
            switch (m_Orient)
            {
                case Orient.Vertical:
                    switch (m_Location.align)
                    {
                        case Location.Align.TopCenter:
                        case Location.Align.TopLeft:
                        case Location.Align.TopRight:
                            return new Vector2(0, -index * (itemHeight + itemGap));

                        case Location.Align.Center:
                        case Location.Align.CenterLeft:
                        case Location.Align.CenterRight:
                            float totalHeight = size * itemHeight + (size - 1) * itemGap;
                            float startY = totalHeight / 2;
                            return new Vector2(0, startY - index * (itemHeight + itemGap));

                        case Location.Align.BottomCenter:
                        case Location.Align.BottomLeft:
                        case Location.Align.BottomRight:
                            return new Vector2(0, (size - index - 1) * (itemHeight + itemGap));
                    }
                    return Vector2.zero;

                case Orient.Horizonal:
                    switch (m_Location.align)
                    {
                        case Location.Align.TopLeft:
                        case Location.Align.CenterLeft:
                        case Location.Align.BottomLeft:
                            return new Vector2(index * (itemWidth + itemGap), 0);

                        case Location.Align.TopCenter:
                        case Location.Align.Center:
                        case Location.Align.BottomCenter:
                            float totalWidth = size * itemWidth + (size - 1) * itemGap;
                            float startX = totalWidth / 2;
                            return new Vector2(-startX + itemWidth / 2 + index * (itemWidth + itemGap), 0);
                        case Location.Align.TopRight:
                        case Location.Align.CenterRight:
                        case Location.Align.BottomRight:
                            return new Vector2(-(size - index - 1) * (itemWidth + itemGap), 0);
                    }
                    return Vector2.zero;
            }
            return Vector2.zero;
        }

        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            m_Data = ChartHelper.ParseStringFromString(jsonData);
        }
    }
}
