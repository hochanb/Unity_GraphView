using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EventGraph
{

    public class EventNode : Node
    {
        public Port input;
        public Port output;

        public EventNode(string nodeName)
        {
            title = nodeName;

            // Input port
            input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            input.portName = "Input";
            inputContainer.Add(input);

            // Output port
            output = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            output.portName = "Output";
            outputContainer.Add(output);

            // Add a text field
            var textField = new TextField("Description");
            textField.RegisterValueChangedCallback(evt => title = evt.newValue);
            mainContainer.Add(textField);

            RefreshExpandedState();
            RefreshPorts();
        }

    }
}