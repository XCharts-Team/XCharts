/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace XCharts
{
    public class XChartsBuild : IPreprocessBuild, IPostprocessBuild
    {
        public int callbackOrder => 1;

        public void OnPostprocessBuild(BuildTarget target, string path)
        {
        }

        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            XThemeMgr.ExportAllCustomTheme();
        }
    }
}