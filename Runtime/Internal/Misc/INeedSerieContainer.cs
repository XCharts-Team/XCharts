
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    public interface INeedSerieContainer
    {
        int containerIndex { get; }
        int containterInstanceId { get; }
    }
}