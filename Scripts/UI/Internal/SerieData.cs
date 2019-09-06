using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// A data item of serie.
    /// 系列中的一个数据项。可存储数据名和1-n维的数据。
    /// </summary>
    [System.Serializable]
    public class SerieData
    {
        [SerializeField] private string m_Name;
        [SerializeField] private bool m_Selected;
        [SerializeField] private List<float> m_Data = new List<float>();

        private bool m_Show = true;
        private GameObject m_Label;
        private bool m_LabelAutoSize;
        private float m_LabelPaddingLeftRight;
        private float m_LabelPaddingTopBottom;

        /// <summary>
        /// the name of data item.
        /// 数据项名称。
        /// </summary>
        public string name { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// Whether the data item is selected.
        /// 该数据项是否被选中。
        /// </summary>
        public bool selected { get { return m_Selected; } set { m_Selected = value; } }
        /// <summary>
        /// An arbitrary dimension data list of data item.
        /// 可指定任意维数的数值列表。
        /// </summary>
        public List<float> data { get { return m_Data; } set { m_Data = value; } }
        /// <summary>
        /// [default:true] Whether the data item is showed.
        /// 该数据项是否要显示。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// Whether the data item is highlighted.
        /// 该数据项是否被高亮，一般由鼠标悬停或图例悬停触发高亮。
        /// </summary>
        public bool highlighted { get; set; }
        /// <summary>
        /// the label of data item.
        /// 该数据项的文本标签。
        /// </summary>
        public Text labelText { get; private set; }
        public RectTransform labelRect { get; private set; }
        public Vector3 labelPosition{get;set;}
        //public Image labelImage { get; private set; }
        /// <summary>
        /// 是否可以显示Label
        /// </summary>
        public bool canShowLabel { get; set; }
        /// <summary>
        /// the maxinum value.
        /// 最大值。
        /// </summary>
        public float max { get { return m_Data.Max(); } }
        /// <summary>
        /// the mininum value.
        /// 最小值。
        /// </summary>
        public float min { get { return m_Data.Min(); } }

        public float GetData(int index)
        {
            if (index >= 0 && index < m_Data.Count) return m_Data[index];
            else return 0;
        }

        public void InitLabel(GameObject labelObj, bool autoSize, float paddingLeftRight, float paddingTopBottom)
        {
            m_Label = labelObj;
            m_LabelAutoSize = autoSize;
            m_LabelPaddingLeftRight = paddingLeftRight;
            m_LabelPaddingTopBottom = paddingTopBottom;
            labelText = labelObj.GetComponentInChildren<Text>();
            //labelImage = labelObj.GetComponent<Image>();
            labelRect = labelObj.GetComponent<RectTransform>();
        }

        public void SetLabelActive(bool active)
        {
            if (m_Label)
            {
                ChartHelper.SetActive(m_Label, active);
            }
        }

        public void SetLabelText(string text)
        {
            if (labelText)
            {
                labelText.text = text;
                if (m_LabelAutoSize)
                {
                    labelRect.sizeDelta = new Vector2(labelText.preferredWidth + m_LabelPaddingLeftRight * 2,
                                        labelText.preferredHeight + m_LabelPaddingTopBottom * 2);
                }
            }
        }

        public float GetLabelWidth()
        {
            if (labelText) return labelText.preferredWidth + m_LabelPaddingLeftRight * 2;
            else return 0;
        }

        public float GetLabelHeight()
        {
            if (labelText) return labelText.preferredHeight + m_LabelPaddingTopBottom * 2;
            return 0;
        }

        public void SetLabelPosition(Vector3 position)
        {
            if (m_Label)
            {
                m_Label.transform.localPosition = position;
            }
        }
    }
}
