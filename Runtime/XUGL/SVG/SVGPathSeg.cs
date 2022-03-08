using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace XUGL
{
    public class SVGPathSeg
    {
        public SVGPathSegType type;
        public bool relative;
        public List<float> parameters = new List<float>();
        public string raw;

        public SVGPathSeg(SVGPathSegType type)
        {
            this.type = type;
        }

        public float value
        {
            get
            {
                if (type == SVGPathSegType.H)
                    return SVG.yMirror ? -parameters[0] : parameters[0];
                else
                    return parameters[0];
            }
        }
        public float x { get { return parameters[0]; } }
        public float y { get { return SVG.yMirror ? -parameters[1] : parameters[1]; } }
        public Vector2 p1 { get { return new Vector2(parameters[0], (SVG.yMirror ? -parameters[1] : parameters[1])); } }
        public Vector2 p2 { get { return new Vector2(parameters[2], (SVG.yMirror ? -parameters[3] : parameters[3])); } }
        public Vector2 p3 { get { return new Vector2(parameters[4], (SVG.yMirror ? -parameters[5] : parameters[5])); } }
    }
}