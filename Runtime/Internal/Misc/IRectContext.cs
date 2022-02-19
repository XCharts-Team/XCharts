
using UnityEngine;

namespace XCharts.Runtime
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