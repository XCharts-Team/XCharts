/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using UnityEngine;

namespace XCharts
{
    public class ChartComponent
    {
        [SerializeField] protected string m_JsonData;
        [SerializeField] protected bool m_DataFromJson;

        [NonSerialized] protected bool m_VertsDirty;
        [NonSerialized] protected bool m_ComponentDirty;

        /// <summary>
        /// json格式的字符串数据
        /// </summary>
        /// <returns></returns>
        public string jsonData { get { return m_JsonData; } set { m_JsonData = value; ParseJsonData(value); } }
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
        internal void OnAfterDeserialize()
        {
            if (m_DataFromJson)
            {
                ParseJsonData(m_JsonData);
                m_DataFromJson = false;
            }
        }

        internal void OnBeforeSerialize()
        {
        }

        public virtual void ParseJsonData(string json)
        {
            throw new Exception("no support yet");
        }

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
}