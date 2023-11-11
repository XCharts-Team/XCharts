using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// padding setting of item or text.
    /// ||边距设置。
    /// </summary>
    [Serializable]
    public class Padding : ChildComponent
    {
        [SerializeField] protected bool m_Show = true;
        [SerializeField] protected float m_Top = 0;
        [SerializeField] protected float m_Right = 2f;
        [SerializeField] protected float m_Left = 2f;
        [SerializeField] protected float m_Bottom = 0;

        public Padding() { }

        public Padding(float top, float right, float bottom, float left)
        {
            SetPadding(top, right, bottom, left);
        }

        public void SetPadding(float top, float right, float bottom, float left)
        {
            m_Top = top;;
            m_Right = right;
            m_Bottom = bottom;
            m_Left = left;
        }
        /// <summary>
        /// show padding.
        /// 是否显示。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// padding of top.
        /// ||顶部间距。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtil.SetStruct(ref m_Top, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// padding of right.
        /// ||右部间距。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtil.SetStruct(ref m_Right, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// padding of bottom.
        /// ||底部间距。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtil.SetStruct(ref m_Bottom, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// padding of left.
        /// ||左边间距。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtil.SetStruct(ref m_Left, value)) SetComponentDirty(); }
        }
    }
}