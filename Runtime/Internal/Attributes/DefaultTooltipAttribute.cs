using System;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DefaultTooltipAttribute : Attribute
    {
        public readonly Tooltip.Type type;
        public readonly Tooltip.Trigger trigger;

        public DefaultTooltipAttribute(Tooltip.Type type, Tooltip.Trigger trigger)
        {
            this.type = type;
            this.trigger = trigger;
        }
    }
}