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

        private VWGraphView graphView;
        private Color defaultBackgroundColor;

        public virtual void Initialize(VWGraphView vwGraphView, Vector2 position)
        {
            DialogName = "DialogName";
            Choices = new List<string>();
            Text = "Dialog text.";

            graphView = vwGraphView;

            // There probably is no easy way to get the color from the .uss file..
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("vw-node__main-container");
            extensionContainer.AddToClassList("vw-node__extension-container");
        }

        public virtual void Draw()
        {
            // Title Container

            TextField dialogNameTextField = VWElementUtility.CreateTextField(DialogName, callback =>
            {
                graphView.RemoveUngroupedNode(this);
                DialogName = callback.newValue;
                graphView.AddUngroupedNode(this);
            });

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

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
    }
}

