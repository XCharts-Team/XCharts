/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
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