using UnityEditor;
using UnityEngine;

namespace XCharts.Editor
{
    public class EditorCustomStyles
    {
        static readonly Color splitterDark = new Color(0.12f, 0.12f, 0.12f, 0.5f);
        static readonly Color splitterLight = new Color(0.6f, 0.6f, 0.6f, 0.5f);
        static readonly Texture2D paneOptionsIconDark = (Texture2D) EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/pane options.png");
        static readonly Texture2D paneOptionsIconLight = (Texture2D) EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/pane options.png");
        static readonly Color headerBackgroundDark = new Color(0.1f, 0.1f, 0.1f, 0.2f);
        static readonly Color headerBackgroundLight = new Color(1f, 1f, 1f, 0.2f);

        public static readonly GUIStyle headerStyle = UnityEditor.EditorStyles.boldLabel;
        public static readonly GUIStyle foldoutStyle = new GUIStyle(UnityEditor.EditorStyles.foldout)
        {
            font = headerStyle.font,
            fontStyle = headerStyle.fontStyle,
        };
        public static readonly GUIContent iconAdd = new GUIContent("+", "Add");
        public static readonly GUIContent iconRemove = new GUIContent("-", "Remove");
        public static readonly GUIContent iconUp = new GUIContent("↑", "Up");
        public static readonly GUIContent iconDown = new GUIContent("↓", "Down");
        public static readonly GUIStyle invisibleButton = "InvisibleButton";
        public static readonly GUIStyle smallTickbox = new GUIStyle("ShurikenToggle");

        public static Color splitter { get { return EditorGUIUtility.isProSkin ? splitterDark : splitterLight; } }
        public static Texture2D paneOptionsIcon { get { return EditorGUIUtility.isProSkin ? paneOptionsIconDark : paneOptionsIconLight; } }
        public static Color headerBackground { get { return EditorGUIUtility.isProSkin ? headerBackgroundDark : headerBackgroundLight; } }
    }
}