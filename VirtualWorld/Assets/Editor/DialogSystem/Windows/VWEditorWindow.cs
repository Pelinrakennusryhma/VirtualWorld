using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialog
{
    public class VWEditorWindow : EditorWindow
    {
        [MenuItem("Window/Dialog/Dialog Graph")]
        public static void ShowExample()
        {
            GetWindow<VWEditorWindow>("Dialog Graph");
        }

        private void CreateGUI()
        {
            AddGraphView();

            AddStyles();
        }

        private void AddGraphView()
        {
            VWGraphView graphView = new VWGraphView();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Dialog/VWVariables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}