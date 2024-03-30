using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.BehaviourTrees;
using Player;
using JetBrains.Annotations;

public class test : MonoBehaviour, IDamagable
{
    public GameObject weapon;
    public BehaviourTreeOwner m_BTOwner;
    public int attckID;
    

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
        int hitID = this.GetComponent<AttackToHit>().GetValue(attckID);
        m_BTOwner.SetExposedParameterValue("HitID", hitID);
        m_BTOwner.SetExposedParameterValue("GotHit", true);
    }

    public void OnDamage()
    {
        EnableGotHit();
    }
}
