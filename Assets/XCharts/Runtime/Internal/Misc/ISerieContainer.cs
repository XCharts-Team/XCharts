
namespace XCharts
{
    public interface ISerieContainer
    {
        //bool runtimeIsPointerEnter { get; }
        int index { get; }
        bool IsPointerEnter();
    }
}