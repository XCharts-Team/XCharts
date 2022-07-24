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
            { typeof(EmphasisStyle), "m_EmphasisStyles" },
            { typeof(BlurStyle), "m_BlurStyles" },
            { typeof(SelectStyle), "m_SelectStyles" },
        };

        [SerializeField][IgnoreDoc] private List<LabelStyle> m_Labels = new List<LabelStyle>();
        [SerializeField][IgnoreDoc] private List<LabelLine> m_LabelLines = new List<LabelLine>();
        [SerializeField][IgnoreDoc] private List<EndLabelStyle> m_EndLabels = new List<EndLabelStyle>();
        [SerializeField][IgnoreDoc] private List<LineArrow> m_LineArrows = new List<LineArrow>();
        [SerializeField][IgnoreDoc] private List<AreaStyle> m_AreaStyles = new List<AreaStyle>();
        [SerializeField][IgnoreDoc] private List<TitleStyle> m_TitleStyles = new List<TitleStyle>();
        [SerializeField][IgnoreDoc] private List<EmphasisStyle> m_EmphasisStyles = new List<EmphasisStyle>();
        [SerializeField][IgnoreDoc] private List<BlurStyle> m_BlurStyles = new List<BlurStyle>();
        [SerializeField][IgnoreDoc] private List<SelectStyle> m_SelectStyles = new List<SelectStyle>();

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
        /// the icon of data.
        /// |数据项标题样式。
        /// </summary>
        public TitleStyle titleStyle { get { return m_TitleStyles.Count > 0 ? m_TitleStyles[0] : null; } }
        /// <summary>
        /// style of emphasis state.
        /// |高亮状态的样式。
        /// </summary>
        public EmphasisStyle emphasisStyle { get { return m_EmphasisStyles.Count > 0 ? m_EmphasisStyles[0] : null; } }
        /// <summary>
        /// style of blur state.
        /// |淡出状态的样式。
        /// </summary>
        public BlurStyle blurStyle { get { return m_BlurStyles.Count > 0 ? m_BlurStyles[0] : null; } }
        /// <summary>
        /// style of select state.
        /// |选中状态的样式。
        /// </summary>
        public SelectStyle selectStyle { get { return m_SelectStyles.Count > 0 ? m_SelectStyles[0] : null; } }

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