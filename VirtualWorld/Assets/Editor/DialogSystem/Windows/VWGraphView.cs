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
            CreateNode();
            AddStyles();
        }

        void CreateNode()
        {
            VWNode node = new VWNode();
            node.Initialize();
            node.Draw();
            AddElement(node);
        }

        void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
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

