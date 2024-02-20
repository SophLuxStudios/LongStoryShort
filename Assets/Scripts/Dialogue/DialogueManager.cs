using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //used Class'
    private PlayerInteract playerInteract;
    private Quest quest;
    private GameManager gameManager;

    //private fields
    private Queue<string> sentences;
    private Animator animator;
    private bool isIndoorScene;
    [SerializeField] private bool isReadyToMove;

    //public variables
    public bool isEventOn;
    public bool isDisplayingNarratorDialogue;
    public Dialogue currentDialogue;
    
    //UI
    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI buttonText;

    void Awake()
    {
        characterNameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        dialogueText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        animator = gameObject.GetComponent<Animator>();
        playerInteract = FindObjectOfType<PlayerInteract>();
        quest = FindObjectOfType<Quest>();
        sentences = new Queue<string>();

        isIndoorScene = gameManager.IsIndoorScene();
    }

    public void DisplayDialogue(string speaker, string dialogue)
    {
        OpenDialogueBox(true);

        characterNameText.text = speaker + ":";

        CancelInvoke();
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(dialogue));
    }

    private void ClearDialogueDisplay()
    {
        characterNameText.text = "";
        dialogueText.text = "";

        OpenDialogueBox(false);

        isDisplayingNarratorDialogue = false;
    }

    public void InvokeClearDisplay(float seconds)
    {
        Invoke("ClearDialogueDisplay", seconds);
    }

    IEnumerator TypeDialogue (string dialogue)
    {
        dialogueText.text = "";

        foreach(char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void StartNPCDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        currentDialogue = dialogue;
        currentDialogue.atDialogue = true;

        playerInteract.NPCDialogueStarted();

        int dialogueIndex = Random.Range(0,3);

        switch(dialogueIndex)
        {
            case 0:
                foreach(string sentence in dialogue.sentences0)
                {
                    dialogue.nPCSpeaks = true;
                    sentences.Enqueue(sentence);
                }
                break;
            case 1:
                foreach(string sentence in dialogue.sentences1)
                {
                    dialogue.nPCSpeaks = false;
                    sentences.Enqueue(sentence);
                }
                break;
            case 2:
                foreach(string sentence in dialogue.sentences2)
                {
                    dialogue.nPCSpeaks = true;
                    sentences.Enqueue(sentence);
                }
                break;
        }

        DisplayNextSentence();
    }

    public void StartEventDialogue(Dialogue dialogue)
    {
        isEventOn = true;

        sentences.Clear();

        currentDialogue = dialogue;
        currentDialogue.atDialogue = true;

        playerInteract.NPCDialogueStarted();

        foreach(string sentence in dialogue.eventSentences)
        {
            dialogue.nPCSpeaks = false;
            sentences.Enqueue(sentence);
        }

        if(gameManager.IsSpaceRescueScene())
        {
            playerInteract.isDreamDialogue = true;
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            isReadyToMove = true;
            if(isEventOn)
            {
                EndNPCEventDialogue();
                if(PlayerPrefs.GetString("CurrentQuest") == "Introduce yourself.")
                {
                    gameManager.EnableMovement(true);
                    quest.QuestCompleted();
                }
            }
            else
            {
                if(gameManager.IsMazeScene())
                {
                    FindObjectOfType<Dissolve>().StartDissolving();
                    Debug.Log("Dissolving started.");
                }

                EndNPCDialogue();
            }
            return;
        }
        else if(sentences.Count == 1)
        {
            buttonText.text = "END";
        }

        string sentence = sentences.Dequeue();
        
        if(currentDialogue.nPCSpeaks)
        {
            DisplayDialogue(currentDialogue.name, sentence);

            currentDialogue.nPCSpeaks = false;
        }
        else
        {
            DisplayDialogue("Cansu", sentence);

            currentDialogue.nPCSpeaks = true;
        }
    }

    private void EndNPCDialogue()
    {
        currentDialogue.atDialogue = false;
        currentDialogue = null;

        Debug.Log("DialogueManager.EndNPCDialogue() has run");

        playerInteract.NPCDialogueEnded();

        if(gameManager.IsSpaceRescueScene())
        {
            playerInteract.isDreamDialogue = false;
        }
    }

    private void EndNPCEventDialogue()
    {
        currentDialogue.atDialogue = false;
        currentDialogue = null;

        if(gameManager.IsSpaceRescueScene())
        {
            playerInteract.isDreamDialogue = false;
            gameManager.EnableMovement(true);
            InvokeClearDisplay(1.5f);
        }

        playerInteract.NPCEventDialogueEnded();
    }

    public void StartNarratorDialogue(Dialogue dialogue, bool isEvent)
    {
        isReadyToMove = false;

        sentences.Clear();

        currentDialogue = dialogue;
        currentDialogue.atDialogue = true;

        buttonText.text = "NEXT";
        playerInteract.isNarratorDialogue = true;

        gameManager.EnableMovement(false);

        int dialogueIndex = Random.Range(0,8);

        if(!isEvent)
        {
            switch(dialogueIndex)
            {
                case 0://37.5 percent chance
                case 1:
                case 2:
                    if(isIndoorScene)
                    {
                        quest.GiveIndoorQuest(false);
                    }
                    foreach(string sentence in dialogue.sentences0)
                    {
                        dialogue.nPCSpeaks = true;
                        sentences.Enqueue(sentence);
                    }
                    break;
                case 3://37.5 percent chance
                case 4:
                case 5:
                    if(isIndoorScene)
                    {
                        quest.GiveIndoorQuest(false);
                    }
                    foreach(string sentence in dialogue.sentences1)
                    {
                        dialogue.nPCSpeaks = false;
                        sentences.Enqueue(sentence);
                    }
                    break;
                case 6:
                case 7://25 percent chance
                    if(isIndoorScene)
                    {
                        quest.GiveIndoorQuest(true);
                    }
                    foreach(string sentence in dialogue.sentences2)
                    {

                        dialogue.nPCSpeaks = true;
                        sentences.Enqueue(sentence);
                    }
                    break;
            }
        }
        else
        {
            if(PlayerPrefs.GetInt("CansuReturningHome") == 1)
            {
                quest.GiveIndoorQuest(false);
            }
            
            foreach(string sentence in dialogue.eventSentences)
            {
                dialogue.nPCSpeaks = true;
                sentences.Enqueue(sentence);
            }
        }

        DisplayNextNarratorSentence();
    }

    public void DisplayNextNarratorSentence()
    {
        isDisplayingNarratorDialogue = true;

        if(sentences.Count == 0)
        {
            //if it's the first time tina arrives
            if(PlayerPrefs.GetInt("FirstNarratorDialogue", 1) == 1 && isIndoorScene)
            {
                gameManager.TinaGoesDoorToKitchen();
                quest.GiveIndoorQuest(false);
                PlayerPrefs.SetInt("FirstNarratorDialogue", 0);
            }
            else if(PlayerPrefs.GetInt("Day") == 7 && PlayerPrefs.GetInt("IsNightTime") == 1)//if it's the Birthday surprise
            {
                gameManager.EndGame();
            }

            currentDialogue.atDialogue = false;
            currentDialogue = null;

            buttonText.text = "INTERACT";
            playerInteract.isNarratorDialogue = false;

            isReadyToMove = true;

            gameManager.EnableMovement(true);

            return;
        }
        else if(sentences.Count == 1)
        {
            buttonText.text = "END";
        }

        string sentence = sentences.Dequeue();

        DisplayDialogue(currentDialogue.name, sentence);
    }

    public bool ReadyToMove()
    {
        return isReadyToMove;
    }

    private void OpenDialogueBox(bool open)
    {
        if(gameManager.IsOutdoorScene() || gameManager.IsSpaceRescueScene() || gameManager.IsMazeScene())
        {
            animator.SetBool("isOpen", open);
        }
    }
}