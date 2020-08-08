using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConversationSet : ScriptableObject
{
    public List<ConversationPart> conversations;
    public List<ConversationPart> answers;

#if UNITY_EDITOR
    [HideInInspector] public string fileName;
#endif
}

[System.Serializable]
public class ConversationPart
{
    public int id;
    public string infoText;
    public string text;
    public float waitTime = 1;
    public List<ConversationPart> answers;//= new List<AnswerClass>();
    public bool isAnswer;
    public List<int> nextConversation;
    public List<int> nextAnswers;
    public List<ScriptableEvent> events;
}


//--------------------the old system-----------------/

[System.Serializable][System.Obsolete]
public class PointInConversation
{
    public int id;
    public string text;
    public string textToDisplay;
    public float waitTime = 1;
    public List<AnswerClass> answers;//= new List<AnswerClass>();
}


[System.Serializable][System.Obsolete]
public class AnswerClass
{
    public int id;
    public string text;
    public string textToDisplay;
    public float waitTime = 1;
    public PointInConversation nextPointInConversation = new PointInConversation();
}

