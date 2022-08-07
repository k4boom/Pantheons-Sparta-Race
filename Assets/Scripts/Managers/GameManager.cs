using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField] public List<Transform> players;
    [SerializeField] private Text tm;
    [SerializeField] private Canvas menu;
    [SerializeField] private Canvas inGameUI;
    [SerializeField] private GameObject opponentGroup;
    [SerializeField] public Transform finishLine;
    public int sliderValue;
    [SerializeField] private NavMeshSurface nms;
    [SerializeField] private NavMeshSurface nms2;
    [SerializeField] private DrawManager drawManager;

    private void Awake()
    {
        inGameUI.enabled = false;
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        foreach(Transform child in opponentGroup.transform)
        {
            players.Add(child);
        }

        for(int i=1; i< players.Count; i++)
        {
            players[i].gameObject.GetComponent<OpponentController>().SetTargetVector(instance.finishLine);
        }
        nms.BuildNavMesh();
        nms2.BuildNavMesh();
    }


    public int FindRanking()
    {
        var ranking = players.OrderBy(tr => tr.position.x).ToList();
        int index = ranking.FindIndex(tr => tr == players[0]);
        tm.text = "Rank: " + (index + 1).ToString();      
        return index + 1;
    }

    public void OutputPercentage()
    {
        tm.text = "You are " + Globals.instance.paintedPercentage + "% Pantheon Crew";
    }

    public void ToggleMenuCanvas()
    {
        menu.enabled = !menu.enabled;
        inGameUI.enabled = !inGameUI.enabled;
        menu.GetComponent<Menu>().onMenu = !menu.GetComponent<Menu>().onMenu;
        
    }

    public void RestartGame()
    {
        players[0].gameObject.GetComponent<PlayerMovement>().StartOver();
        for (int i =1; i <players.Count; i++)
        {
            players[i].gameObject.GetComponent<OpponentController>().StartOver();
        }
        drawManager.board.StartOver();
        ToggleMenuCanvas();
        Time.timeScale = 0;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}


