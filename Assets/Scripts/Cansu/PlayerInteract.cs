using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    //used classes
    private GameManager gameManager;
    private DialogueManager dialogueManager;
    private Tina tina;
    private Quest quest;

    //private fields
    private string dialogue;
    private string interactableName;
    private string NPCName;
    [SerializeField] private GameObject interactedObject;
    private bool tinaWaiting;
    private bool tinaHasSitten;
    private string dreamSceneName;

    //button labels
    private const string interactLable = "INTERACT";
    private const string nextLable = "NEXT";
    private const string speakLable = "SPEAK";
    private const string petHerLable = "PET HER";
    private const string againLable = "AGAIN";
    
    //public variables
    public bool isTinaEvent;
    public bool isNarratorDialogue;
    public bool isDreamDialogue;

    //UI
    [SerializeField] private TextMeshProUGUI buttonText;

    void Start()
    {
        gameManager = GameManager.Instance;
        dialogueManager = FindObjectOfType<DialogueManager>();
        tina = FindObjectOfType<Tina>();
        quest = FindObjectOfType<Quest>();

        //ClearDialoguePanel();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Interactable"))
        {
            interactableName = other.name;
            switch(interactableName)
            {
                case "Mirror":
                    ChangeButtonText("MIRROR");
                    break;
                case "Bed":
                    ChangeButtonText("SLEEP");
                    break;
                case "CoffeeMachine":
                    if(PlayerPrefs.GetString("CurrentQuest") == "Make coffee.")
                    {
                        interactedObject = other.gameObject;
                        ChangeButtonText("MAKE\nCOFFEE");
                    }
                    break;
                case "Door":
                    if(PlayerPrefs.GetString("CurrentQuest") != "Make coffee.")
                    {
                        if(PlayerPrefs.GetString("CurrentQuest") == "Go to the dentist.")
                        {
                            ChangeButtonText("DENTIST");
                        }
                        else
                        {
                            ChangeButtonText("GO OUT");
                        }

                        interactedObject = other.gameObject;
                    }
                    break;
                case "BuildingDoor":
                    ChangeButtonText("GO\nINSIDE");
                    break;
                case "DreamBuildingDoor":
                    ChangeButtonText("GO\nINSIDE");
                    break;
                case "ParkSign":
                    ChangeButtonText("READ\nSIGN");
                    break;
                case "Poop(Clone)":
                    ChangeButtonText("PICK UP");
                    interactedObject = other.gameObject;
                    break;
                case "Market":
                    if(PlayerPrefs.GetString("CurrentQuest") == "Buy beer.")
                    {
                        ChangeButtonText("BUY\nBEER");
                    }
                    else
                    {
                        ChangeButtonText("BUY\nTREAT");
                    }
                    interactedObject = other.gameObject;
                    break;
                case "AlienScreen":
                case "Module1":
                case "Module2":
                    ChangeButtonText("READ\nSCREEN");
                    break;
                case "HumanContainer":
                    ChangeButtonText("SLEEP");
                    break;
                case "Panel":
                    ChangeButtonText("SHORT\nCIRCUIT");
                    break;
                case "SpaceCraft":
                    ChangeButtonText("FLY");
                    break;
                default:
                    ChangeButtonText(interactLable);
                    break;
            }
        }
        else if(other.CompareTag("NPC") && PlayerPrefs.GetString("CurrentQuest") != "Make coffee.")
        {
            NPCName = other.name;

            switch(NPCName)
            {
                case "Whisper":
                    ChangeButtonText(petHerLable);
                    interactedObject = other.gameObject;
                    break;
                case "Tina":
                    if(isTinaEvent)
                    {
                        ChangeButtonText("SCOLD\nHER");
                    }
                    else if(PlayerPrefs.GetString("CurrentQuest") == "Teach Tina to sit.")
                    {
                        ChangeButtonText("TEACH\nSIT");
                    }
                    else if(PlayerPrefs.GetString("CurrentQuest") == "Teach Tina to wait/come.")
                    {
                        ChangeButtonText("TEACH\nWAIT");
                    }
                    else if(PlayerPrefs.GetString("CurrentQuest") == "Catch Tina!")
                    {
                        ChangeButtonText("CATCH");
                    }
                    else
                    {
                        ChangeButtonText(petHerLable);
                    }
                    interactedObject = other.gameObject;
                    break;
                case "Sophia":
                    ChangeButtonText(speakLable);
                    interactedObject = other.gameObject;
                    break;
                case "Caner":
                    ChangeButtonText(speakLable);
                    interactedObject = other.gameObject;
                    break;
                case "Ish":
                    if(PlayerPrefs.GetString("CurrentQuest") == "Drink with him.")
                    {
                        ChangeButtonText("BOTTOMS\nUP");
                    }
                    else
                    {
                        ChangeButtonText(speakLable);
                    }
                    interactedObject = other.gameObject;
                    break;
                case "BlackFigure":
                    ChangeButtonText(speakLable);
                    interactedObject = other.gameObject;
                    break;
                default:
                    ChangeButtonText(interactLable);
                    break;
            }

            dialogueManager.currentDialogue = interactedObject.GetComponent<DialogueTrigger>().dialogue;
        }
    }

    private void ChangeButtonText(string text)
    {
        buttonText.text = text;
    }

    public void NPCDialogueEnded()
    {
        switch(NPCName)
        {
            case "Whisper":
                ChangeButtonText($"{petHerLable}\n{againLable}");
                break;
            case "Tina":
                ChangeButtonText($"{petHerLable}\n{againLable}");
                break;
            default:
                ChangeButtonText($"{speakLable}\n{againLable}");
                break;
        }

        Debug.Log("PlayerInteract.NPCDialogueEnded() has run.");

        //Enable movement when conversation ends
        gameManager.EnableMovement(true);
    }

    public void NPCEventDialogueEnded()
    {
        /*switch(NPCName)
        {
            case "Tina":
                ChangeButtonText(interactLable);
                break;
            case "Ish":
                ChangeButtonText(interactLable);
                break;
            default:
                ChangeButtonText(interactLable);
                break; 
        }*/

        ChangeButtonText(interactLable);

        if(gameManager.IsIndoorScene())
        {
            tina.ReturnKitchen();
        }
    }

    public void NPCDialogueStarted()
    {
        ChangeButtonText(nextLable);

        //Disable movement when conversation starts
        gameManager.EnableMovement(false);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Interactable") || other.CompareTag("NPC"))
        {
            if(dialogueManager.currentDialogue == null)
            {
                ChangeButtonText(interactLable);
                ClearDialoguePanel();
            }
            else if(!dialogueManager.currentDialogue.AtDialogue)
            {
                //Debug.Log("exited.");
                ChangeButtonText(interactLable);
                ClearDialoguePanel();
            }
            
            if(tinaHasSitten && PlayerPrefs.GetString("CurrentQuest") == "Teach Tina to sit.")
            {
                Debug.Log("Tina has learned to sit.");
                dialogueManager.DisplayDialogue("Cansu", "Good girl here take it.");
                tina.shouldFollowCansu = true;
                quest.QuestCompleted();
                Invoke(nameof(ClearDialoguePanel), 1.5f);
            }
            else if(tinaWaiting && PlayerPrefs.GetString("CurrentQuest") == "Teach Tina to wait/come.")
            {
                ChangeButtonText("TEACH\nCOME");
                Invoke(nameof(ClearDialoguePanel), 1.5f);
            }
            else if(PlayerPrefs.GetString("CurrentQuest") == "Drink with him.")
            {
                ChangeButtonText(interactLable);
                Invoke(nameof(ClearDialoguePanel), 2f);
            }
        }

        NPCName = null;
        interactableName = null;
        interactedObject = null;
    }

    private void ClearDialoguePanel()
    {
        dialogueManager.InvokeClearDisplay(1f);
    }

    void OnInteract()
    {
        CancelInvoke();
        
        if(NPCName == null && interactableName == null && !isNarratorDialogue && !isDreamDialogue)
        {
            if(tinaWaiting)
            {
                dialogueManager.DisplayDialogue("Cansu", "Tina come.\nGood girl.");
                Invoke(nameof(ClearDialoguePanel), 1.5f);
                tina.shouldFollowCansu = true;
                tinaWaiting = false;
                ChangeButtonText(interactLable);
                quest.QuestCompleted();
            }
            else
            {
                EmptyInteraction();
            }
        }
        else if(isNarratorDialogue)
        {
            dialogueManager.DisplayNextNarratorSentence();
        }
        else if(isDreamDialogue)
        {
            dialogueManager.DisplayNextSentence();
        }
        else if(NPCName != null) //Interacting with NPC
        {
            if(isTinaEvent)
            {
                NPCEventTrigger(interactedObject.GetComponent<DialogueTrigger>());
            }
            else if(PlayerPrefs.GetString("CurrentQuest") == "Teach Tina to sit.")
            {
                dialogueManager.DisplayDialogue("Cansu", "Tina sit.");
                Invoke(nameof(ClearDialoguePanel), 1.5f);
                tina.shouldFollowCansu = false;
                tina.TinaSit(true);
                tinaHasSitten = true;
            }
            else if(PlayerPrefs.GetString("CurrentQuest") == "Catch Tina!")
            {
                dialogueManager.DisplayDialogue("Cansu", "Bad Tina!!!!!!!!\nYou scared the shit out of me.");
                tina.TinaGotCaught();
                quest.QuestCompleted();
                ChangeButtonText(petHerLable);
            }
            else if(PlayerPrefs.GetString("CurrentQuest") == "Teach Tina to wait/come.")
            {
                dialogueManager.DisplayDialogue("Cansu", "Tina wait.");
                Invoke(nameof(ClearDialoguePanel), 1.5f);
                tina.TinaSit(true);
                tina.shouldFollowCansu = false;
                tinaWaiting = true;   
            }
            else if(PlayerPrefs.GetString("CurrentQuest") == "Introduce yourself." && NPCName == "Ish")
            {
                NPCEventTrigger(interactedObject.GetComponent<DialogueTrigger>());
            }
            else if(PlayerPrefs.GetString("CurrentQuest") == "Drink with him." && NPCName == "Ish")
            {
                //ChangeButtonText(speakLable);
                dialogueManager.DisplayDialogue("Cansu", "I got us beers. No need to thank let's drink in silence");
                interactedObject.GetComponent<CircleCollider2D>().enabled = false;
                quest.QuestCompleted();
            }
            else
            {
                NPCDialogueTrigger(interactedObject.GetComponent<DialogueTrigger>());
            }
        }
        else if(interactableName != null) //Interacting with object
        {
            switch(interactableName)
            {
                case "Mirror":
                    dialogue = "“Thou art the fairest in the land.” said the mirror.";
                    break;
                case "Bed":
                    if(PlayerPrefs.GetInt("IsNightTime") == 1)
                    {
                        if(PlayerPrefs.GetString("CurrentQuest") ==  "Go to the dentist.")
                        {
                            dialogue = "She needs some milk!\nOr dentist dunno which.";
                        }
                        else
                        {
                            gameManager.EnableInteractButton(false);

                            dialogue = "“Who texted me goodnight? Nobody” thought she with a gloomy face.";
                            switch(PlayerPrefs.GetInt("Day", 1))
                            {
                                case 1:
                                case 2:
                                case 6:
                                    quest.QuestCompleted();
                                    Invoke(nameof(LoadIndoorSceneFromBed), 1.5f);
                                    break;
                                case 3:
                                    quest.QuestCompleted();
                                    dreamSceneName = "SpaceRescueScene";
                                    Invoke(nameof(LoadDreamScene), 1.5f);
                                    break;
                                case 4:
                                    quest.QuestCompleted();
                                    dreamSceneName = "SpaceShooterScene";
                                    Invoke(nameof(LoadDreamScene), 1.5f);
                                    break;
                                case 5:
                                    quest.QuestCompleted();
                                    dreamSceneName = "MazeScene";
                                    Invoke(nameof(LoadDreamScene), 1.5f);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        dialogue = "“I will sleep when I'm back” She thought.";
                    }
                    break;
                case "CoffeeMachine":
                    if(PlayerPrefs.GetString("CurrentQuest") == "Make coffee.")
                    {
                        interactedObject.GetComponent<BoxCollider2D>().enabled = false;
                        ChangeButtonText(interactLable);
                        dialogue = "“Yummy.” Cansu said,\nrelatively in peace.";
                        dialogueManager.DisplayDialogue("NARRATOR", dialogue);
                        quest.QuestCompleted();
                    }
                    break;
                case "Door":
                    if(PlayerPrefs.GetString("CurrentQuest") != "Make coffee.")
                    {
                        if(PlayerPrefs.GetString("CurrentQuest") != "Go to the dentist.")
                        {
                            if(PlayerPrefs.GetInt("IsNightTime") == 1)
                            {
                                dialogue = "It's too late to go out";
                            }
                            else
                            {
                                dialogue = "“Yayyy! Mommy is taking me for a walk.” said Tina in her merry voice as usual.";
                                tina = FindObjectOfType<Tina>();
                                tina.GoToDoor();
                                interactedObject.GetComponent<Animator>().SetBool("Open", true);
                                interactedObject.GetComponent<BoxCollider2D>().enabled = false;
                            }
                        }
                        else
                        {
                            dialogue = "What a responsible lady.";
                            interactedObject.GetComponent<Animator>().SetBool("Open", true);
                            interactedObject.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                    break;
                case "BuildingDoor":
                    switch(PlayerPrefs.GetString("CurrentQuest"))
                    {
                        case "Take Tina for a walk.":
                            dialogue = "We don't want her to shit on the carpet.\nDo we?";
                            break;
                        case "Pick the shitling's shit up":
                            dialogue = "You left her shit on the park.";
                            break;
                        case "Buy treat for training.":
                        case "Buy beer.":
                            dialogue = "This is not the market\nYou lazy fuck!";
                            break;
                        case "Drink with him.":
                            dialogue = "He is at the park.\nGo to him. NOW!";
                            break;
                        case "Teach Tina to sit.":
                        case "Teach Tina to wait/come.":
                        case "Teach Tina to fetch.":
                            dialogue = "Don't you run away “PLAYER”.\nYou gotta train her.";
                            break;
                        default:
                            gameManager.LoadIndoorScene("Outdoor");
                            return;
                    }
                    break;
                case "ParkSign":
                    dialogue = "Gulizar Yurtseven Park";
                    break;
                case "Poop(Clone)":
                    dialogue = "Gross";
                    quest.QuestCompleted();
                    interactedObject.SetActive(false);
                    Invoke(nameof(ClearDialoguePanel), 2f);
                    break;
                case "Market":
                    if(PlayerPrefs.GetString("CurrentQuest") == "Buy beer.")
                    {
                        dialogue = "She got the beers and started walking towards the park";
                    }
                    else
                    {
                        dialogue = "Cansu has bought some treats to teach Tina tricks.";
                    }
                    quest.QuestCompleted();
                    interactedObject.GetComponent<BoxCollider2D>().enabled = false;
                    break;
                case "AlienScreen":
                case "Module1":
                case "Module2":
                    dialogue = "Hard to read but probably\n“Ne mutlu Türk'üm diyene”\nshe said mockingly.";
                    break;
                case "HumanContainer":
                    dialogue = "No not Inception!";
                    break;
                case "Panel":
                    gameManager.WireObjectsActivation(true);
                    return;
                case "SpaceCraft":
                    gameManager.LoadIndoorScene("Dream");
                    return;
                case "DreamBuildingDoor":
                    gameManager.EnterMaze();
                    return;
                default:
                    dialogue = null;
                    break;
            }

            dialogueManager.DisplayDialogue("NARRATOR", dialogue);
        }
    }

    private void LoadIndoorSceneFromBed()
    {
        PlayerPrefs.SetInt("Day", PlayerPrefs.GetInt("Day", 1) + 1);
        gameManager.LoadIndoorScene("IndoorSleep");
    }

    private void LoadDreamScene()
    {
        PlayerPrefs.SetInt("Day", PlayerPrefs.GetInt("Day", 1) + 1);
        gameManager.LoadDreamScene(dreamSceneName);
    }

    private void NPCDialogueTrigger(DialogueTrigger dialogueTrigger)
    {
        if(dialogueTrigger.dialogue.AtDialogue)
        {
            dialogueManager.DisplayNextSentence();
        }
        else
        {
            dialogueTrigger.TriggerDialogue();
        }
    }

    private void NPCEventTrigger(DialogueTrigger dialogueTrigger)
    {
        if(dialogueTrigger.dialogue.AtDialogue)
        {
            dialogueManager.DisplayNextSentence();
        }
        else
        {
            dialogueTrigger.TriggerEventDialogue();
        }
    }

    private void EmptyInteraction()
    {
        int randomDialogue = Random.Range(0, 3);                
            
        switch(randomDialogue)
        {
            case 0:
                dialogue = "DO NOT PUSH ME!";
                break;
            case 1:
                dialogue = "THAT TICKLES!";
                break;
            case 2:
                dialogue = "OHH STOP IT.";
                break;
        }

        dialogueManager.DisplayDialogue("NARRATOR", dialogue);

        Invoke(nameof(ClearDialoguePanel), 2f);
    }
}