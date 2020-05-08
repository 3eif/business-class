using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    //This function is called when a user gets a question correct. The function gets the current amount of coins, then adds x amount to it, and saves the amount of coins.
    public static void addCoin(string coinS, int coinI){
        int coinsNum = PlayerPrefs.GetInt(coinS, 0);
        coinsNum += coinI;
        PlayerPrefs.SetInt(coinS, coinsNum);
        PlayerPrefs.Save();
    }
}
