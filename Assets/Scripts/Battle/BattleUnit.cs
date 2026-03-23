using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase Base;
    [SerializeField] int Level;
    [SerializeField] bool isPlayerUnit;


    public Pokemon Pokemon { get; set; }

    public void Setup()
    {
        Pokemon = new Pokemon(Base, Level);
        if (isPlayerUnit)
        {
            GetComponent<Image>().sprite = Pokemon.Base.BackSprite;
            Debug.Log("backsprite");
        }
        else
        {
            GetComponent<Image>().sprite = Pokemon.Base.FrontSprite;
            Debug.Log("froontsprite");
        }

    }
}
