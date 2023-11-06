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

        #region Overrides
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if(startPort == port)
                {
                    return;
                }

                if(startPort.node == port.node)
                {
                    return;
                }

                if(startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);

            });

            return compatiblePorts;
        }

        #endregion

        #region Manipulators
        void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", typeof(VWSingleChoiceNode)));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", typeof(VWMultipleChoiceNode)));
            this.AddManipulator(CreateGroupContextualMenu());
        }

        IManipulator CreateNodeContextualMenu(string actionTitle, Type dialogType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogType, actionEvent.eventInfo.localMousePosition)))
                );
            return contextualMenuManipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogGroup", actionEvent.eventInfo.localMousePosition)))
                );
            return contextualMenuManipulator;
        }

        #endregion

        #region Element Creation

        private Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }

        VWNode CreateNode(Type dialogType, Vector2 position)
        {
            VWNode node = (VWNode)Activator.CreateInstance(dialogType);

            node.Initialize(position);
            node.Draw();

            return node;
        }

        #endregion

        #region Element Additions
        void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "Dialog/VWGraphViewStyles.uss",
                "Dialog/VWNodeStyles.uss"
            );
        }
        #endregion
    }
}

