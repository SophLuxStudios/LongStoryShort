using UnityEngine;
using UnityEngine.UI;

public class MuteManager : MonoBehaviour
{
    private Toggle muteToggle;

    public void Awake()
    {
        muteToggle = gameObject.GetComponent<Toggle>();
        
        muteToggle.isOn = PlayerPrefs.GetInt("IsMuted") != 1 ;
    }

    public void MutePressed()
    {
        PlayerPrefs.SetInt("IsMuted", muteToggle.isOn == true ? 0 : 1);
        AudioListener.pause = !muteToggle.isOn;
        Debug.Log("IsMuted is: " + !muteToggle.isOn);
    }
}