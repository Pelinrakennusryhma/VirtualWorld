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

        public virtual void Initialize(Vector2 position)
        {
            DialogName = "DialogName";
            Choices = new List<string>();
            Text = "Dialog text.";

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("vw-node__main-container");
            extensionContainer.AddToClassList("vw-node__extension-container");
        }

        public virtual void Draw()
        {
            // Title Container

            TextField dialogNameTextField = new TextField()
            {
                value = DialogName
            };

            dialogNameTextField.AddToClassList("vw-node__text-field");
            dialogNameTextField.AddToClassList("vw-node__filename-text-field");
            dialogNameTextField.AddToClassList("vw-node__text-field__hidden");

            titleContainer.Insert(0, dialogNameTextField);

            // Input Container

            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            inputPort.portName = "Dialog Connection";

            inputContainer.Add(inputPort);

            // Extension Container

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("vw-node__custom-data-container");

            Foldout textFoldout = new Foldout()
            {
                text = "Dialog Text"
            };

            TextField textTextField = new TextField()
            {
                value = Text
            };

            textTextField.AddToClassList("vw-node__text-field");
            textTextField.AddToClassList("vw-node__quote-text-field");

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }
    }
}

