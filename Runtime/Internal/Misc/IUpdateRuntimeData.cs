using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    public interface IUpdateRuntimeData
    {
        void UpdateRuntimeData(BaseChart chart);
    }
}