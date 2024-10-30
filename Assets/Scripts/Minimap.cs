using UnityEngine;

public class Minimap : MonoBehaviour
{
    //private fields
    private GameObject minimapWindow;
    private GameObject openButton;

    void Start()
    {
        minimapWindow = transform.GetChild(0).gameObject;
        openButton = transform.GetChild(1).gameObject;

        ActivateMinimap(PlayerPrefs.GetInt("MinimapOpen", 1) == 1);
    }

    private void ActivateMinimap(bool isMinimapOpen)
    {
        minimapWindow.SetActive(isMinimapOpen);
        openButton.SetActive(!isMinimapOpen);
    }

    public void MinimapButton()
    {
        bool isMinimapOpen = PlayerPrefs.GetInt("MinimapOpen", 1) != 1;

        ActivateMinimap(isMinimapOpen);

        PlayerPrefs.SetInt("MinimapOpen", isMinimapOpen ? 1 : 0);
    }
}