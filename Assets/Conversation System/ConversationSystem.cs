using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConversationSystem : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private Text npcText;
    [SerializeField] private Text screenText;
    [SerializeField] private Text playerText;

    [SerializeField] private Text answerTextLeft, answerTextRight;

    [Header("Conversation")]
    [SerializeField] private ConversationSet currentConversation;

    [SerializeField] private ConversationPart currentPart;

    private bool isTalking = false;
    private float longStop = 1f;
    private float animationTime = 0.05f;

    private void Awake()
    {
        currentPart = currentConversation.conversations[0];
        StartCoroutine(TypeText(npcText, currentPart.text));
        screenText.text = currentPart.infoText;
        playerText.text = "";
        GetNextAnswers();
    }

    //helper function to get the answers we want to display on screen
    private void GetNextAnswers()
    {
        if (currentPart.nextAnswers.Count == 0) return;
        answerTextLeft.text = currentConversation.answers[currentPart.nextAnswers[0]].infoText;

        if (currentPart.nextAnswers.Count > 1)
        {
            answerTextRight.text = currentConversation.answers[currentPart.nextAnswers[1]].infoText;
        }
    }

    //helper function to get the next poin in the conversation
    private void SetNextConversationPart(int index)
    {
        if (currentConversation.answers[currentPart.nextAnswers[index]].nextConversation.Count >= 1)
        {
            int nextIndex = currentConversation.answers[currentPart.nextAnswers[index]].nextConversation[0] - 1;
            if (currentConversation.conversations[nextIndex] != null)
            {
                currentPart = currentConversation.conversations[nextIndex];
            }
        }
        else
        {
            screenText.text = "That's all folks";
        }
    }


    //This function is used to interact with this class from inside of a button element
    //choiceIndex is either 0 (left button) 1 (right button)
    public void ClickButton(int choiceIndex)
    {
        if (isTalking) return;
        if (currentPart.nextAnswers.Count == 0) return; //check if there are answers
        if (currentPart.nextAnswers.Count == 1 && choiceIndex == 1) return; //make sure we cannot click buttons without answers

        answerTextLeft.text = "";
        answerTextRight.text = "";
        StartCoroutine(GuideConversation(choiceIndex));
    }

    private bool HasNextAnswer()
    {
        return currentPart.nextAnswers.Count > 0;
    }
  
    /// <summary>
    /// This coroutine will handle the order in which the npc and the player talk to eachother
    /// when we call the coroutine TypeText inside of GuideConversation 
    /// it will pause execution until the coroutine is finished
    /// </summary>
    /// <param name="choiceIndex">the next node in the node editor</param>
    /// <returns></returns>
    private IEnumerator GuideConversation(int choiceIndex)
    {
        isTalking = true;
        yield return StartCoroutine(TypeText(playerText, currentConversation.answers[currentPart.nextAnswers[choiceIndex]].text));
        screenText.text = "";

        foreach (ScriptableEvent e in currentConversation.answers[currentPart.nextAnswers[choiceIndex]].events)
        {
            e.DoEvent();
        }

        SetNextConversationPart(choiceIndex);
        yield return StartCoroutine(TypeText(npcText, currentPart.text));
        screenText.text = currentPart.infoText;

        if (HasNextAnswer())
        {
            GetNextAnswers();
        }
        else
        {
            screenText.text = "That's all folks";
        }
        isTalking = false;
    }

    /// <summary>
    /// simple function to create a type writer effect 
    /// when we want to split a sentence for a cleaner text we can use the "/"
    /// </summary>
    /// <param name="txt"> the text element to typewrite </param>
    /// <param name="story"> the text to typewrite </param>
    /// <returns></returns>
    private IEnumerator TypeText(Text txt, string story)
    {
        txt.text = "";
        foreach (char c in story)
        {
            if (c == '/')
            {
                yield return new WaitForSeconds(longStop);
                txt.text = "";
            }
            else
            {
                txt.text += c;
            }
            yield return new WaitForSeconds(animationTime);
        }
        yield return new WaitForSeconds(longStop/2);
    }
}
