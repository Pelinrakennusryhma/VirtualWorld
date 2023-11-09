using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialog
{
    public class VWMultipleChoiceNode : VWNode
    {
        public override void Initialize(VWGraphView vwGraphView, Vector2 position)
        {
            base.Initialize(vwGraphView, position);

            Choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container

            Button addChoiceButton = VWElementUtility.CreateButton("Add Choice", () =>
            {
                Port choicePort = CreateChoicePort("New Choice");

                Choices.Add("New Choice");
                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("vw-node__button");

            mainContainer.Insert(1, addChoiceButton);

            // Output container

            foreach (string choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        #region Element Creation
        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort();

            Button deleteChoiceButton = VWElementUtility.CreateButton("X");

            deleteChoiceButton.AddToClassList("vw-node__button");

            TextField choiceTextField = VWElementUtility.CreateTextField(choice);

            choiceTextField.AddClasses(
                "vw-node__text-field",
                "vw-node__choice-text-field",
                "vw-node__text-field__hidden"
            );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
        #endregion
    }
}

