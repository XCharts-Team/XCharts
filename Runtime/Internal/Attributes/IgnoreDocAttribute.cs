using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IgnoreDoc : Attribute
    {
        public IgnoreDoc()
        {
        }
    }
}