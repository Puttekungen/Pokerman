using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar hpBar;

    public void SetData(Pokemon Pokemon)
    {
        nameText.text = Pokemon.Base.Name;
        levelText.text = "Lvl " + Pokemon.Level;
        hpBar.SetHp((float)Pokemon.HP / Pokemon.MaxHP);
    }
}
