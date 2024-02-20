using UnityEngine;

public class NarratorDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueWakingUp;
    public Dialogue dialogueReturning;
    public Dialogue dialogueBirthdayMorning;
    public Dialogue dialogueBirthdayReturning;
    private Quest quest;

    void Start()
    {
        quest = FindObjectOfType<Quest>();

        if(GameManager.Instance.isTinaInHerRoom)
        {
            FindObjectOfType<DialogueManager>().DisplayDialogue("NARRATOR", "Tina was in her room.\n“Not again.” she thought");
            quest.GiveIndoorQuest(false);
        }
        else
        {
            Invoke("TriggerNarratorDialogue", .5f);
        }
    }

    private void TriggerNarratorDialogue()
    {
        if(PlayerPrefs.GetInt("CansuReturningHome", 0) == 0)
        {
            if(PlayerPrefs.GetInt("FirstNarratorDialogue", 1) == 0)
            {
                if(PlayerPrefs.GetInt("Day") == 7)
                {
                    FindObjectOfType<DialogueManager>().StartNarratorDialogue(dialogueBirthdayMorning, true);
                }
                else
                {
                    FindObjectOfType<DialogueManager>().StartNarratorDialogue(dialogueWakingUp, false);
                }
            }
            else if(PlayerPrefs.GetInt("FirstNarratorDialogue", 1) == 1)
            {
                FindObjectOfType<DialogueManager>().StartNarratorDialogue(dialogueWakingUp, true);
            }
        }
        else if(PlayerPrefs.GetInt("CansuReturningHome", 0) == 1)
        {
            if(PlayerPrefs.GetInt("Day") == 3 && PlayerPrefs.GetInt("GoneToTheDentist", 0) == 0)
            {
                FindObjectOfType<DialogueManager>().StartNarratorDialogue(dialogueReturning, true);
            }
            else if(PlayerPrefs.GetInt("Day") == 7)
            {
                FindObjectOfType<DialogueManager>().StartNarratorDialogue(dialogueBirthdayReturning, true);
            }
            else
            {
                FindObjectOfType<DialogueManager>().StartNarratorDialogue(dialogueReturning, false);
            }

            PlayerPrefs.SetInt("CansuReturningHome", 0);
        }
    }
}