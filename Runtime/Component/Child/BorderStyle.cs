using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// The style of border.
    /// ||边框样式。
    /// </summary>
    [System.Serializable]
    [Since("v3.10.0")]
    public class BorderStyle : ChildComponent
    {
        [SerializeField] private bool m_Show = false;
        [SerializeField] private float m_BorderWidth;
        [SerializeField] private Color32 m_BorderColor;
        [SerializeField] private bool m_RoundedCorner = true;
        [SerializeField] private float[] m_CornerRadius = new float[] { 0, 0, 0, 0 };

        /// <summary>
        /// whether the border is visible.
        /// ||是否显示边框。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }

        /// <summary>
        /// the width of border.
        /// ||边框宽度。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetAllDirty(); }
        }

        /// <summary>
        /// the color of border.
        /// ||边框颜色。
        /// </summary>
        public Color32 borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtil.SetColor(ref m_BorderColor, value)) SetAllDirty(); }
        }

        /// <summary>
        /// whether the border is rounded corner.
        /// ||是否显示圆角。
        /// </summary>
        public bool roundedCorner
        {
            get { return m_RoundedCorner; }
            set { if (PropertyUtil.SetStruct(ref m_RoundedCorner, value)) SetAllDirty(); }
        }

        /// <summary>
        /// The radius of rounded corner. Its unit is px. Use array to respectively specify the 4 corner radiuses((clockwise upper left, 
        /// upper right, bottom right and bottom left)). When is set to (1,1,1,1), all corners are rounded.
        /// ||圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。当为(1,1,1,1)时为全圆角。
        /// </summary>
        public float[] cornerRadius
        {
            get { return m_CornerRadius; }
            set { if (PropertyUtil.SetClass(ref m_CornerRadius, value)) SetAllDirty(); }
        }

        public float GetRuntimeBorderWidth()
        {
            return m_Show ? m_BorderWidth : 0;
        }

        public Color32 GetRuntimeBorderColor()
        {
            return m_Show ? m_BorderColor : ColorUtil.clearColor32;
        }

        public float[] GetRuntimeCornerRadius()
        {
            return m_Show && roundedCorner ? m_CornerRadius : null;
        }
    }
}