using System.Collections;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    //Used classes
    private CameraShake cameraShake;
    private DialogueManager dialogueManager;
    private Tina tina;
    private GameManager gameManager;

    //private fields
    [SerializeField] GameObject market;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject ish;

    //UI
    private GameObject questButton;
    private GameObject questListPanel;
    private TextMeshProUGUI newQuestText;
    private TextMeshProUGUI questText;

    void Start()
    {
        gameManager = GameManager.Instance;
        questButton = transform.GetChild(0).gameObject;
        questListPanel = transform.GetChild(1).gameObject;
        newQuestText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        questText = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();

        cameraShake = FindObjectOfType<CameraShake>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        tina = FindObjectOfType<Tina>();

        SetQuestText(PlayerPrefs.GetString("CurrentQuest" , ""));

        if(PlayerPrefs.GetString("CurrentQuest") == "Buy treat for training." &&
        gameManager.IsOutdoorScene())
        {
            market.GetComponent<BoxCollider2D>().enabled = true;
        }

        if(gameManager.IsMazeScene())
        {
            PlayerPrefs.SetString("CurrentQuest", "Find your way to home.");
            Invoke("NewQuestGiven", 2f);
        }

        ///////////////////////////////////////////////////////////FOR TESTING///////////////////////////////////////////////////////////
        //PlayerPrefs.SetInt("Day", 7);/////////////////////////////////////////////////////////////////////////////////////////////////
        //Debug.Log("TESTING IS ACTIVE");//////////////////////////////////////////////////////////////////////////////////////////////
        //PlayerPrefs.SetString("CurrentQuest", "Take Tina for a walk.");/////////////////////////////////////////////////////////////
        //questText.text = PlayerPrefs.GetString("CurrentQuest");////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Debug.Log("Day: " + PlayerPrefs.GetInt("Day", 1) + ". It is " + (PlayerPrefs.GetInt("IsNightTime") == 1 ? "night time." : "day time"));
    }

    public void NewQuestGiven()
    {
        newQuestText.text = "NEW QUEST";

        questText.text = PlayerPrefs.GetString("CurrentQuest");

        OpenQuestListPanel();
    }

    public void GiveIndoorQuest(bool isCoffee)
    {
        CancelInvoke();

        if(PlayerPrefs.GetInt("CansuReturningHome") == 0)
        {
            if(isCoffee)
            {
                PlayerPrefs.SetString("CurrentQuest", "Make coffee.");
                cameraShake.ActivateShake(true);
            }
            else
            {
                PlayerPrefs.SetString("CurrentQuest", "Take Tina for a walk.");
            }
        }
        if(PlayerPrefs.GetInt("CansuReturningHome") == 1)
        {
            if(PlayerPrefs.GetInt("Day") == 3 && 
            gameManager.IsIndoorScene() &&
            PlayerPrefs.GetInt("GoneToTheDentist", 0) == 0)
            {
                PlayerPrefs.SetString("CurrentQuest", "Go to the dentist.");
                ///Tina does not follow
                ///Door says dentist
            }
            else
            {
                PlayerPrefs.SetString("CurrentQuest", "Get some sleep.");
            }
        }

        Invoke(nameof(NewQuestGiven), 2f);
    }

    private void GiveOutdoorQuest(bool isShit)
    {
        if(isShit)
        {
            StartCoroutine(WaitThenDisplayDialogue(3f, "Tina", "Poo poo time!"));
            PlayerPrefs.SetString("CurrentQuest", "Pick the shitling's shit up");
        }
        else
        {
            switch(PlayerPrefs.GetInt("Day", 1))
            {
                case 1:
                    PlayerPrefs.SetString("CurrentQuest", "Buy treat for training.");
                    market.GetComponent<BoxCollider2D>().enabled = true;
                    break;
                case 2:
                    PlayerPrefs.SetString("CurrentQuest", "Teach Tina to sit.");
                    break;
                case 3:
                    PlayerPrefs.SetString("CurrentQuest", "Catch Tina!");
                    AudioManager.Instance.Play("Fireworks");
                    tina.TinaRanAway();
                    break;
                case 4:
                    PlayerPrefs.SetString("CurrentQuest", "Teach Tina to wait/come.");
                    break;
                case 5:
                    PlayerPrefs.SetString("CurrentQuest", "Introduce yourself.");
                    ish.GetComponent<CircleCollider2D>().enabled = true;
                    break;
                case 6:
                    PlayerPrefs.SetString("CurrentQuest", "Buy beer.");
                    market.GetComponent<BoxCollider2D>().enabled = true;
                    break;
                default:
                    PlayerPrefs.SetString("CurrentQuest", "Get some sleep.");
                    break;
            }
        }

        Invoke(nameof(NewQuestGiven), 2.5f);
    }

    IEnumerator WaitThenDisplayDialogue(float seconds, string speaker, string dialogue)
    {
        yield return new WaitForSeconds(seconds);

        dialogueManager.DisplayDialogue(speaker, dialogue);
    }

    public void QuestCompleted()
    {
        switch(PlayerPrefs.GetString("CurrentQuest"))
        {
            case "Make coffee.":
                cameraShake.ActivateShake(false);
                GiveIndoorQuest(false);
                break;
            case "Take Tina for a walk.":
                GiveOutdoorQuest(true);
                tina.GoToShit();
                break;
            case "Pick the shitling's shit up":
                GiveOutdoorQuest(false);
                break;
            case "Buy beer.":
                PlayerPrefs.SetString("CurrentQuest", "Drink with him.");
                NewQuestGiven();
                break;
            case "Find your way to home.":
                PlayerPrefs.SetString("CurrentQuest", "Find the end of the maze.");
                NewQuestGiven();
                break;
            default://Default if outside quests are completed.
                GiveIndoorQuest(false);
                break;
        }
    }

    private void SetQuestText(string questInfo)
    {
        newQuestText.text = "QUEST";

        questText.text = questInfo;
    }

    public void OpenQuestListPanel()
    {
        CancelInvoke();

        if(gameManager.IsIndoorScene())
        {
            menu.SetActive(false);
        }

        questButton.SetActive(false);
        questListPanel.SetActive(true);

        Invoke(nameof(CloseQuestListPanel), 4f);
    }

    private void CloseQuestListPanel()
    {
        if(gameManager.IsIndoorScene())
        {
            menu.SetActive(true);
        }

        questButton.SetActive(true);
        questListPanel.SetActive(false);

        SetQuestText(PlayerPrefs.GetString("CurrentQuest"));
    }
}