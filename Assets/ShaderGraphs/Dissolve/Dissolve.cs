using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private Material material;
    private bool isDissolving;
    private float fade = 1;

    void Update()
    {
        if(isDissolving)
        {
            fade -= (Time.deltaTime) / 1.6f;
            Debug.Log("Fade is: " + fade + "\nTime is: " + Time.deltaTime);

            if(fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
                
                Invoke("WakeUpFromDream", 0.2f);
            }

            material.SetFloat("_Fade", fade);
        }
    }

    private void WakeUpFromDream()
    {
        GameManager.Instance.LoadIndoorScene("Dream");
    }

    public void StartDissolving()
    {
        GetComponent<SpriteRenderer>().material = material;
        isDissolving = true;
    }
}