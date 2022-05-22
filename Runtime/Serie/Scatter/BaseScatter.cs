namespace XCharts.Runtime
{
    [System.Serializable]
    public class BaseScatter : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }
    }
}