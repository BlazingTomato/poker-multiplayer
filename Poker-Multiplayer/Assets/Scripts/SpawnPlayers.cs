using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    
    public GameObject player;
    public GameObject newPlayer;

    [SerializeField] Canvas canvas;


    private void Start(){

        newPlayer = PhotonNetwork.Instantiate(player.name, new Vector2(0,0), Quaternion.identity);
        newPlayer.transform.SetParent (canvas.transform, false);
        player.transform.localPosition = new Vector3(0,0,0);
        //player.transform.localRotation = new Vector3(0,0,0);
    }


}
