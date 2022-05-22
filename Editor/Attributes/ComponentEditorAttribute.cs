using System;

namespace XCharts.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ComponentEditorAttribute : Attribute
    {
        public readonly Type componentType;

        public ComponentEditorAttribute(Type componentType)
        {
            this.componentType = componentType;
        }
    }
}