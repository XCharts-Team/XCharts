
using UnityEngine;

namespace XCharts
{
    public class ChartObject
    {
        protected GameObject m_GameObject;

        public virtual void Destroy()
        {
            GameObject.Destroy(m_GameObject);
        }
    }
}