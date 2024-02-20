using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public bool nPCSpeaks;
    public bool atDialogue;

    [TextArea(3,5)]
    public string[] sentences0;

    [TextArea(3,5)]
    public string[] sentences1;

    [TextArea(3,5)]
    public string[] sentences2;

    [TextArea(3,5)]
    public string[] eventSentences;
}