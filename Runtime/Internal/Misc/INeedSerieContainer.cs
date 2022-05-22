using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    public interface INeedSerieContainer
    {
        int containerIndex { get; }
        int containterInstanceId { get; }
    }
}