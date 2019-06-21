using UnityEngine;
using System;

namespace XCharts
{
    public class JsonDataSupport : IJsonData, ISerializationCallbackReceiver
    {
        [SerializeField] protected string m_JsonData;
        [SerializeField] protected bool m_DataFromJson;

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
