using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartNPCDialogue(dialogue);
    }

    public void TriggerEventDialogue()
    {
        FindObjectOfType<DialogueManager>().StartEventDialogue(dialogue);
    }
}