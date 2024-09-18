using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(AppearanceAnimation));
    }

    //Animation of appearance of score panel
    private IEnumerator AppearanceAnimation()
    {
        TMP_Text text = GetComponentInChildren<TMP_Text>();
        Image textPanel = GetComponentInChildren<Image>();
        for(int transparencyValue = 0; transparencyValue <= 100; transparencyValue++)
        {
            text.color = new(1, 1, 1, transparencyValue / 100f);
            textPanel.color = new(1, 1, 1, transparencyValue / 100f);
            yield return new WaitForEndOfFrame();
        }
    } 

    //Event handler for cubes merge
    public void OnCubeMerge(CubeCollisionToMerge cubeMerge)
    {
        score += cubeMerge.ScoreToGet;
        GetComponentInChildren<TMP_Text>().text = score.ToString();
    }

    //Reset a score panel
    public void Reset()
    {
        score = 0;
        GetComponentInChildren<TMP_Text>().text = score.ToString();
    }
}
