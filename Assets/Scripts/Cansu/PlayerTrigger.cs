using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    //used classes
    private DialogueManager dialogueManager;
    private Quest quest;

    //private fields
    private string triggerName;
    private bool atPark;
    [SerializeField] GameObject torches;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        quest = FindObjectOfType<Quest>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Trigger"))
        {
            triggerName = other.name;
            switch(triggerName)
            {
                case "GoOut":
                    if(PlayerPrefs.GetString("CurrentQuest") == "Go to the dentist.")
                    {
                        PlayerPrefs.SetInt("CansuReturningHome", 1);
                        PlayerPrefs.SetInt("GoneToTheDentist", 1);
                        quest.QuestCompleted();
                        GameManager.Instance.LoadIndoorScene("IndoorDentist");
                    }
                    else
                    {
                        GameManager.Instance.LoadOutdoorScene();
                    }
                    break;
                case "Corner":
                    dialogueManager.DisplayDialogue("NARRATOR", "There she turned the corner as elegant as ever");
                    break;
                case "Omrumce":
                    dialogueManager.DisplayDialogue("NARRATOR", "“I bet people live in this building are really cool.” she said unknowing her future");
                    break;
                case "Park":
                    if(PlayerPrefs.GetString("CurrentQuest") == "Take Tina for a walk.")
                    {
                        quest.QuestCompleted();
                    }
                    if(!atPark)
                    {
                        atPark = true;
                        dialogueManager.DisplayDialogue("NARRATOR", "She entered the park that would change her life,\nForever.");
                        dialogueManager.InvokeClearDisplay(2.5f);
                    }
                    break;
                case "Bazaar":
                    dialogueManager.DisplayDialogue("NARRATOR", "She never visited that bazaar.");
                    break;
                case "AlienRoadBlock":
                    dialogueManager.DisplayDialogue("Unknown", "We must go now.");
                    break;
                case "MazeEndTrigger":
                    torches.SetActive(true);
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Trigger"))
        {
            if(triggerName != "Park")
            {
                ClearDialoguePanel();
            }
            
            triggerName = null;
        }
    }

    private void ClearDialoguePanel()
    {
        dialogueManager.InvokeClearDisplay(1.5f);
    }
}