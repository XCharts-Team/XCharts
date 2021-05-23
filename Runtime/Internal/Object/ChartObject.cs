/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

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