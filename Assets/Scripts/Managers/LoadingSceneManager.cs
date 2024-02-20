using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    int textCounter;
    private string[] text = new string[3];
    private string[] startingText = new string[3]{"Becoming a Cansu...", "Making the player graceful...", "Smoothing the last flaws..."};
    private string[] inOutText = new string[3]{"Opening the door...", "Walking the stairs...", "Taking a breath..."};
    private string[] returnMenuText = new string[3]{"Player leaving...","NPCs crying in ruski...", "Making sure NPCs are OK..."};
    private string[] dentistText = new string[3]{"Cansu going to the dentist...","It doesn't hurt at all...", "She is feeling much better..."};
    private string[] sleepingText = new string[3]{"Cansu closing her eyes...","zzZZzZzZzzZzz...", "more zzZZzZzZzzZzz..."};
    private string[] afterDreamText = new string[3]{"What was that?","Processing the weird dream...", "Getting up..."};
    private string[] endGameText = new string[3]{"This was just the beginning...","Our story is getting longer...", "To Be Continued..."};

    void Start()
    {
        textCounter = 0;

        switch(PlayerPrefs.GetString("SceneToLoad"))
        {
            case "IndoorScene":
                switch(PlayerPrefs.GetString("IndoorLoadingFrom"))
                {
                    case "Menu":
                        text = startingText;
                        break;
                    case "Outdoor":
                        text = inOutText;
                        break;
                    case "IndoorDentist":
                        text = dentistText;
                        break;
                    case "IndoorSleep":
                        text = sleepingText;
                        break;
                    case "Dream":
                        text = afterDreamText;
                        break;
                }
                break;
            case "OutdoorScene":
                text = inOutText;
                break;
            case "MainMenu":
                if(PlayerPrefs.GetInt("EndGame", 0) == 0)
                {
                    text = returnMenuText;
                }
                else
                {
                    text = endGameText;
                }
                break;
            case "SpaceRescueScene":
            case "SpaceShooterScene":
            case "MazeScene":
                text = sleepingText;
                break;
        }

        DisplayLoadingText();
        Invoke("DisplayLoadingText", 1f);
        Invoke("DisplayLoadingText", 2f);
        Invoke("LoadScene", 3f);
    }

    private void DisplayLoadingText()
    {
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(text[textCounter]));

        textCounter++;
    }

    IEnumerator TypeDialogue (string dialogue)
    {
        loadingText.text = "";

        foreach(char letter in dialogue.ToCharArray())
        {
            loadingText.text += letter;
            yield return null;
            //yield return null;
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync(PlayerPrefs.GetString("SceneToLoad"));
    }
}