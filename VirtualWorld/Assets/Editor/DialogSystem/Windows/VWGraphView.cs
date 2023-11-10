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
        private VWEditorWindow editorWindow;
        private VWSearchWindow searchWindow;

        private SerializableDictionary<string, VWNodeErrorData> ungroupedNodes;
        private SerializableDictionary<Group, SerializableDictionary<string, VWNodeErrorData>> groupedNodes;
        public VWGraphView(VWEditorWindow vwEditorWindow)
        {
            editorWindow = vwEditorWindow;

            ungroupedNodes = new SerializableDictionary<string, VWNodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, VWNodeErrorData>>();

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();

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
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => 
                AddElement(CreateNode(dialogType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
                );
            return contextualMenuManipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => 
                AddElement(CreateGroup("DialogGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
                );
            return contextualMenuManipulator;
        }

        #endregion

        #region Element Creation

        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<VWSearchWindow>();
                searchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        public Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }

        public VWNode CreateNode(Type dialogType, Vector2 position)
        {
            VWNode node = (VWNode)Activator.CreateInstance(dialogType);

            node.Initialize(this, position);
            node.Draw();

            AddUngroupedNode(node);

            return node;
        }
        #endregion
        #region Callbacks
        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                for (int i = selection.Count - 1; i >= 0; i--)
                {
                    if (selection[i] is VWNode node)
                    {
                        if(node.Group != null)
                        {
                            node.Group.RemoveElement(node);
                        }

                        RemoveUngroupedNode(node);
                        RemoveElement(node);
                    }
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is VWNode))
                    {
                        continue;
                    }

                    VWNode node = (VWNode)element;
                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, group);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is VWNode))
                    {
                        continue;
                    }

                    VWNode node = (VWNode)element;

                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);
                }
            };
        }

        #endregion
        #region Repeated Elements
        public void AddUngroupedNode(VWNode node)
        {
            string nodeName = node.DialogName;

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                VWNodeErrorData nodeErrorData = new VWNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                ungroupedNodes.Add(nodeName, nodeErrorData);
            } else
            {
                List<VWNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
                ungroupedNodesList.Add(node);

                Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
                node.SetErrorStyle(errorColor);

                if (ungroupedNodesList.Count > 1)
                {
                    ungroupedNodesList[0].SetErrorStyle(errorColor);
                }
            }
        }

        public void RemoveUngroupedNode(VWNode node)
        {
            string nodeName = node.DialogName;
            List<VWNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);
            node.ResetStyle();

            if(ungroupedNodesList.Count == 1)
            {
                ungroupedNodesList[0].ResetStyle();
            } 
            else if(ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }
        }


        public void AddGroupedNode(VWNode node, Group group)
        {
            string nodeName = node.DialogName;

            node.Group = group;

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, VWNodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                VWNodeErrorData nodeErrorData = new VWNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, nodeErrorData);
                return;
            }

            List<VWNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Add(node);

            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if(groupedNodesList.Count == 2)
            {
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(VWNode node, Group group)
        {
            string nodeName = node.DialogName;

            node.Group = null;

List <VWNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Remove(node);
            node.ResetStyle();

            if(groupedNodesList.Count == 1)
            {
                groupedNodesList[0].ResetStyle();
                return;
            }

            if(groupedNodesList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);

                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
            }
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

        #region Utilities
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }
        #endregion
    }
}

