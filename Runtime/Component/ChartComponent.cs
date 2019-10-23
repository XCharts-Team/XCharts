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

        /// <summary>
        /// json格式的字符串数据
        /// </summary>
        /// <returns></returns>
        public string jsonData { get { return m_JsonData; } set { m_JsonData = value; ParseJsonData(value); } }

        public void OnAfterDeserialize()
        {
            if (m_DataFromJson)
            {
                ParseJsonData(m_JsonData);
                m_DataFromJson = false;
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public virtual void ParseJsonData(string json)
        {
            throw new Exception("no support yet");
        }
    }
}