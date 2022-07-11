using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Settings related to text.
    /// |文本的内边距设置。
    /// </summary>
    [Serializable]
    public class TextPadding : Padding
    {
        public TextPadding() { }

        public TextPadding(float top, float right, float bottom, float left)
        {
            SetPadding(top, right, bottom, left);
        }
    }
}