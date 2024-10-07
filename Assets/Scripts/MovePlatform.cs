using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovePlatform : MonoBehaviour
{
    private bool gameStarted = false;

    public List<Transform> waypoints;
    public float duration = 3000f;
    public GameObject textBegin;
    public GameObject gameplayUI;
    public SnapshotController controller;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) && !gameStarted)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        gameStarted = true;

        Vector3[] positions = new Vector3[waypoints.Count];

        int i = 0;
        foreach(Transform current in waypoints)
        {
            positions[i] = current.position;
            i++;
        }

        transform.DOPath(positions, duration, PathType.Linear, PathMode.Full3D, 10, null).OnComplete(
            ()=>{
                Debug.Log("ENDGAME");
                controller.EndGame();
                gameplayUI.SetActive(false);
            }
        );

        textBegin.SetActive(false);
        gameplayUI.SetActive(true);
    }
}
