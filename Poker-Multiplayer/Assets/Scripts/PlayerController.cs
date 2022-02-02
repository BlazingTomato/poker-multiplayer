using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{

    [Header("Initialization")] 
    [SerializeField] GameObject canvas;
    public GameObject[] spawnPoints;
    PhotonView view;

    [Header("Player Info")]
    public int money;
    public bool hasFolded;
    public bool isTurn;
    [SerializeField] GameObject TimerImage;
    float turnTime  = 10f;


    private void Start() {
        canvas = GameObject.FindGameObjectWithTag("Background");
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        view = GetComponent<PhotonView>();
        this.transform.SetParent(canvas.transform, false);

        Vector2 position;

        if(PhotonNetwork.IsMasterClient){
            position = spawnPoints[0].transform.position;
        }
        else{
            position = spawnPoints[PhotonNetwork.PlayerList.Length - 1].transform.position;
        }
        
        this.GetComponent<Transform>().position = position;
    }

    public void StartGame(){
        money = 1000;
        hasFolded = false;
        isTurn = false;
    }

    public void setTurn(){
        isTurn = true;
    }

    private void Update() {
        if(!view.IsMine){
            return;
        }

        if(hasFolded){
            return;
        }

        if(!isTurn){
            return;
        }

        UpdateTimer();

    }

    void UpdateTimer(){
        turnTime -= Time.deltaTime;
        
        if(turnTime == 0){
            if(isTurn){
                isTurn = false;
                hasFolded = true;
            }
        }
        else{
            TimerImage.GetComponent<Image>().fillAmount = turnTime/10f;
        }
    }

    public void Check(){
        
    }
    public void Bet(){

    }

    public void Fold(){

    }


    
    

}
