
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    public interface IUpdateRuntimeData
    {
        void UpdateRuntimeData(float chartX, float chartY, float chartWidth, float chartHeight);
    }
}