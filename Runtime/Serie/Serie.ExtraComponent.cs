using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XCharts.Runtime
{
    public partial class Serie
    {
        public static Dictionary<Type, string> extraComponentMap = new Dictionary<Type, string>
        { { typeof(LabelStyle), "m_Labels" },
            { typeof(LabelLine), "m_LabelLines" },
            { typeof(EndLabelStyle), "m_EndLabels" },
            { typeof(LineArrow), "m_LineArrows" },
            { typeof(AreaStyle), "m_AreaStyles" },
            { typeof(TitleStyle), "m_TitleStyles" },
            { typeof(EmphasisItemStyle), "m_EmphasisItemStyles" },
            { typeof(EmphasisLabelStyle), "m_EmphasisLabels" },
            { typeof(EmphasisLabelLine), "m_EmphasisLabelLines" },
        };

        [SerializeField][IgnoreDoc] private List<LabelStyle> m_Labels = new List<LabelStyle>();
        [SerializeField][IgnoreDoc] private List<LabelLine> m_LabelLines = new List<LabelLine>();
        [SerializeField][IgnoreDoc] private List<EndLabelStyle> m_EndLabels = new List<EndLabelStyle>();
        [SerializeField][IgnoreDoc] private List<LineArrow> m_LineArrows = new List<LineArrow>();
        [SerializeField][IgnoreDoc] private List<AreaStyle> m_AreaStyles = new List<AreaStyle>();
        [SerializeField][IgnoreDoc] private List<TitleStyle> m_TitleStyles = new List<TitleStyle>();
        [SerializeField][IgnoreDoc] private List<EmphasisItemStyle> m_EmphasisItemStyles = new List<EmphasisItemStyle>();
        [SerializeField][IgnoreDoc] private List<EmphasisLabelStyle> m_EmphasisLabels = new List<EmphasisLabelStyle>();
        [SerializeField][IgnoreDoc] private List<EmphasisLabelLine> m_EmphasisLabelLines = new List<EmphasisLabelLine>();

        /// <summary>
        /// The style of area.
        /// |区域填充样式。
        /// </summary>
        public AreaStyle areaStyle { get { return m_AreaStyles.Count > 0 ? m_AreaStyles[0] : null; } }
        /// <summary>
        /// Text label of graphic element,to explain some data information about graphic item like value, name and so on.
        /// |图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
        /// </summary>
        public LabelStyle label { get { return m_Labels.Count > 0 ? m_Labels[0] : null; } }
        public LabelStyle endLabel { get { return m_EndLabels.Count > 0 ? m_EndLabels[0] : null; } }
        /// <summary>
        /// The line of label.
        /// |标签上的视觉引导线。
        /// </summary>
        public LabelLine labelLine { get { return m_LabelLines.Count > 0 ? m_LabelLines[0] : null; } }
        /// <summary>
        /// The arrow of line.
        /// |折线图的箭头。
        /// </summary>
        public LineArrow lineArrow { get { return m_LineArrows.Count > 0 ? m_LineArrows[0] : null; } }
        /// <summary>
        /// 高亮的图形样式
        /// </summary>
        public EmphasisItemStyle emphasisItemStyle { get { return m_EmphasisItemStyles.Count > 0 ? m_EmphasisItemStyles[0] : null; } }
        /// <summary>
        /// 高亮时的标签样式
        /// </summary>
        public EmphasisLabelStyle emphasisLabel { get { return m_EmphasisLabels.Count > 0 ? m_EmphasisLabels[0] : null; } }
        /// <summary>
        /// 高亮时的标签引导线样式
        /// </summary>
        public EmphasisLabelLine emphasisLabelLine { get { return m_EmphasisLabelLines.Count > 0 ? m_EmphasisLabelLines[0] : null; } }
        /// <summary>
        /// the icon of data.
        /// |数据项标题样式。
        /// </summary>
        public TitleStyle titleStyle { get { return m_TitleStyles.Count > 0 ? m_TitleStyles[0] : null; } }

        public void RemoveAllExtraComponent()
        {
            var serieType = GetType();
            foreach (var kv in extraComponentMap)
            {
                ReflectionUtil.InvokeListClear(this, serieType.GetField(kv.Value));
            }
            SetAllDirty();
        }

        public T AddExtraComponent<T>() where T : ChildComponent, ISerieExtraComponent
        {
            return AddExtraComponent(typeof(T)) as T;
        }

        public ISerieExtraComponent AddExtraComponent(Type type)
        {
            if (GetType().IsDefined(typeof(SerieExtraComponentAttribute), false))
            {
                var attr = GetType().GetAttribute<SerieExtraComponentAttribute>();
                if (attr.Contains(type))
                {
                    var fieldName = string.Empty;
                    if (extraComponentMap.TryGetValue(type, out fieldName))
                    {
                        var field = typeof(Serie).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                        if (ReflectionUtil.InvokeListCount(this, field) <= 0)
                        {
                            var extraComponent = Activator.CreateInstance(type) as ISerieExtraComponent;
                            ReflectionUtil.InvokeListAdd(this, field, extraComponent);
                            SetAllDirty();
                            return extraComponent;
                        }
                        else
                        {
                            return ReflectionUtil.InvokeListGet<ISerieExtraComponent>(this, field, 0);
                        }
                    }
                }
            }
            throw new System.Exception(string.Format("Serie {0} not support extra component: {1}",
                GetType().Name, type.Name));
        }

        public void RemoveExtraComponent<T>() where T : ISerieExtraComponent
        {
            RemoveExtraComponent(typeof(T));
        }

        public void RemoveExtraComponent(Type type)
        {
            if (GetType().IsDefined(typeof(SerieExtraComponentAttribute), false))
            {
                var attr = GetType().GetAttribute<SerieExtraComponentAttribute>();
                if (attr.Contains(type))
                {
                    var fieldName = string.Empty;
                    if (extraComponentMap.TryGetValue(type, out fieldName))
                    {
                        var field = typeof(Serie).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                        ReflectionUtil.InvokeListClear(this, field);
                        SetAllDirty();
                        return;
                    }
                }
            }
            throw new System.Exception(string.Format("Serie {0} not support extra component: {1}",
                GetType().Name, type.Name));
        }

        private void RemoveExtraComponentList<T>(List<T> list) where T : ISerieExtraComponent
        {
            if (list.Count > 0)
            {
                list.Clear();
                SetAllDirty();
            }
        }
    }
}