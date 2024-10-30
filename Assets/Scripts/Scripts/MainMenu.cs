using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startText;
    void Awake()
    {
        if(PlayerPrefs.GetInt("NewGame", 1) == 1)
        {
            startText.text = "START";
        }
        else
        {
            startText.text = "RESUME";
        }
    }
    void Start()
    {
        if(PlayerPrefs.GetInt("EndGame", 0) == 1)
        {
            PlayerPrefs.GetInt("EndGame", 0);
        }

        Debug.Log(PlayerPrefs.GetInt("ContinueFromOutdoor"));
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("IndoorLoadingFrom", "Menu");
        
        if(PlayerPrefs.GetInt("NewGame", 0) == 1)
        {
            PlayerPrefs.SetString("SceneToLoad", "IndoorScene");
        }
        else
        {
            PlayerPrefs.SetInt("Day", PlayerPrefs.GetInt("ContinueFromDay", 1));
            if(PlayerPrefs.GetInt("ContinueFromOutside", 0) == 1)
            {
                PlayerPrefs.SetString("SceneToLoad", "OutdoorScene");
            }
            else
            {
                PlayerPrefs.SetInt("CansuReturningHome", PlayerPrefs.GetInt("IsNightTime", 0));

                PlayerPrefs.SetString("SceneToLoad", "IndoorScene");
            }
        }

        SceneManager.LoadSceneAsync("LoadingScene");
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}