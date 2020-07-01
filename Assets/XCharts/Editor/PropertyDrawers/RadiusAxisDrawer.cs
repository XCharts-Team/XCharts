/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(RadiusAxis), true)]
    public class RadiusAxisDrawer : AxisDrawer
    {
        protected override string GetDisplayName(string displayName)
        {
            if (displayName.StartsWith("Element"))
            {
                displayName = displayName.Replace("Element", "Radius Axis");
            }
            return displayName;
        }
    }
}