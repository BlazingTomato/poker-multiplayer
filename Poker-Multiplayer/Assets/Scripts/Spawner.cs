using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Spawner : MonoBehaviour
{
    
    public Transform[] spawnPoints;
    public Dealer dealer;
    public GameObject[] dealerCards = new GameObject[5];
    public float startTimeBtwDeal;
    float timeBtwCards;
    public GameObject card;


    int i = 0;

    private void Update(){
        if(timeBtwCards <= 0){
            Vector3 spawnPosition = spawnPoints[i].position;
            Vector3 spawnScale = spawnPoints[i].localScale;
            dealerCards[i] = PhotonNetwork.Instantiate(card.name, spawnPosition, Quaternion.identity);
            dealerCards[i].transform.localScale = spawnScale;
        }else{
            timeBtwCards -= Time.deltaTime;
        }
    }


}
