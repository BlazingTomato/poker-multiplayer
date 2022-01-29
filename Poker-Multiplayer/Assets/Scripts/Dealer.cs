using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dealer : MonoBehaviour
{
    
    public List<Sprite> fullDeck;
    List<Sprite> tempDeck;

    int[] valuesTo13;
    int[] suitesTo13;
    int[] suites;


    public GameObject[] dealerCards = new GameObject[5];
    
    private void Start() {
        tempDeck = fullDeck;
        dealerCards = new GameObject[5];
        valuesTo13 = new int[13];
        suitesTo13 = new int[13];
        suites = new int[4];
    }
    private void NewRound(){
        tempDeck = fullDeck;
        dealerCards = new GameObject[5];
        valuesTo13 = new int[13];
        suitesTo13 = new int[13];
        suites = new int[4];
    }


    #region cardConversion
    //convert card string to values: starting from 2-10, those are labled as numbers so parse int will work
    //for face cards, take the format and take the beginning to the underscore
    // convert face card string to the corresponding value
    // initalize to the array
    private int CardToValue(Sprite card){

        string cardName = card.name;
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

        return value - 1;
    }
    
    //convert card suite string to value
    // get the index of the 2nd underscore to the end e.g. string = "spades"
    //convert string to corresponding values
    private int CardToSuite(Sprite card){
        string cardName = card.name;
        string cardValue = cardName.Substring(cardName.IndexOf("_",cardName.IndexOf("_",3))+1);

        int value = -1;

        if(cardValue.Equals("clubs")){
            value = 0;
        }else if(cardValue.Equals("diamonds")){
            value = 1;
        }else if(cardValue.Equals("hearts")){
            value = 2;
        }else if(cardValue.Equals("spades")){
            value = 3;
        }

        return value;

    }

    /* take the value and suite values with the corresponding methods
    Add +1 in the values array at the index value. Add +1 in the suites array at the index value

    */
    private void valuesToArray(Sprite currentCard){

        int value = CardToValue(currentCard);
        int suite = CardToSuite(currentCard);
        valuesTo13[value]++;
        suitesTo13[value] = suite;
        suites[suite]++;
    }

    #endregion
    
    #region dealMethods
    private void Flop(){
        for(int i = 0; i < 3; i++){
            int randomCard = UnityEngine.Random.Range(0,tempDeck.Count);
            Sprite currentCard = tempDeck[randomCard];
            dealerCards[i].GetComponent<Image>().sprite = currentCard;
            tempDeck.Remove(currentCard);
            
            valuesToArray(currentCard);
        }
    }

    private void River(){
        int randomCard = UnityEngine.Random.Range(0,tempDeck.Count);
        Sprite currentCard = tempDeck[randomCard];
        dealerCards[3].GetComponent<Image>().sprite = currentCard;
        valuesToArray(tempDeck[randomCard]);
        tempDeck.Remove(tempDeck[randomCard]);
        
    }

    private void Turn(){
        int randomCard = UnityEngine.Random.Range(0,tempDeck.Count);
        Sprite currentCard = tempDeck[randomCard];
        dealerCards[4].GetComponent<Image>().sprite = currentCard;
        valuesToArray(tempDeck[randomCard]);
        tempDeck.Remove(tempDeck[randomCard]);
    }   

    #endregion
    
    #region calcMethods

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
            if (valuesTo13[i] == 2){
                return i;
            } 
        }

        return -1;
    }

    int getStraight(){
        
        int current = 13;
        int count = 1;

        for(int i = valuesTo13.Length - 2; i >= 0; i--){
            if (valuesTo13[i] == 1){
                count++;
                if(count == 5){
                    return current;
                }
            } 
            else{
                current = i-1;
                count = 0;
                i--;
            }
        }
        return -1;
    }

    int getFlush(){
        
        for(int i = suites.Length - 1; i >= 0; i--){
            if (suites[i] == 5){
                return i;
            } 
        }

        return -1;
    }

    bool getFlushRange(int start, int end){

        int flushValue = suitesTo13[start-1];
        for(int i = start; i < end; i++){
            if(suitesTo13[i] != flushValue){
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


    #endregion

}
