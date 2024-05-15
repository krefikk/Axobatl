using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

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

    public void Start()
    {
        cardAnimator = GetComponentInChildren<CardAnimator>();
        cards = Resources.LoadAll<Card>("Cards/");
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
        middleTitle.text = cards[IDArray[1]].Title;
        middleDesc.text = cards[IDArray[1]].Description;
        rightTitle.text = cards[IDArray[2]].Title;
        rightDesc.text = cards[IDArray[2]].Description;
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
}
