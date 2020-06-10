/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// 系列数据项的图标
    /// </summary>
    [System.Serializable]
    public class IconStyle : SubComponent
    {
        public enum Layer
        {
            UnderLabel,
            AboveLabel
        }
        [SerializeField] private bool m_Show;
        [SerializeField] private Layer m_Layer;
        [SerializeField] private Sprite m_Sprite;
        [SerializeField] private Color m_Color = Color.white;
        [SerializeField] private float m_Width = 40;
        [SerializeField] private float m_Height = 40;
        [SerializeField] private Vector3 m_Offset;

        public void Reset()
        {
            m_Show = false;
            m_Layer = Layer.UnderLabel;
            m_Sprite = null;
            m_Color = Color.white;
            m_Width = 40;
            m_Height = 40;
            m_Offset = Vector3.zero;
        }
        /// <summary>
        /// Whether the data icon is show.
        /// 是否显示图标。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// 显示在上层还是在下层。
        /// </summary>
        public Layer layer { get { return m_Layer; } set { m_Layer = value; } }
        /// <summary>
        /// The image of icon.
        /// 图标的图片。
        /// </summary>
        public Sprite sprite { get { return m_Sprite; } set { m_Sprite = value; } }
        /// <summary>
        /// 图标颜色。
        /// </summary>
        public Color color { get { return m_Color; } set { m_Color = value; } }
        /// <summary>
        /// 图标宽。
        /// </summary>
        public float width { get { return m_Width; } set { m_Width = value; } }
        /// <summary>
        /// 图标高。
        /// </summary>
        public float height { get { return m_Height; } set { m_Height = value; } }
        /// <summary>
        /// 图标偏移。
        /// </summary>
        public Vector3 offset { get { return m_Offset; } set { m_Offset = value; } }


    }
}
