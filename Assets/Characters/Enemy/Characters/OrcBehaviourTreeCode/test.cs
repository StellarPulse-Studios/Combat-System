using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.BehaviourTrees;
using Player;
using JetBrains.Annotations;

using VERS;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem.Controls;

public class test : MonoBehaviour, IDamagable
{
    public Vector3Reference hitPoint;
    public GameObject weapon;
    public BehaviourTreeOwner m_BTOwner;
    public List<GameObject> contactPoint;

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
        int index = -1;
        float distance = int.MaxValue;
        for(int i = 0; i < contactPoint.Count; i++)
        {
            GameObject go = contactPoint[i];
            if(distance > Vector3.Distance(go.transform.position, hitPoint.Value))
            {
                distance = Vector3.Distance(go.transform.position, hitPoint.Value);
                index = i;
            }
        }
        int hitID = GetComponent<AttackToHit>().GetValue(contactPoint[index]);
        m_BTOwner.SetExposedParameterValue("HitID", hitID);
        m_BTOwner.SetExposedParameterValue("GotHit", true);
    }

    public void OnDestroy()
    {
        EnableGotHit();
    }

    public void OnDamage()
    {
        EnableGotHit();
    }
}
