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
        }

        private void AddGraphView()
        {
            VWGraphView graphView = new VWGraphView();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
    }
}