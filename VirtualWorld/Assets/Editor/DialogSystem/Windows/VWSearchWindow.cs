using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialog
{
    enum NodeType
    {
        SingleChoice,
        MultipleChoice
    }
    public class VWSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private VWGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(VWGraphView vwGraphView)
        {
            graphView = vwGraphView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialog Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", indentationIcon))
                {
                    level = 2,
                    userData = NodeType.SingleChoice,
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", indentationIcon))
                {
                    level = 2,
                    userData = NodeType.MultipleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Dialog Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (SearchTreeEntry.userData)
            {
                case NodeType.SingleChoice:
                    VWSingleChoiceNode singleChoiceNode = 
                        (VWSingleChoiceNode)graphView.CreateNode(typeof(VWSingleChoiceNode), localMousePosition);
                    graphView.AddElement(singleChoiceNode);
                    return true;
                case NodeType.MultipleChoice:
                    VWMultipleChoiceNode multipleChoiceNode = 
                        (VWMultipleChoiceNode)graphView.CreateNode(typeof(VWMultipleChoiceNode), localMousePosition);
                    graphView.AddElement(multipleChoiceNode);
                    return true;
                case Group _:
                    Group group = graphView.CreateGroup("DialogGroup", localMousePosition);
                    graphView.AddElement(group);
                    return true;
                default:
                    return false;
            }
        }
    }
}

