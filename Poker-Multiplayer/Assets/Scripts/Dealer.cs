using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun; 

public class Dealer : MonoBehaviour
{
    
    [Header("Deck of cards")]
    public List<Sprite> fullDeck;
    [SerializeField] List<Sprite> tempDeck;

    [Header("Deal information")]
    public Transform[] spawnPoints;
    public float startTimeBtwDeal;
    float timeBtwCards = 5f;
    public GameObject card;
    int round = 0;
    bool gameStarted = false;



    

    [Header("Temp Card Values")]
    [SerializeField] int[] valuesTo13;
    [SerializeField] int[] suitesTo13;
    [SerializeField] int[] suites;
    [SerializeField] GameObject[] dealerCards = new GameObject[5];

    [Header("Player Info")]
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] playerCards = new GameObject[2];
    PhotonView view;

    [SerializeField] GameObject textFile;
    Photon.Realtime.Player[] photonPlayers;
    
    
    private void Start() {

        tempDeck = fullDeck;
        valuesTo13 = new int[13];
        suitesTo13 = new int[13];
        suites = new int[5];

        view = GetComponent<PhotonView>();

        view.RPC("GetPlayers", RpcTarget.AllBuffered);

        photonPlayers = PhotonNetwork.PlayerList;

        foreach(Photon.Realtime.Player p in photonPlayers){
            Debug.Log(p);
        }
    }

    
    public void StartGame(){
        view.RPC("StartGameRPC", RpcTarget.All);

        view.RPC("DealCardsToPlayer", RpcTarget.All);
    
    }

        private void Update(){

        if(!PhotonNetwork.IsMasterClient){
            return;
        }

        if(!gameStarted){
            return;
        }

        if(timeBtwCards <= 0){
            timeBtwCards = 5f;
            if(round == 0){
                //Debug.Log("Flop");
                view.RPC("playCard", RpcTarget.AllBuffered, UnityEngine.Random.Range(0,tempDeck.Count), 0);
                view.RPC("playCard", RpcTarget.AllBuffered, UnityEngine.Random.Range(0,tempDeck.Count), 1);
                view.RPC("playCard", RpcTarget.AllBuffered, UnityEngine.Random.Range(0,tempDeck.Count), 2);

                view.RPC("valuesToArray",RpcTarget.AllBuffered, dealerCards[0].GetComponent<Image>().sprite.name);
                view.RPC("valuesToArray",RpcTarget.AllBuffered, dealerCards[1].GetComponent<Image>().sprite.name);
                view.RPC("valuesToArray",RpcTarget.AllBuffered, dealerCards[2].GetComponent<Image>().sprite.name);

                round++;
            }
            else if(round == 1){
                //Debug.Log("River");
                view.RPC("playCard", RpcTarget.AllBuffered, UnityEngine.Random.Range(0,tempDeck.Count), 3);

                view.RPC("valuesToArray",RpcTarget.AllBuffered, dealerCards[3].GetComponent<Image>().sprite.name);

                round++;
            }
            else if(round == 2){
                //Debug.Log("Turn");
                view.RPC("playCard", RpcTarget.AllBuffered, UnityEngine.Random.Range(0,tempDeck.Count), 4);
                view.RPC("valuesToArray",RpcTarget.AllBuffered, dealerCards[4].GetComponent<Image>().sprite.name);
                round++;
            }
            else{
                //Debug.Log("Reset");
                view.RPC("NewRound",RpcTarget.AllBuffered);
            }

            view.RPC("HighestHandText", RpcTarget.All);

        }else{
            timeBtwCards -= Time.deltaTime;
        }
            
        
    }


    [PunRPC]
    void GetPlayers(){
        players = GameObject.FindGameObjectsWithTag("Player");

    }

    [PunRPC]
    void StartGameRPC(){
        gameStarted = true;
    }

    [PunRPC]
    private void NewRound(){
        tempDeck = fullDeck;
        round = 0;
        timeBtwCards = 5f;

        for(int i = 0; i < dealerCards.Length; i++){
            dealerCards[i].GetComponent<Image>().color = new Color(255f,255f,255f,0f);       
        }

        for(int i = 0; i < photonPlayers.Length; i++){
            view.RPC("DealCardsToPlayer", photonPlayers[i]);
        }

        valuesTo13 = new int[13];
        suitesTo13 = new int[13];
        suites = new int[5];
    }

    [PunRPC]
    void DealCardsToPlayer(){
        int card1 = UnityEngine.Random.Range(0,tempDeck.Count);
        view.RPC("discardCard", RpcTarget.AllBuffered, card1);
        int card2 = UnityEngine.Random.Range(0,tempDeck.Count);
        view.RPC("discardCard", RpcTarget.AllBuffered, card2);
    
        Sprite cardSprite1 = tempDeck[card1];
        Sprite cardSprite2 = tempDeck[card2];

        playerCards[0].GetComponent<Image>().sprite = cardSprite1;
        playerCards[1].GetComponent<Image>().sprite = cardSprite2;

        
    }

    [PunRPC]
    void discardCard(int card){
        Sprite currentCard = tempDeck[card];
        tempDeck.Remove(currentCard);
    }

    



    #region cardConversion
    //convert card string to values: starting from 2-10, those are labled as numbers so parse int will work
    //for face cards, take the format and take the beginning to the underscore
    // convert face card string to the corresponding value
    // initalize to the array
    private int CardToValue(string cardName){

        string cardValue = cardName.Substring(0,cardName.IndexOf("_"));

        int value = -1;

        try{
            value = int.Parse(cardValue);  

        }catch(Exception){
            if(cardValue.Equals("jack")){
                value = 11;
            }
            else if(cardValue.Equals("queen")){
                value = 12;
            }
            else if(cardValue.Equals("king")){
                value = 13;
            }
            else if(cardValue.Equals("ace")){
                value = 14;
            }
        }

        return value - 2;
    }
    
    //convert card suite string to value
    // get the index of the 2nd underscore to the end e.g. string = "spades"
    //convert string to corresponding values
    
    
    private int CardToSuite(string cardName){
        
        int firstIndexOfUnder = cardName.IndexOf("_");
        int secondIndexOfUnder = cardName.IndexOf("_",firstIndexOfUnder + 1);

        string cardValue = cardName.Substring(secondIndexOfUnder+1);

        //Debug.Log("Suite: " + cardValue);

        int value = -1;

        if(cardValue.Equals("clubs")){
            value = 1;
        }else if(cardValue.Equals("diamonds")){
            value = 2;
        }else if(cardValue.Equals("hearts")){
            value = 3;
        }else if(cardValue.Equals("spades")){
            value = 4;
        }

        return value;

    }

    /* take the value and suite values with the corresponding methods
    Add +1 in the values array at the index value. Add +1 in the suites array at the index value

    */

    [PunRPC]
    private void valuesToArray(string currentCard){

        //Debug.Log(currentCard);

        int value = CardToValue(currentCard);
        int suite = CardToSuite(currentCard);

        //Debug.Log("value: " + value + " suite: " + suite);

        valuesTo13[value]++;
        suitesTo13[value] = suite;
        suites[suite]++;
    }

    #endregion
    
    #region dealMethods

    [PunRPC]
    private void playCard(int randomCard, int i){
        Sprite currentCard = tempDeck[randomCard];
        tempDeck.Remove(currentCard);
        dealerCards[i].GetComponent<Image>().sprite = currentCard;
        dealerCards[i].GetComponent<Image>().color = new Color(255f,255f,255f,255f);
    }

    #endregion
    
    #region calcMethods

    [ContextMenu("getHighestHand")]  

    [PunRPC]
    void HighestHandText(){
        textFile.GetComponent<TMPro.TMP_Text>().text = getHighestHand();
    }  
    string getHighestHand(){

        if(royalStraight() && isFlushRange(12, 8)){
            return "Royal Flush";
        }

        int straight = getStraight();

        if(straight != 1 && isFlushRange(straight,straight-4)){
            return "Straight Flush: " + straight;
        }

        int fourOfAKind = getFourOfAKind();
        
        if(fourOfAKind != -1){
            return "Four of A Kind: " +  fourOfAKind;
        }

        int triple = getTriple();
        int pair = getHighestPair();

        if(triple != -1 && pair != -1){
            return "Full house: " + triple + ", " + pair; 
        }

        int flush = getFlush();
        int highestFlush = getHighestFlush(flush);

        if(flush != -1){
            return "Flush: " + highestFlush; 
        }

        if(straight != -1){
            return "Straight: " + straight;
        }

        if(triple != -1){
            return "Triple: " + triple;
        }

        int secondPair = getSecondPair(pair);
        
        if(pair != -1 && secondPair != -1){
            return "Two Pair: " + pair + ", " + secondPair;
        }

        if(pair != -1){
            return "Pair: " + pair;
        }

        return "High Card: " + getHighCard();

    }
    int getHighCard(){

        for(int i = valuesTo13.Length - 1; i >= 0; i--){
            if (valuesTo13[i] == 1){
                return i;
            } 
        }
        return -1;

    }
    int getHighestPair(){

        for(int i = valuesTo13.Length - 1; i >= 0; i--){
            if (valuesTo13[i] == 2){
                return i;
            } 
        }

        return -1;
    }

    int getSecondPair(int firstPair){

        for(int i = firstPair - 1; i >= 0; i--){
            if (valuesTo13[i] == 2){
                return i;
            } 
        }

        return -1;
    }

    int getTriple(){
        
        for(int i = valuesTo13.Length - 1; i >= 0; i--){
            if (valuesTo13[i] == 3){
                return i;
            } 
        }

        return -1;
    }

    int getStraight(){
        
        int current = 12;
        int count = 1;

        for(int i = valuesTo13.Length - 1; i >= 0; i--){
            if (valuesTo13[i] == 1){
                count++;
                if(count == 5){
                    return current;
                }
            } 
            else{
                current = i-1;
                count = 0;
            }
        }
        return -1;
    }

    bool royalStraight(){

        for(int i = valuesTo13.Length - 1; i >= 9; i--){
            if(valuesTo13[i] != 1){
                return false;
            }
        }
        return true;
    }

    int getFlush(){
        
        for(int i = suites.Length - 1; i >= 0; i--){
            if (suites[i] == 5){
                return i;
            } 
        }

        return -1;
    }

    bool isFlushRange(int end, int start){

        if(end == -1 || suitesTo13[end] == 0){
            return false;
        }

        int suiteValue = suitesTo13[end];

        for(int i = end; i >= start; i--){
            if(suitesTo13[i] != suiteValue){
                return false;
            }
        }

        return true;
    }

    int getFourOfAKind(){
        
        for(int i = valuesTo13.Length - 1; i >= 0; i--){
            if (valuesTo13[i] == 4){
                return i;
            } 
        }

        return -1;
    }

    int getHighestFlush(int flush){
        if(flush == -1){
            return -1;
        }
        
        for(int i = valuesTo13.Length - 1; i >= 0; i--){
            if (suitesTo13[i] == flush){
                return  i;
            }
        }

        return -1;
    }

    #endregion

}
