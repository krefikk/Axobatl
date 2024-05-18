using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.SceneManagement;
using FMODUnity;

public class CardUIHandler : MonoBehaviour
{
    CardAnimator cardAnimator;
    Card[] cards;
    List<int> displayedOneTimeCardIDs = new List<int>();
    public TextMeshProUGUI leftTitle;
    public TextMeshProUGUI middleTitle;
    public TextMeshProUGUI rightTitle;
    public TextMeshProUGUI leftDesc;
    public TextMeshProUGUI middleDesc;
    public TextMeshProUGUI rightDesc;
    bool displayed = false;
    int leftCardPerkID;
    int middleCardPerkID;
    int rightCardPerkID;

    public EventReference cardLoad;
    public EventReference cardSelect;
    public GameObject instance; // placeholder because I can't get the singleton to work using PlayerController.player

    public void Start()
    {
        cardAnimator = GetComponentInChildren<CardAnimator>();
        cards = Resources.LoadAll<Card>("Cards/");

        RuntimeManager.PlayOneShotAttached(cardLoad, instance);
    }

    public void Update()
    {

        if (cardAnimator.AnimFinished() && !displayed) 
        {
            DisplayChosenCards();
            displayed = true;
        }
    }

    public void DisplayChosenCards()
    {
        int[] IDArray = ChosenCards();
        leftTitle.text = cards[IDArray[0]].Title;
        leftDesc.text = cards[IDArray[0]].Description;
        leftCardPerkID = IDArray[0];
        middleTitle.text = cards[IDArray[1]].Title;
        middleDesc.text = cards[IDArray[1]].Description;
        middleCardPerkID = IDArray[1];
        rightTitle.text = cards[IDArray[2]].Title;
        rightDesc.text = cards[IDArray[2]].Description;
        rightCardPerkID = IDArray[2];
        Debug.Log(leftCardPerkID);
        Debug.Log(middleCardPerkID);
        Debug.Log(rightCardPerkID);
    }

    public int[] ChosenCards() 
    {
        int firstID = Random.Range(0, cards.Length);
        while (CheckInside(firstID, displayedOneTimeCardIDs)) 
        {
            firstID = Random.Range(0, cards.Length);
        }
        int secondID = Random.Range(0, cards.Length);
        while (secondID == firstID || CheckInside(secondID, displayedOneTimeCardIDs)) 
        {
            secondID = Random.Range(0, cards.Length);
        }
        int thirdID = Random.Range(0, cards.Length);
        while (thirdID == firstID || thirdID == secondID || CheckInside(thirdID, displayedOneTimeCardIDs))
        {
            thirdID = Random.Range(0, cards.Length);
        }
        if (cards[firstID].OneTime) 
        {
            displayedOneTimeCardIDs.Add(firstID);
        }
        if (cards[secondID].OneTime)
        {
            displayedOneTimeCardIDs.Add(secondID);
        }
        if (cards[thirdID].OneTime)
        {
            displayedOneTimeCardIDs.Add(thirdID);
        }
        int[] array = new int[3];
        array[0] = firstID;
        array[1] = secondID;
        array[2] = thirdID;
        return array;
    }

    public bool CheckInside(int number, List<int> array) 
    {
        if (array is null) 
        {
            return false;
        }
        for (int i = 0; i < array.Count; i++)
        {
            if (number == array[i])
            {
                return true;
            }
        }
        return false;
    }

    public void AssignLeftCardToPlayer() 
    {
        AssignPerkToPlayer(leftCardPerkID);
        leftTitle.text = "";
        leftDesc.text = "";
        SceneManager.LoadScene(0);
        PlayerController.player.gameObject.SetActive(true);
        PlayerController.player.inCardsScene = false;
    }

    public void AssignMiddleCardToPlayer()
    {
        AssignPerkToPlayer(middleCardPerkID);
        middleTitle.text = "";
        middleDesc.text = "";
        SceneManager.LoadScene(0);
        PlayerController.player.gameObject.SetActive(true);
        PlayerController.player.inCardsScene = false;
    }

    public void AssignRightCardToPlayer()
    {
        AssignPerkToPlayer(rightCardPerkID);
        rightTitle.text = "";
        rightDesc.text = "";
        SceneManager.LoadScene(0);
        PlayerController.player.gameObject.SetActive(true);
        PlayerController.player.inCardsScene = false;
    }

    void AssignPerkToPlayer(int ID) 
    {
        if (ID == 0) { PlayerController.player.HealthBoost(); }
        else if (ID == 1) { PlayerController.player.KatanaDash(); }
        else if (ID == 2) { PlayerController.player.CoolerDash(); }
        else if (ID == 3) { PlayerController.player.FatalBullets(); }
        else if (ID == 4) { PlayerController.player.AppleJuice(); }
        else if (ID == 5) { PlayerController.player.Fortress(); }
        else if (ID == 6) { PlayerController.player.SqueezE(); }
        else if (ID == 7) { PlayerController.player.PressurizedBullets(); }
        else if (ID == 8) { PlayerController.player.HighCaliber(); }
        else if (ID == 9) { PlayerController.player.SpeedUp(); }
        else if (ID == 10) { PlayerController.player.DashBurger(); }
        else if (ID == 11) { PlayerController.player.ClickClack(); }
        else if (ID == 12) { PlayerController.player.BlastGum(); }
        else if (ID == 13) { PlayerController.player.SawBullets(); }
        else if (ID == 14) { PlayerController.player.LetItBeBullet(); }
        else if (ID == 15) { PlayerController.player.DashAirlines(); }
    }
}
