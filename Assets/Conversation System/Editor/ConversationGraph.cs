using XNode;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ConversationGraph : NodeGraph
{
    int answers = 0;
    int convos = 0;


    //helper function to get all the conversationNodes
    public int GetNodeCount()
    {
        int counter = 0;
        foreach (Node n in nodes)
        {
            if (n is ConversationNode)
                counter++;
        }
        return counter;
    }

    //helper function to add all the nodes to a list - debugging-
    public void AddNodesToList(ConversationSet c)
    {
        foreach (Node n in nodes)
        {
            if (n is ConversationNode)
            {
                ConversationNode node = n as ConversationNode;
                ConversationPart p = new ConversationPart();
                p.id = convos;
                convos++;
                p.infoText = node.conversationText;
                p.text = node.textOnScreen;
                p.waitTime = node.waitSeconds;
                c.conversations.Add(p);
            }

            if (n is AnswerNode)
            {
                AnswerNode node = n as AnswerNode;
                ConversationPart p = new ConversationPart();
                p.id = answers;
                answers++;
                p.infoText = node.answerText;
                p.text = node.answerTextOnScreen;
                p.waitTime = node.waitSeconds;
                c.answers.Add(p);
            }
        }
    }


    [ContextMenu("Convert to file")]
    public void ReturnAllNodes()
    {
        //first we create a new container in the form of a scriptable object
        ConversationSet c1 = CreateInstance<ConversationSet>();
    
        //loop through all nodes until we find the first instance of conversationNode 
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is ConversationNode)
            {
                ConversationNode cNode = nodes[i] as ConversationNode;
                c1.conversations = new List<ConversationPart>();
                c1.answers = new List<ConversationPart>();
                if (cNode.fileName == string.Empty || cNode.fileName == null)
                {
                    c1.fileName = "untitled";
                }
                else
                {
                    c1.fileName = cNode.fileName;
                }

                //run a recursive fuction to loop through all nodes in the graph
                TraceConversation(cNode, c1);

                //call the helperfunction CreateMyAsset to actually save the created scriptable object
                CreateObject.CreateMyAsset(c1, c1.fileName);
                return;
            }
        }
    }

    /// <summary>
    /// Loop through all nodes in our graph
    /// When we reach a branch we first loop through each branch until finished
    /// </summary>
    /// <param name="n"> the current conversationNode in the graph </param>
    /// <param name="c"> The conversationSet to save to </param>
    private void TraceConversation(ConversationNode n, ConversationSet c)
    {
        //first we create a new part, and populate this with each answer
        ConversationPart p = new ConversationPart();
        p.nextAnswers = new List<int>();
        p.id = convos;
        p.isAnswer = false;
        convos++;
        p.infoText = n.conversationText;
        p.text = n.textOnScreen;
        p.waitTime = n.waitSeconds;
        c.conversations.Add(p);

        //check if something is connected to the answer port
        if (n.GetOutputPort("positive_Out").IsConnected)
        {
            //loop through all connected answer nodes
            for (int i = 0; i < n.GetOutputPort("positive_Out").GetConnections().Count; i++)
            {
                NodePort nP = n.GetOutputPort("positive_Out").GetConnection(i);
                AnswerNode node = (nP.node as AnswerNode);
                ConversationPart pA = new ConversationPart();
                pA.nextConversation = new List<int>();
                pA.events = new List<ScriptableEvent>();
                
                foreach (ScriptableEvent e in node.events)
                {
                    pA.events.Add(e);
                }

                pA.id = answers;

                pA.infoText = node.answerText;
                pA.text = node.answerTextOnScreen;
                pA.waitTime = node.waitSeconds;
                pA.isAnswer = true;
                //set next index to the 

                c.answers.Add(pA);
                p.nextAnswers.Add(answers);
                answers++;


                //check if a new conversation is connected to the answer node
                if (nP.GetConnection(0).node is ConversationNode)
                {
                    if (nP.node.GetOutputPort("answerText").IsConnected)
                    {
                        pA.nextConversation.Add(convos + 1);
                        
                        //call this function again until there are no connection left
                        TraceConversation(nP.node.GetOutputPort("answerText").GetConnection(0).node as ConversationNode, c);
                        Debug.Log(nP.node.GetOutputPort("answerText").GetConnection(0).node.name);
                    }
                }
            }
        }
    }



    private void GetNextConversation(ConversationNode n, ConversationSet c, PointInConversation p)
    {
        //p.text = n.conversationText;
        //p.textToDisplay = n.textOnScreen;
        //p.waitTime = n.waitSeconds;

        //if (n.GetOutputPort("positive_Out").IsConnected)
        //{
        //    p.answers = new List<AnswerClass>();

        //    for (int i = 0; i < n.GetOutputPort("positive_Out").GetConnections().Count; i++)
        //    {
        //        c.connections.Add(i.ToString());
        //        NodePort nP = n.GetOutputPort("positive_Out").GetConnection(i);

        //        AnswerClass positiveAnswer = new AnswerClass();
        //        positiveAnswer.text = (nP.node as AnswerNode).answerText;
        //        positiveAnswer.textToDisplay = (nP.node as AnswerNode).answerTextOnScreen;
        //        positiveAnswer.waitTime = (nP.node as AnswerNode).waitSeconds;

        //        p.answers.Add(positiveAnswer);
        //        if (nP.GetConnection(0).node is ConversationNode)
        //        {
        //            if (nP.node.GetOutputPort("answerText").IsConnected)
        //            {
        //                GetNextConversation(nP.node.GetOutputPort("answerText").GetConnection(0).node as ConversationNode, c, p.answers[i].nextPointInConversation);
        //                Debug.Log(nP.node.GetOutputPort("answerText").GetConnection(0).node.name);
        //            }
        //        }
        //    }
        //}
    }

   


}

