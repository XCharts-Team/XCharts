using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(YAxis), true)]
    public class YAxisDrawer : AxisDrawer
    {
        protected override string GetDisplayName(string displayName)
        {
            if (displayName.StartsWith("Element"))
            {
                displayName = displayName.Replace("Element", "Y Axis");
            }
            return displayName;
        }
    }
}