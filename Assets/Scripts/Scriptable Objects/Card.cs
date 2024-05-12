using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 51)]
public class Card : ScriptableObject
{
    [SerializeField] private int uniqueID = 0;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Sprite image;
    [SerializeField] private TextMeshProUGUI desc;

    public int UniqueID
    {
        get { return uniqueID; }
    }

    public TextMeshProUGUI Title
    {
        get { return title; }
    }

    public Sprite Image
    {
        get { return image; }
    }

    public TextMeshProUGUI Description
    {
        get { return desc; }
    }
}
