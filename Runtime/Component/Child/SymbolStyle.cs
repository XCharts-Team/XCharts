using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// the type of symbol.
    /// ||标记图形的类型。
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// 不显示标记。
        /// </summary>
        None,
        /// <summary>
        /// 自定义标记。
        /// </summary>
        Custom,
        /// <summary>
        /// 圆形。
        /// </summary>
        Circle,
        /// <summary>
        /// 空心圆。
        /// </summary>
        EmptyCircle,
        /// <summary>
        /// 正方形。可通过设置`itemStyle`的`cornerRadius`变成圆角矩形。
        /// </summary>
        Rect,
        /// <summary>
        /// 空心正方形。
        /// </summary>
        EmptyRect,
        /// <summary>
        /// 三角形。
        /// </summary>
        Triangle,
        /// <summary>
        /// 空心三角形。
        /// </summary>
        EmptyTriangle,
        /// <summary>
        /// 菱形。
        /// </summary>
        Diamond,
        /// <summary>
        /// 空心菱形。
        /// </summary>
        EmptyDiamond,
        /// <summary>
        /// 箭头。
        /// </summary>
        Arrow,
        /// <summary>
        /// 空心箭头。
        /// </summary>
        EmptyArrow,
        /// <summary>
        /// 加号。
        /// </summary>
        Plus,
        /// <summary>
        /// 减号。
        /// </summary>
        Minus,
    }

    /// <summary>
    /// 系列数据项的标记的图形
    /// </summary>
    [System.Serializable]
    public class SymbolStyle : ChildComponent
    {
        [SerializeField] protected bool m_Show = true;
        [SerializeField] protected SymbolType m_Type = SymbolType.EmptyCircle;
        [SerializeField] protected float m_Size = 0f;
        [SerializeField] protected float m_Gap = 0;
        [SerializeField] protected float m_Width = 0f;
        [SerializeField] protected float m_Height = 0f;
        [SerializeField] protected Vector2 m_Offset = Vector2.zero;
        [SerializeField] protected Sprite m_Image;
        [SerializeField] protected Image.Type m_ImageType;
        [SerializeField] protected Color32 m_Color;

        public virtual void Reset()
        {
            m_Show = false;
            m_Type = SymbolType.EmptyCircle;
            m_Size = 0f;
            m_Gap = 0;
            m_Width = 0f;
            m_Height = 0f;
            m_Offset = Vector2.zero;
            m_Image = null;
            m_ImageType = Image.Type.Simple;
        }

        /// <summary>
        /// Whether the symbol is showed.
        /// ||是否显示标记。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the type of symbol.
        /// ||标记类型。
        /// </summary>
        public SymbolType type
        {
            get { return m_Type; }
            set { if (PropertyUtil.SetStruct(ref m_Type, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the size of symbol.
        /// ||标记的大小。
        /// </summary>
        public float size
        {
            get { return m_Size; }
            set { if (PropertyUtil.SetStruct(ref m_Size, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the gap of symbol and line segment.
        /// ||图形标记和线条的间隙距离。
        /// </summary>
        public float gap
        {
            get { return m_Gap; }
            set { if (PropertyUtil.SetStruct(ref m_Gap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 图形的宽。
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtil.SetStruct(ref m_Width, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 图形的高。
        /// </summary>
        public float height
        {
            get { return m_Height; }
            set { if (PropertyUtil.SetStruct(ref m_Height, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 自定义的标记图形。
        /// </summary>
        public Sprite image
        {
            get { return m_Image; }
            set { if (PropertyUtil.SetClass(ref m_Image, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the fill type of image.
        /// ||图形填充类型。
        /// </summary>
        public Image.Type imageType
        {
            get { return m_ImageType; }
            set { if (PropertyUtil.SetStruct(ref m_ImageType, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 图形的偏移。
        /// </summary>
        public Vector2 offset
        {
            get { return m_Offset; }
            set { if (PropertyUtil.SetStruct(ref m_Offset, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 图形的颜色。
        /// </summary>
        public Color32 color
        {
            get { return m_Color; }
            set { if (PropertyUtil.SetStruct(ref m_Color, value)) SetAllDirty(); }
        }
        public Vector3 offset3 { get { return new Vector3(m_Offset.x, m_Offset.y, 0); } }
        private List<float> m_AnimationSize = new List<float>() { 0, 5, 10 };
        /// <summary>
        /// the setting for effect scatter.
        /// ||带有涟漪特效动画的散点图的动画参数。
        /// </summary>
        public List<float> animationSize { get { return m_AnimationSize; } }

        public Color32 GetColor(Color32 defaultColor)
        {
            return ChartHelper.IsClearColor(m_Color) ? defaultColor : m_Color;
        }
    }
}