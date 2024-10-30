using UnityEngine;

public class AlienObjects : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] glyphSR = new SpriteRenderer[3];
    
    void Awake()
    {
        for(int i = 0; i < glyphSR.Length; i++)
        {
            glyphSR[i] = transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
        InvokeRepeating(nameof(ChangeGlyphsX), 0.5f, 1f);
        InvokeRepeating(nameof(ChangeGlyphsY), 1f, 1f);
    }

    private void ChangeGlyphsX()
    {
        for(int i = 0; i < glyphSR.Length; i++)
        {
            glyphSR[i].flipX = !glyphSR[i].flipX;
        }
    }
    private void ChangeGlyphsY()
    {
        for(int i = 0; i < glyphSR.Length; i++)
        {
            glyphSR[i].flipY = !glyphSR[i].flipY;
        }
    }
}