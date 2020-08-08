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

//[CustomNodeEditor(typeof(StartNode))]
//public class EndingNodeEditor : NodeEditor
//{
//    public override void OnBodyGUI()
//    {
//        StartNode node = target as StartNode;
//        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("connection"));

//        if (GUILayout.Button("check input"))
//        {
//            NodePort outPort = node.GetOutputPort("connection");
//            List<NodePort> tmp = outPort.GetConnections();
//            convo c1 = ScriptableObject.CreateInstance<convo>();
//            c1.nodes = new List<Node>();
//            c1.nodes.Add(node);
//            //c1.connects = outPort.GetConnections();
//            Node tmpNode = node.GetPort("connection").GetConnection(0).node;
//            int layers = 0;
//            while (tmpNode != null && layers <100)
//            {
//                c1.nodes.Add(tmpNode);
//                if (tmpNode.GetPort("answerText").GetConnections() != null)
//                {
//                    Debug.Log(tmpNode.GetPort("answerText").GetConnections().Count);
//                    //tmpNode = tmpNode.GetPort("answerText").GetConnections()[0].node;
//                    layers++;
//                }
//            }
//            CreateObject.CreateMyAsset("hello", c1);

//            //Debug.Log(tmp[0].node.name);
//            //if (tmp[0].node)
//            //{
//            //    ConversationNode n = tmp[0].node as ConversationNode;
//            //    // convo c1 = new convo(n.conversationText, n.positive_Out, n.Negative_Out);
//            //    convo c1 = ScriptableObject.CreateInstance<convo>();
//            //    c1.topic = n.conversationText;

//            //    c1.connects = n.GetOutputPort("positive_Out").GetConnections();
//            //    c1.posAns = (c1.connects[0].node as AnswerNode).answerText;
//            //    CreateObject.CreateMyAsset("hello", c1);

//            //}
//        }
//    }
//}



[System.Serializable]
public class convo : ScriptableObject
{
    public string topic;
    public string posAns, negAns;
    public List<NodePort> connects;
    public List<Node> nodes;
    public convo(string t, string p, string n)
    {
        topic = t;
        posAns = p;
        negAns = n;
    }
}

public class CreateObject
{
    // Start is called before the first frame update [MenuItem("Assets/Create/My Scriptable Object")]
    public static void CreateMyAsset(string name, ConversationSet asset)
    {

        AssetDatabase.CreateAsset(asset, "Assets/" + name + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
// }
// public class ConversationTest : ScriptableObject
// {
//     public List<ConversationStore> topics;
//     public string topic;
//     public string answerA, answerB;
//     public bool endOfConversation = false;
// }



//public void FetchInfo()
//{

//    ConversationNode check = (ConversationNode)base.connection as ConversationNode;
//    List<ConversationStore> topics = new List<ConversationStore>();
//    int layers = 0;

//    while (check && layers < 100)
//    {
//        AnswerNode a1 = (AnswerNode)check.input1 as AnswerNode;
//        AnswerNode a2 = (AnswerNode)check.input2 as AnswerNode;
//        ConversationStore store = new ConversationStore(check.text, a1.text, a2.text);
//        topics.Add(store);
//        check = (ConversationNode)check.connection as ConversationNode;
//        layers++;

//    }

//    topics.Reverse();
//    foreach (ConversationStore s in topics)
//    {
//        Debug.Log(s);
//    }
//    ConversationTest asset = ScriptableObject.CreateInstance<ConversationTest>();
//    asset.topics = topics;
//    CreateObject.CreateMyAsset("hello", asset);
//}
