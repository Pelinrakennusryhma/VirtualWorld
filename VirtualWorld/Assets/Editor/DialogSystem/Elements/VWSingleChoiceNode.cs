using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialog
{
    public class VWSingleChoiceNode : VWNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);


            Choices.Add("Next Dialog");
        }

        public override void Draw()
        {
            base.Draw();

            // Output container

            foreach (string choice in Choices)
            {
                Port choicePort = this.CreatePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}

