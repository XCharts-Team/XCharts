using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class SerieHandlerAttribute : Attribute
    {
        public readonly Type handler;
        public readonly bool allowMultiple = true;

        public SerieHandlerAttribute(Type handler)
        {
            this.handler = handler;
            this.allowMultiple = true;
        }
        public SerieHandlerAttribute(Type handler, bool allowMultiple)
        {
            this.handler = handler;
            this.allowMultiple = allowMultiple;
        }
    }
}