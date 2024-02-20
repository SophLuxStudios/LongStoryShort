using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance;

    //used Class'
    private PlayerInteract playerInteract;
    private DialogueManager dialogueManager;

    //private fields
    [SerializeField] private GameObject cansu;
    [SerializeField] private GameObject tina;
    [SerializeField] private GameObject whisper;
    [SerializeField] private GameObject windowLights;
    [SerializeField] private GameObject ishAndRavenPark;
    [SerializeField] private GameObject snowEffect;
    [SerializeField] private GameObject NPC;
    [SerializeField] private GameObject NPCBirthday;
    [SerializeField] private GameObject wireObjects;
    [SerializeField] private Animator laserGateAnimator;
    [SerializeField] private GameObject laserGateCollider;
    [SerializeField] private GameObject alienRoadBlock;
    [SerializeField] private GameObject blackFigure;
    [SerializeField] private BoxCollider2D panelCollider;
    private Vector2 cansuRoomPosition = new Vector2(-1.1f, -1f);
    private Vector2 cansuDoorPosition = new Vector2(2.8f, 6f);
    private Vector2 tinaComingHomePosition = new Vector2(2.1f, 6f);
    private Vector2 mazeStartingPosition = new Vector2(5, -5.5f);
    [SerializeField] GameObject mazeVCam;
    [SerializeField] GameObject miniMap;
    private string sceneName;

    //public
    public bool isTinaInHerRoom;

    //UI
    [SerializeField] private GameObject leftStick;
    [SerializeField] private GameObject interactButton;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            Instance = this;
        }

        sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "IndoorScene")
        {
            PlayerPrefs.SetInt("NewGame", 0);
            
            if(PlayerPrefs.GetInt("IsNightTime", 0) == 1)
            {
                //Turn the lights out
                windowLights.SetActive(false);

                if(PlayerPrefs.GetInt("Day") == 7)
                {
                    NPC.SetActive(false);
                    NPCBirthday.SetActive(true);
                }
            }

            //Set cansu position depending on returning from outside or waking up
            if(PlayerPrefs.GetInt("CansuReturningHome", 0) == 0)
            {
                cansu.transform.position = cansuRoomPosition;

                //Set tina deactive if it's the first time
                if(PlayerPrefs.GetInt("FirstNarratorDialogue", 1) == 1)
                {
                    tina.transform.position = tinaComingHomePosition;
                    tina.GetComponent<CircleCollider2D>().enabled = false;
                    tina.SetActive(false);
                }
                else if(PlayerPrefs.GetInt("Day") != 7)
                {
                    //Randomize locations only if Cansu is in her room
                    RandomizePetLocations();
                }
            }
            else
            {
                cansu.transform.position = cansuDoorPosition;
                
                ///if it's not the third day Tina always returns with Cansu
                //if it is the third day Tina only returns with her if dentist is not visited
                if(PlayerPrefs.GetInt("Day", 1) != 3 || PlayerPrefs.GetInt("GoneToTheDentist", 0) == 0)
                {
                    tina.transform.position = tinaComingHomePosition;
                }
            }

            PlayerPrefs.SetInt("ContinueFromOutdoor", 0);
            PlayerPrefs.SetInt("ContinueFromDay", PlayerPrefs.GetInt("Day", 1));
        }
        else if(sceneName == "OutdoorScene")
        {
            if(PlayerPrefs.GetInt("Day", 1) == 5 || PlayerPrefs.GetInt("Day", 1) == 6)
            {
                ishAndRavenPark.SetActive(true);
            }
            else if(PlayerPrefs.GetInt("Day", 1) == 7)
            {
                snowEffect.SetActive(true);
            }

            PlayerPrefs.SetInt("ContinueFromOutdoor", 1);
            PlayerPrefs.SetInt("ContinueFromDay", PlayerPrefs.GetInt("Day", 1));
        }
        else if(sceneName == "SpaceRescueScene")
        {
            AudioManager.Instance.Stop("ThemeSong");
            AudioManager.Instance.Play("SpaceRescueMusic");
        }
        else if(sceneName == "SpaceShooterScene")
        {
            AudioManager.Instance.Stop("ThemeSong");
            AudioManager.Instance.Play("SpaceShooterMusic");
        }
        else if(sceneName == "MazeScene")
        {
            AudioManager.Instance.Stop("ThemeSong");
            AudioManager.Instance.Play("MazeMusic");
        }
    }

    private void RandomizePetLocations()
    {
        //Randomize pet locations (20% chance tina in Cansu's room)
        int locationIndex = Random.Range(1,6);

        if(locationIndex == 5)
        {
            isTinaInHerRoom = true;
  
            tina.transform.position = new Vector2(.4f, -.2f);
            whisper.transform.position = new Vector2(-3.53f, 4.87f);
        }
    }

    void Start()
    {
        playerInteract = FindObjectOfType<PlayerInteract>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        
        if(sceneName != "SpaceShooterScene")
        {
            playerInteract.isTinaEvent = isTinaInHerRoom;
        }

        if(PlayerPrefs.GetInt("CansuReturningHome") == 1 && sceneName == "IndoorScene")
        {
            Invoke("TinaGoesDoorToKitchen", .5f);
        }
    }

    public void TinaGoesDoorToKitchen()
    {
        tina.SetActive(true);
        tina.GetComponent<Tina>().GoToKitchenFromDoor();
    }

    public void WireObjectsActivation(bool activation)
    {
        //Camera.main.enabled = !activation;
        wireObjects.SetActive(activation);

        if(!activation)
        {
            laserGateAnimator.SetBool("isOpen", true);
            laserGateCollider.SetActive(false);
            panelCollider.enabled = false;

            //Move Camera and activate road block
            Invoke("SpaceRescueMoveCamera", .5f);
        }
    }

    private void SpaceRescueMoveCamera()
    {
        //Camera.main.transform.position = new Vector3(1, 20.5f, -10);

        alienRoadBlock.SetActive(true);
        blackFigure.GetComponent<BlackFigure>().StartWalking();
        StartCoroutine(SmoothLerp(2f));
    }

    private IEnumerator SmoothLerp(float time)
    {
        Vector3 startingPos  = Camera.main.transform.position;
        Vector3 finalPos = new Vector3(1, 20.5f, -10);
        float elapsedTime = 0;
         
        while(elapsedTime < time)
        {
            Camera.main.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void EnterMaze()
    {
        //close minimap

        cansu.transform.position = mazeStartingPosition;
        mazeVCam.SetActive(true);
        miniMap.SetActive(false);
        cansu.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void EndGame()
    {
        int isMuted = PlayerPrefs.GetInt("IsMuted");

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("IsMuted", isMuted);

        PlayerPrefs.SetInt("EndGame", 1);
        PlayerPrefs.SetString("SceneToLoad", "MainMenu");
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    public void LoadIndoorScene(string from)
    {
        //if not loaded from sleep it is night time
        //PlayerPrefs.SetInt("IsNightTime", from != "IndoorSleep" ? 1 : 0);

        switch(from)
        {
            case "IndoorSleep":
                PlayerPrefs.SetInt("IsNightTime", 0);
                break;
            case "Dream":
                PlayerPrefs.SetInt("IsNightTime", 0);
                AudioManager.Instance.Stop("SpaceRescueMusic");
                AudioManager.Instance.Stop("SpaceShooterMusic");
                AudioManager.Instance.Stop("MazeMusic");
                AudioManager.Instance.Play("ThemeSong");
                break;
            default:
                PlayerPrefs.SetInt("IsNightTime", 1);
                break;
        }

        PlayerPrefs.SetString("IndoorLoadingFrom", from);
        PlayerPrefs.SetString("SceneToLoad", "IndoorScene");
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    public void LoadDreamScene(string dreamSceneName)
    {
        PlayerPrefs.SetString("SceneToLoad", dreamSceneName);
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    public void LoadOutdoorScene()
    {
        PlayerPrefs.SetInt("CansuReturningHome", 1);

        PlayerPrefs.SetString("SceneToLoad", "OutdoorScene");
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    public void LoadMainMenu()
    {
        PlayerPrefs.SetString("SceneToLoad", "MainMenu");
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    public void EnableMovement(bool isEnable)
    {
        leftStick.GetComponent<OnScreenStick>().enabled = isEnable;
    }
    public void EnableInteractButton(bool isEnable)
    {
        interactButton.GetComponent<OnScreenButton>().enabled = isEnable;
        interactButton.GetComponent<Button>().enabled = isEnable;
    }
    public void EnableInputs(bool isEnable)
    {
        leftStick.GetComponent<OnScreenStick>().enabled = isEnable;
        interactButton.GetComponent<OnScreenButton>().enabled = isEnable;
        interactButton.GetComponent<Button>().enabled = isEnable;
    }

    public void EndAllEvents()
    {
        playerInteract.isTinaEvent = false;
        dialogueManager.isEventOn = false;
    }

    public bool IsIndoorScene()
    {
        return sceneName == "IndoorScene" ? true : false;
    }

    public bool IsOutdoorScene()
    {
        return sceneName == "OutdoorScene" ? true : false;
    }

    public bool IsSpaceRescueScene()
    {
        return sceneName == "SpaceRescueScene" ? true : false;
    }

    public bool IsMazeScene()
    {
        return sceneName == "MazeScene" ? true : false;
    }
}