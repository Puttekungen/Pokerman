using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;


    public void SetHp(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }
}
