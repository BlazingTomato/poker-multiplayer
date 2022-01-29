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

    private void Start() {
        view = GetComponent<PhotonView>();
    }

    private void Update() {
        if(view.IsMine){
            
        }
    }


}
