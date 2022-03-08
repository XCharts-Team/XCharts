using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace XUGL
{
    public enum SVGPathSegType
    {
        /// <summary>
        /// move to
        /// </summary>
        M,
        /// <summary>
        /// line to
        /// </summary>
        L,
        /// <summary>
        /// horizontal line to
        /// </summary>
        H,
        /// <summary>
        /// vertial line to
        /// </summary>
        V,
        /// <summary>
        /// curve to
        /// </summary>
        C,
        /// <summary>
        /// smooth curve to
        /// </summary>
        S,
        /// <summary>
        /// quadratic bezier curve
        /// </summary>
        Q,
        /// <summary>
        /// smooth quadratic bezier curve to
        /// </summary>
        T,
        /// <summary>
        /// elliptical Arc
        /// </summary>
        A,
        /// <summary>
        /// close path
        /// </summary>
        Z
    }
}