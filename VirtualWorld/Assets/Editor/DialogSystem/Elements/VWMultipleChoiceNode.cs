using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialog
{
    public class VWMultipleChoiceNode : VWNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            Choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container

            Button addChoiceButton = new Button()
            {
                text = "Add Choice"
            };

            addChoiceButton.AddToClassList("vw-node__button");

            mainContainer.Insert(1, addChoiceButton);

            // Output container

            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

                choicePort.portName = "";

                Button deleteChoiceButton = new Button()
                {
                    text = "X"
                };

                deleteChoiceButton.AddToClassList("vw-node__button");

                TextField choiceTextField = new TextField()
                {
                    value = choice
                };

                choiceTextField.AddToClassList("vw-node__text-field");
                choiceTextField.AddToClassList("vw-node__choice-text-field");
                choiceTextField.AddToClassList("vw-node__text-field__hidden");

                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}

