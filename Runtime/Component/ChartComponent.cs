/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    public class ChartComponent
    {
        [NonSerialized] protected bool m_VertsDirty;
        [NonSerialized] protected bool m_ComponentDirty;
        [NonSerialized] protected Painter m_Painter;
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

        internal virtual void SetVerticesDirty()
        {
            m_VertsDirty = true;
        }

        internal virtual void ClearVerticesDirty()
        {
            m_VertsDirty = false;
        }

        internal virtual void SetComponentDirty()
        {
            m_ComponentDirty = true;
        }

        internal virtual void ClearComponentDirty()
        {
            m_ComponentDirty = false;
        }

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
    }

    public class MainComponent : ChartComponent
    {
    }

    public class SubComponent : ChartComponent
    {
    }
}