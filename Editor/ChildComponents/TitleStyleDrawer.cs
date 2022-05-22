using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomPropertyDrawer(typeof(TitleStyle), true)]
    public class TitleStyleDrawer : LabelStyleDrawer
    {
        public override string ClassName { get { return "TitleStyle"; } }
    }
}