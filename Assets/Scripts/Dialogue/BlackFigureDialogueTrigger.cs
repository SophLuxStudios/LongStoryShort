using UnityEngine;

public class BlackFigureDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    void Start()
    {
        Invoke(nameof(TriggerNarratorDialogue), .5f);
    }

    private void TriggerNarratorDialogue()
    {
        FindObjectOfType<DialogueManager>().StartEventDialogue(dialogue);
    }
}