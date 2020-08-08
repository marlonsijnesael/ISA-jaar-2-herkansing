using UnityEngine;
using UnityEditor;

using XNode;
using XNodeEditor;

public class ConversationNode : Node
{
    public string conversationText;

    [Input(backingValue = ShowBackingValue.Never)] public string connections;
    [Output] public string positive_Out;

    public string textOnScreen;
    public float waitSeconds = 1;
    public bool isFirst = false;
    public string fileName = "";
    public bool show = true;

    public override object GetValue(NodePort port)
    {
        // Check which output is being requested. 
        // In this node, there aren't any other outputs than "result".
        if (port.fieldName == "positive_Out")
        {
            // Return input value + 1
            return GetInputValue<string>("conversationText", this.conversationText);
        }

        // Hopefully this won't ever happen, but we need to return something
        // in the odd case that the port isn't "result"
        else return null;
    }
}


/// <summary>
/// custom editor gui for our conversation node
/// Visual distinction between the nodes makes it easier to follow the graph by eye
/// </summary>
[CustomNodeEditor(typeof(ConversationNode))]
public class SimpleNodeEditor : NodeEditor
{
    private ConversationNode conversationNode;

    public override void OnBodyGUI()
    {
        if (conversationNode == null) conversationNode = target as ConversationNode;


        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("isFirst"));

        if (serializedObject.FindProperty("isFirst").boolValue)
        {
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("fileName"));
        }

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("waitSeconds"));

        EditorGUILayout.LabelField("UI text");

        serializedObject.FindProperty("conversationText").stringValue = EditorGUILayout.TextField(serializedObject.FindProperty("conversationText").stringValue, GUILayout.Height(50));

        EditorGUILayout.LabelField("NPC Text");

        serializedObject.FindProperty("textOnScreen").stringValue = EditorGUILayout.TextField(serializedObject.FindProperty("textOnScreen").stringValue, GUILayout.Height(50));

        if (!serializedObject.FindProperty("isFirst").boolValue)
        {
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("connections"));
        }

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("positive_Out"));
        
        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}