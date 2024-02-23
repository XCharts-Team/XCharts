using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ComponentHandlerAttribute : Attribute
    {
        public readonly Type handler;
        public readonly bool allowMultiple = true;
        public readonly int order = 3;

        public ComponentHandlerAttribute(Type handler, int order = 3)
        {
            this.handler = handler;
            this.allowMultiple = true;
            this.order = order;
        }

        public ComponentHandlerAttribute(Type handler, bool allowMultiple, int order = 3)
        {
            this.handler = handler;
            this.allowMultiple = allowMultiple;
            this.order = order;
        }
    }
}