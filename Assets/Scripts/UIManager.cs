using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ICubeMergeObserver
{
    //Set In Inspector
    [SerializeField] private GameruleManager gameruleManager;
    [SerializeField] private MergeCubesEventManager mergeCubesEventManager;
    [SerializeField] private ScorePanel scorePanel;
    [SerializeField] private GameObject startButton;
    [SerializeField] private Image grayScreen;
    [SerializeField] private Button restartButton;

    private int xCordForMoveAwayRestartTools = 10000;
    // Start is called before the first frame update
    void Start()
    {
        //Enable score panel
        SubscribeToCubesMerge();
        scorePanel.enabled = true;
        RemoveStartButton();
    }

    //Remove Start Button from screen
    public void RemoveStartButton()
    {
        Destroy(startButton);
    }

    //Function for showing death screen to player
    public void EndGame()
    {
        grayScreen.transform.localPosition = new(0, grayScreen.transform.localPosition.y, grayScreen.transform.localPosition.z);
        restartButton.transform.localPosition = new(0, restartButton.transform.localPosition.y, restartButton.transform.localPosition.z);
        StartCoroutine(nameof(EndGameAnimation));
    }

    //Animation of appearing death screen
    private IEnumerator EndGameAnimation()
    {
        for (int transparencyValue = 0; transparencyValue <= 50; transparencyValue++)
        {
            grayScreen.color = new(1, 1, 1, transparencyValue / 100f);
            restartButton.GetComponent<Image>().color = new(1, 1, 1, transparencyValue / 50f);
            restartButton.GetComponentInChildren<TMP_Text>().color = new(1, 1, 1, transparencyValue / 50f);
            yield return new WaitForEndOfFrame();
        }
    }

    //Event handler for click on restart button
    public void ClickOnRestartButton()
    {
        grayScreen.color = new(1, 1, 1, 0);
        restartButton.GetComponent<Image>().color = new(1, 1, 1, 0);
        restartButton.GetComponentInChildren<TMP_Text>().color = new(1, 1, 1, 0);

        grayScreen.transform.localPosition = new(xCordForMoveAwayRestartTools, grayScreen.transform.localPosition.y, grayScreen.transform.localPosition.z);
        restartButton.transform.localPosition = new(xCordForMoveAwayRestartTools, restartButton.transform.localPosition.y, restartButton.transform.localPosition.z);

        gameruleManager.RestartGame();
        ResetScorePanel();
    }

    //Reset a score panel
    public void ResetScorePanel()
    {
        scorePanel.Reset();
    }

    //Subscribe to event of cubes merge
    public void SubscribeToCubesMerge()
    {
        mergeCubesEventManager.AddObserver(this);
    }


    //Event handler for cubes merge
    public void OnCubeMerge(CubeCollisionToMerge cubeMerge)
    {
        scorePanel.OnCubeMerge(cubeMerge);
    }
}
