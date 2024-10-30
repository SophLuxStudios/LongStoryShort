using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public bool NPCSpeaks;
    public bool AtDialogue;

    [TextArea(3,5)]
    public string[] sentences0;

    [TextArea(3,5)]
    public string[] sentences1;

    [TextArea(3,5)]
    public string[] sentences2;

    [TextArea(3,5)]
    public string[] eventSentences;
}