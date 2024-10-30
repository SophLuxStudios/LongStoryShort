using UnityEngine;

public class NarratorOurdoorDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueSteppingOut;

    void Start()
    {
        Invoke(nameof(TriggerNarratorDialogue), .5f);
    }

    private void TriggerNarratorDialogue()
    {  
        FindObjectOfType<DialogueManager>().StartNarratorDialogue(dialogueSteppingOut, false);
    }
}