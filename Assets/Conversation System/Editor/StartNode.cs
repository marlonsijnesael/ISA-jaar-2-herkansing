using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using UnityEditorInternal;
using UnityEditor;

public class StartNode : Node
{
    public string test = "test";
    [Output(backingValue = ShowBackingValue.Never)] public string connection;
    public override object GetValue(NodePort port)
    {
        // Check which output is being requested. 
        // In this node, there aren't any other outputs than "result".
        if (port.fieldName == "connection")
        {
            // Return input value + 1
            return connection;//GetInputValue<string>("connection", this.test);
        }
        // Hopefully this won't ever happen, but we need to return something
        // in the odd case that the port isn't "result"
        else return null;
    }
}



public class CreateObject
{
    // Start is called before the first frame update [MenuItem("Assets/Create/My Scriptable Object")]
    public static void CreateMyAsset( ConversationSet asset, string name = "untitled")
    {

        AssetDatabase.CreateAsset(asset, "Assets/" + name + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
