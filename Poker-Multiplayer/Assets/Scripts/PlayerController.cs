using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{

    PhotonView view;
    public GameObject[] cards;
    public int money;
    public bool hasFolded;
    public bool isTurn;
    [SerializeField] GameObject canvas;
    public GameObject[] spawnPoints;

    private void Start() {
        canvas = GameObject.FindGameObjectWithTag("Background");
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        view = GetComponent<PhotonView>();
        this.transform.SetParent(canvas.transform, false);
        Vector2 position = spawnPoints[PhotonNetwork.CountOfPlayers - 1].transform.position;
        this.GetComponent<Transform>().position = position;
    }

    private void Update() {
        if(view.IsMine){
            
        }
    }


}
