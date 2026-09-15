using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "GBTemplate/DialogData")]
[CreateAssetMenu(menuName = "BigDialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<DialogueSentence> Sentences;
    public Portrait Character1;
    public Portrait Character2;
    public bool canPlayerMove;
    public bool chapter1;
}

[System.Serializable]
public class DialogueSentence
{
    [TextArea(3, 10)]
    public string Text;
    public int emotion;
    public string name;
    public AudioClip sound;
}