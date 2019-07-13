using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(XAxis), true)]
    public class XAxisDrawer : AxisDrawer
    {
        protected override string GetDisplayName(string displayName)
        {
            if (displayName.StartsWith("Element"))
            {
                displayName = displayName.Replace("Element", "X Axis");
            }
            return displayName;
        }
    }
}