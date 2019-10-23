/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;

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