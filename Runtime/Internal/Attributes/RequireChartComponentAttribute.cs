using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequireChartComponentAttribute : Attribute
    {
        public readonly Type type0;
        public readonly Type type1;
        public readonly Type type2;

        public RequireChartComponentAttribute(Type requiredComponent)
        {
            type0 = requiredComponent;
        }
        public RequireChartComponentAttribute(Type requiredComponent, Type requiredComponent2)
        {
            type0 = requiredComponent;
            type1 = requiredComponent2;
        }
        public RequireChartComponentAttribute(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)
        {
            type0 = requiredComponent;
            type1 = requiredComponent2;
            type2 = requiredComponent3;
        }
    }
}