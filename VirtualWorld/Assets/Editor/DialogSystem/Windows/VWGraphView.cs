using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialog
{
    public class VWGraphView : GraphView
    {
        public VWGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
        }

        void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", typeof(VWSingleChoiceNode)));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", typeof(VWMultipleChoiceNode)));
        }

        IManipulator CreateNodeContextualMenu(string actionTitle, Type dialogType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogType, actionEvent.eventInfo.localMousePosition)))
                );
            return contextualMenuManipulator;
        }

        VWNode CreateNode(Type dialogType, Vector2 position)
        {
            VWNode node = (VWNode)Activator.CreateInstance(dialogType);

            node.Initialize(position);
            node.Draw();

            return node;
        }

        void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            StyleSheet graphViewStyleSheet = (StyleSheet)EditorGUIUtility.Load("Dialog/VWGraphViewStyles.uss");
            StyleSheet nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("Dialog/VWNodeStyles.uss");

            styleSheets.Add(graphViewStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }
    }
}

