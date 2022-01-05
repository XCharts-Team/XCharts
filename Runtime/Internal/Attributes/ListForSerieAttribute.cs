using System;

namespace XCharts
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ListForSerie : ListFor
    {
        public ListForSerie(Type type) : base(type)
        {
        }
    }
}