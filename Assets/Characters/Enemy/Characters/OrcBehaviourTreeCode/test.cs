using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.BehaviourTrees;
using Player;

public class test : MonoBehaviour, IDamagable
{
    public GameObject weapon;
    public BehaviourTreeOwner m_BTOwner;

    void Equip()
    {
        weapon.SetActive(true);
    }
    void UnEquip()
    {
        weapon.SetActive(false);    
    }

    public void EnableGotHit()
    {
        m_BTOwner.SetExposedParameterValue("GotHit", true);
    }

    public void OnDamage()
    {
        EnableGotHit();
    }
}
