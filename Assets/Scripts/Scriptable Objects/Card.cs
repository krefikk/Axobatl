using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 51)]
public class Card : ScriptableObject
{
    [SerializeField] private int uniqueID = 0;
    [SerializeField] private string title;
    [SerializeField] private string desc;
    [SerializeField] private bool oneTime;

    public int UniqueID
    {
        get { return uniqueID; }
    }

    public string Title
    {
        get { return title; }
    }

    public string Description
    {
        get { return desc; }
    }

    public bool OneTime
    {
        get { return oneTime; }
    }
}
