
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;

namespace XCharts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SerieEditorAttribute : Attribute
    {
        public readonly Type serieType;

        public SerieEditorAttribute(Type serieType)
        {
            this.serieType = serieType;
        }
    }
}