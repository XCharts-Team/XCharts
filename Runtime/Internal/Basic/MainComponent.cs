using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [System.Serializable]
    public class MainComponent : IComparable
    {
        public int instanceId { get { return index; } }
        public int index { get; internal set; }
        protected bool m_VertsDirty;
        protected bool m_ComponentDirty;
        protected Painter m_Painter;

        /// <summary>
        /// 图表重绘标记。
        /// </summary>
        public virtual bool vertsDirty { get { return m_VertsDirty; } }
        /// <summary>
        /// 组件重新初始化标记。
        /// </summary>
        public virtual bool componentDirty { get { return m_ComponentDirty; } }
        /// <summary>
        /// 需要重绘图表或重新初始化组件。
        /// </summary>
        public bool anyDirty { get { return vertsDirty || componentDirty; } }
        public Painter painter { get { return m_Painter; } set { m_Painter = value; } }
        public Action refreshComponent { get; set; }
        public GameObject gameObject { get; set; }
        internal MainComponentHandler handler { get; set; }

        public virtual void SetVerticesDirty()
        {
            m_VertsDirty = true;
        }

        public virtual void ClearVerticesDirty()
        {
            m_VertsDirty = false;
        }

        public virtual void SetComponentDirty()
        {
            m_ComponentDirty = true;
        }

        public virtual void ClearComponentDirty()
        {
            m_ComponentDirty = false;
        }

        public virtual void Reset() { }

        public virtual void ClearData() { }

        public virtual void ClearDirty()
        {
            ClearVerticesDirty();
            ClearComponentDirty();
        }

        public virtual void SetAllDirty()
        {
            SetVerticesDirty();
            SetComponentDirty();
        }

        public virtual void SetDefaultValue() { }

        public virtual void OnRemove()
        {
            if (handler != null)
                handler.RemoveComponent();
        }

        public int CompareTo(object obj)
        {
            var flag = GetType().Name.CompareTo(obj.GetType().Name);
            if (flag == 0)
                return index.CompareTo((obj as MainComponent).index);
            else
                return flag;
        }
    }

    public abstract class MainComponentHandler
    {
        public BaseChart chart { get; internal set; }
        public ComponentHandlerAttribute attribute { get; internal set; }

        public virtual void InitComponent() { }
        public virtual void RemoveComponent() { }
        public virtual void CheckComponent(StringBuilder sb) { }
        public virtual void BeforceSerieUpdate() { }
        public virtual void Update() { }
        public virtual void DrawBase(VertexHelper vh) { }
        public virtual void DrawUpper(VertexHelper vh) { }
        public virtual void DrawTop(VertexHelper vh) { }
        public virtual void OnSerieDataUpdate(int serieIndex) { }
        public virtual void OnPointerClick(PointerEventData eventData) { }
        public virtual void OnPointerDown(PointerEventData eventData) { }
        public virtual void OnPointerUp(PointerEventData eventData) { }
        public virtual void OnPointerEnter(PointerEventData eventData) { }
        public virtual void OnPointerExit(PointerEventData eventData) { }
        public virtual void OnDrag(PointerEventData eventData) { }
        public virtual void OnBeginDrag(PointerEventData eventData) { }
        public virtual void OnEndDrag(PointerEventData eventData) { }
        public virtual void OnScroll(PointerEventData eventData) { }
        internal abstract void SetComponent(MainComponent component);
    }

    public abstract class MainComponentHandler<T> : MainComponentHandler
    where T : MainComponent
    {
        public T component { get; internal set; }

        internal override void SetComponent(MainComponent component)
        {
            this.component = (T) component;
        }
    }
}