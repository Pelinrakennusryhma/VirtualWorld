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

            TextField dialogNameTextField = VWElementUtility.CreateTextField(DialogName);

            dialogNameTextField.AddClasses(
                "vw-node__text-field", 
                "vw-node__filename-text-field", 
                "vw-node__text-field__hidden"
            );

            titleContainer.Insert(0, dialogNameTextField);

            // Input Container

            Port inputPort = this.CreatePort("Dialog Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            // Extension Container

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("vw-node__custom-data-container");

            Foldout textFoldout = VWElementUtility.CreateFoldout("Dialog Text");

            TextField textTextField = VWElementUtility.CreateTextArea(Text);

            textTextField.AddClasses("vw-node__text-field", "vw-node__quote-text-field");

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }
    }
}

