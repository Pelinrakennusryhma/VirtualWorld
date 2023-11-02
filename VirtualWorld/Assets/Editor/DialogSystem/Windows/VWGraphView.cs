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
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", VWDialogType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", VWDialogType.MultipleChoice));
        }

        IManipulator CreateNodeContextualMenu(string actionTitle, VWDialogType dialogType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogType, actionEvent.eventInfo.localMousePosition)))
                );
            return contextualMenuManipulator;
        }

        VWNode CreateNode(VWDialogType dialogType, Vector2 position)
        {
            Type nodeType = Type.GetType($"Dialog.VW{dialogType}Node");
            VWNode node = (VWNode)Activator.CreateInstance(nodeType);

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
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Dialog/VWGraphViewStyles.uss");

            styleSheets.Add(styleSheet);
        }
    }
}

