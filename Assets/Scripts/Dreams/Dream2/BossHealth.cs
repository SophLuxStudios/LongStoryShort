using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    private Slider bossHealtBar;

    void Start()
    {
        bossHealtBar = GetComponent<Slider>();
    }

    public void DecreaseBossHealth()
    {
        bossHealtBar.value--;
    }
}