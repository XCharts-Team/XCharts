using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class Since : Attribute
    {
        public readonly string version;

        public Since(string version)
        {
            this.version = version;
        }
    }
}