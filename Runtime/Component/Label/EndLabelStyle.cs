using System;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    public class EndLabelStyle : LabelStyle
    {
        public EndLabelStyle()
        {
            m_Offset = new Vector3(5, 0, 0);
            m_TextStyle.alignment = TextAnchor.MiddleLeft;
            m_NumericFormatter = "f0";
            m_Formatter = "{a}:{c}";
        }
    }
}