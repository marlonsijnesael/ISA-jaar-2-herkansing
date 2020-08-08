using UnityEngine;
using System.Collections.Generic;
using XNode;

public class AnswerNode : Node
{
    [Input(backingValue = ShowBackingValue.Never)] public string connection;
    [Output(backingValue = ShowBackingValue.Always)] public string answerText;

    public string answerTextOnScreen;
    public float waitSeconds = 1;
    public List<ScriptableEvent> events;

    public override object GetValue(NodePort port)
    {
        // Check which output is being requested. 
        // In this node, there aren't any other outputs than "result".
        if (port.fieldName == "connection")
        {
            // Return input value + 1
            return GetInputValues<string>("connection", "this.connection");
        }
        // Hopefully this won't ever happen, but we need to return something
        // in the odd case that the port isn't "result"
        else return null;
    }
}

//[CustomNodeEditor(typeof(AnswerNode))]
//public class StateNodeEditor : NodeEditor
//{
//    public override void OnBodyGUI()
//    {
//        AnswerNode node = target as AnswerNode;
//        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("connection"));
//        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("answerText"));
//        if (GUILayout.Button("check input"))
//        {
//            NodePort inPort = target.GetPort("connection");
//            Debug.Log(inPort.GetInputValue());
//        }
//    }
//}