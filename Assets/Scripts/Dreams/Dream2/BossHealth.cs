using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    private Slider bossHealthBar;

    void Start()
    {
        bossHealthBar = GetComponent<Slider>();
    }

    public void DecreaseBossHealth()
    {
        bossHealthBar.value--;
    }
}