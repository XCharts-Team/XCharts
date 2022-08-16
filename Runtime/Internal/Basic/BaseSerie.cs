using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    public abstract class BaseSerie
    {
        public virtual bool vertsDirty { get { return m_VertsDirty; } }
        public virtual bool componentDirty { get { return m_ComponentDirty; } }
        
        public virtual SerieColorBy defaultColorBy { get { return SerieColorBy.Serie; } }
        public virtual bool titleJustForSerie { get { return false; } }
        public virtual bool useSortData { get { return false; } }
        public virtual bool multiDimensionLabel { get { return false; } }
        public bool anyDirty { get { return vertsDirty || componentDirty; } }
        public Painter painter { get { return m_Painter; } set { m_Painter = value; } }
        public Action refreshComponent { get; set; }
        public GameObject gameObject { get; set; }

        [NonSerialized] protected bool m_VertsDirty;
        [NonSerialized] protected bool m_ComponentDirty;
        [NonSerialized] protected Painter m_Painter;
        [NonSerialized] public SerieContext context = new SerieContext();
        [NonSerialized] public InteractData interact = new InteractData();

        public SerieHandler handler { get; set; }

        

        public static void ClearVerticesDirty(ChildComponent component)
        {
            if (component != null)
                component.ClearVerticesDirty();
        }

        public static void ClearComponentDirty(ChildComponent component)
        {
            if (component != null)
                component.ClearComponentDirty();
        }

        public static bool IsVertsDirty(ChildComponent component)
        {
            return component == null?false : component.vertsDirty;
        }

        public static bool IsComponentDirty(ChildComponent component)
        {
            return component == null?false : component.componentDirty;
        }

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

        public virtual void OnRemove()
        {
            if (handler != null)
                handler.RemoveComponent();
        }

        public virtual void OnDataUpdate() { }

        public virtual void OnBeforeSerialize() { }

        public virtual void OnAfterDeserialize()
        {
            OnDataUpdate();
        }

        public void RefreshLabel()
        {
            if (handler != null)
                handler.RefreshLabelNextFrame();
        }
    }
}