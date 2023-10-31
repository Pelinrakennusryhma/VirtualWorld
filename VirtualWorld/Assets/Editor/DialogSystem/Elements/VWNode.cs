using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialog
{
    public class VWNode : Node
    {
        public string DialogName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public VWDialogType DialogType { get; set; }

        public void Initialize()
        {
            DialogName = "DialogName";
            Choices = new List<string>();
            Text = "Dialog text.";
        }

        public void Draw()
        {
            // Title Container

            TextField dialogNameTextField = new TextField()
            {
                value = DialogName
            };

            titleContainer.Insert(0, dialogNameTextField);

            // Input Container

            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            inputPort.portName = "Dialog Connection";

            inputContainer.Add(inputPort);

            // Extension Container

            VisualElement customDataContainer = new VisualElement();

            Foldout textFoldout = new Foldout()
            {
                text = "Dialog Text"
            };

            TextField textTextField = new TextField()
            {
                value = Text
            };

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }
    }
}

