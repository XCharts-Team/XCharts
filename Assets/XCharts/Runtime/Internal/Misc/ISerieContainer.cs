/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    public interface ISerieContainer
    {
        //bool runtimeIsPointerEnter { get; }
        int index { get; }
        bool IsPointerEnter();
    }
}