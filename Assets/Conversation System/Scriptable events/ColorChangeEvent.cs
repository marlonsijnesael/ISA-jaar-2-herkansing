using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Conversation Events / ColorChange")]
public class ColorChangeEvent : ScriptableEvent
{
    public Color spriteColor;

    public override void DoEvent()
    {
        GameObject npcBackground = GameObject.FindGameObjectWithTag("npc background");
        Image img = npcBackground.GetComponent<Image>();
        img.color = spriteColor;
    }
}
