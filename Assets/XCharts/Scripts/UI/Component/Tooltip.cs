using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// Tooltip component.
    /// 提示框组件
    /// </summary>
    [System.Serializable]
    public class Tooltip
    {
        /// <summary>
        /// Indicator type.
        /// 阴影指示器。
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// line indicator.
            /// 直线指示器
            /// </summary>
            Line,
            /// <summary>
            /// shadow crosshair indicator.
            /// 阴影指示器
            /// </summary>
            Shadow,
            /// <summary>
            /// no indicator displayed.
            /// 无指示器
            /// </summary>
            None,
            /// <summary>
            /// crosshair indicator, which is actually the shortcut of enable two axisPointers of two orthometric axes.
            /// 十字准星指示器。坐标轴显示Label和交叉线。
            /// </summary>
            Corss
        }

        [SerializeField] private bool m_Show;
        [SerializeField] private Type m_Type;

        private GameObject m_GameObject;
        private GameObject m_Content;
        private Text m_ContentText;
        private RectTransform m_ContentRect;
        private List<int> lastDataIndex { get; set; }

        /// <summary>
        /// Whether to show the tooltip component.
        /// 是否显示提示框组件。
        /// </summary>
        /// <returns></returns>
        public bool show { get { return m_Show; } set { m_Show = value; SetActive(value); } }
        /// <summary>
        /// Indicator type.
        /// 提示框指示器类型。
        /// </summary>
        public Type type { get { return m_Type; } set { m_Type = value; } }

        /// <summary>
        /// The data index currently indicated by Tooltip.
        /// 当前提示框所指示的数据项索引。
        /// </summary>
        public List<int> dataIndex { get; set; }
        /// <summary>
        /// the value for x indicator label.
        /// 指示器X轴上要显示的值。
        /// </summary>
        public float[] xValues { get; set; }
        /// <summary>
        /// the value for y indicator label. 
        /// 指示器Y轴上要显示的值。
        /// </summary>
        public float[] yValues { get; set; }
        /// <summary>
        /// the current pointer position.
        /// 当前鼠标位置。
        /// </summary>
        public Vector2 pointerPos { get; set; }
        /// <summary>
        /// the width of tooltip. 
        /// 提示框宽。
        /// </summary>
        public float width { get { return m_ContentRect.sizeDelta.x; } }
        /// <summary>
        /// the height of tooltip. 
        /// 提示框高。
        /// </summary>
        public float height { get { return m_ContentRect.sizeDelta.y; } }
        /// <summary>
        /// Whether the tooltip has been initialized. 
        /// 提示框是否已初始化。
        /// </summary>
        public bool inited { get { return m_GameObject != null; } }
        /// <summary>
        /// the gameObject of tooltip. 
        /// 提示框的gameObject。
        /// </summary>
        public GameObject gameObject { get { return m_GameObject; } }

        public static Tooltip defaultTooltip
        {
            get
            {
                var tooltip = new Tooltip
                {
                    m_Show = true,
                    xValues = new float[2],
                    yValues = new float[2],
                    dataIndex = new List<int>() { -1, -1 },
                    lastDataIndex = new List<int>() { -1, -1 }
                };
                return tooltip;
            }
        }

        /// <summary>
        /// 绑定提示框gameObject
        /// </summary>
        /// <param name="obj"></param>
        public void SetObj(GameObject obj)
        {
            m_GameObject = obj;
            m_GameObject.SetActive(false);
        }

        /// <summary>
        /// 绑定提示框的文本框gameObject
        /// </summary>
        /// <param name="content"></param>
        public void SetContentObj(GameObject content)
        {
            m_Content = content;
            m_ContentRect = m_Content.GetComponent<RectTransform>();
            m_ContentText = m_Content.GetComponentInChildren<Text>();
        }

        /// <summary>
        /// Keep Tooltiop displayed at the top. 
        /// 保持Tooltiop显示在最顶上
        /// </summary>
        public void UpdateToTop()
        {
            int count = m_GameObject.transform.parent.childCount;
            m_GameObject.GetComponent<RectTransform>().SetSiblingIndex(count - 1);
        }

        /// <summary>
        /// 设置提示框文本背景色
        /// </summary>
        /// <param name="color"></param>
        public void SetContentBackgroundColor(Color color)
        {
            m_Content.GetComponent<Image>().color = color;
        }

        /// <summary>
        /// 设置提示框文本字体颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetContentTextColor(Color color)
        {
            if (m_ContentText)
            {
                m_ContentText.color = color;
            }
        }

        /// <summary>
        /// 设置提示框文本内容
        /// </summary>
        /// <param name="txt"></param>
        public void UpdateContentText(string txt)
        {
            if (m_ContentText)
            {
                m_ContentText.text = txt;
                m_ContentRect.sizeDelta = new Vector2(m_ContentText.preferredWidth + 8,
                    m_ContentText.preferredHeight + 8);
            }
        }

        /// <summary>
        /// 清除提示框指示数据
        /// </summary>
        public void ClearValue()
        {
            dataIndex[0] = dataIndex[1] = -1;
            xValues[0] = xValues[1] = -1;
            yValues[0] = yValues[1] = -1;
        }

        /// <summary>
        /// 提示框是否显示
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return m_GameObject != null && m_GameObject.activeInHierarchy;
        }

        /// <summary>
        /// 设置提示框是否显示
        /// </summary>
        /// <param name="flag"></param>
        public void SetActive(bool flag)
        {
            lastDataIndex[0] = lastDataIndex[1] = -1;
            if (m_GameObject && m_GameObject.activeInHierarchy != flag)
                m_GameObject.SetActive(flag);
        }

        /// <summary>
        /// 更新文本框位置
        /// </summary>
        /// <param name="pos"></param>
        public void UpdateContentPos(Vector2 pos)
        {
            if (m_Content)
                m_Content.transform.localPosition = pos;
        }

        /// <summary>
        /// 获得当前提示框的位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetContentPos()
        {
            if (m_Content)
                return m_Content.transform.localPosition;
            else
                return Vector3.zero;
        }

        /// <summary>
        /// Whether the data item indicated by tooltip has changed. 
        /// 提示框所指示的数据项是否发生变化。
        /// </summary>
        /// <returns></returns>
        public bool IsDataIndexChanged()
        {
            return dataIndex[0] != lastDataIndex[0] ||
                dataIndex[1] != lastDataIndex[1];
        }

        /// <summary>
        /// 当前索引缓存
        /// </summary>
        public void UpdateLastDataIndex()
        {
            lastDataIndex[0] = dataIndex[0];
            lastDataIndex[1] = dataIndex[1];
        }

        /// <summary>
        /// 当前提示框是否选中数据项
        /// </summary>
        /// <returns></returns>
        public bool IsSelected()
        {
            return dataIndex[0] >= 0 || dataIndex[1] >= 0;
        }

        /// <summary>
        /// 指定索引的数据项是否被提示框选中
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsSelected(int index)
        {
            return dataIndex[0] == index || dataIndex[1] == index;
        }
    }
}