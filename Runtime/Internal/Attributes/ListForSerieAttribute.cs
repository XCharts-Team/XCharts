using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ListForSerie : ListFor
    {
        public ListForSerie(Type type) : base(type)
        { }
    }
}