using System;

namespace XCharts
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ListForComponent : ListFor
    {
        public ListForComponent(Type type) : base(type)
        {
        }
    }
}