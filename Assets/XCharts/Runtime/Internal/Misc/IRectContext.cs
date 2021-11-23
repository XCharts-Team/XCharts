/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    public interface IRectContext
    {
        float x { get; }
        float y { get; }
        float width { get; }
        float height { get; }
        Vector3 position { get; }
    }
}