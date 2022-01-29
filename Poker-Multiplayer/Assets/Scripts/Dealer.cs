using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dealer : MonoBehaviour
{
    
    public List<GameObject> fullDeck;
    public List<GameObject> tempDeck;

    GameObject[] dealerCards = new GameObject[5];
    


    private int cardToValue(GameObject card){

        string cardName = card.GetComponent<Image>().name;
        string cardValue = cardName.Substring(0,cardName.IndexOf("_"));

        int value = 0;

        try{
            value = int.Parse(cardValue);  

        }catch(Exception e){
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
        return value;
    }

    private int cardToSuite(GameObject card){

        return 0;
    }
    private void newRound(){
        tempDeck = fullDeck;
        dealerCards = new GameObject[5];
    }


    
    #region dealMethods
    private void flop(){
        for(int i = 0; i < 3; i++){
            int randomCard = UnityEngine.Random.Range(0,tempDeck.Count);
            dealerCards[i] = tempDeck[randomCard];
            tempDeck.Remove(tempDeck[randomCard]);
        }
    }

    private void river(){
        int randomCard = UnityEngine.Random.Range(0,tempDeck.Count);
        dealerCards[3] = tempDeck[randomCard];
        tempDeck.Remove(tempDeck[randomCard]);
        
    }

    private void turn(){
        int randomCard = UnityEngine.Random.Range(0,tempDeck.Count);
        dealerCards[4] = tempDeck[randomCard];
        tempDeck.Remove(tempDeck[randomCard]);
    }   

    #endregion
    


}
