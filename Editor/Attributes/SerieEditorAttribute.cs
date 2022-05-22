using System;

namespace XCharts.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SerieEditorAttribute : Attribute
    {
        public readonly Type serieType;

        public SerieEditorAttribute(Type serieType)
        {
            this.serieType = serieType;
        }
    }
}