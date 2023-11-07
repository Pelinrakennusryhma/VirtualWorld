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

        #region Element Additions
        private void AddGraphView()
        {
            VWGraphView graphView = new VWGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Dialog/VWVariables.uss");
        }
        #endregion
    }
}