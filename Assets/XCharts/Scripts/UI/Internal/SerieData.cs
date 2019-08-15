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
        public Text label { get; set; }
        public RectTransform labelRect { get; set; }
        public Image labelImage { get; set; }
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

        public void SetLabelActive(bool active)
        {
            if (labelImage)
            {
                ChartHelper.SetActive(labelImage.gameObject, active);
            }
        }

        public void SetLabelText(string text)
        {
            if (label)
            {
                label.text = text;
                labelRect.sizeDelta = new Vector2(label.preferredWidth + 4,
                    label.preferredHeight + 4);
            }
        }

        public void SetLabelPosition(Vector3 position)
        {
            if (labelImage)
            {
                labelImage.transform.localPosition = position;
            }
        }
    }
}
