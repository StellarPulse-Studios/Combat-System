using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject weapon;
    void Equip()
    {
        weapon.SetActive(true);
    }
    void UnEquip()
    {
        weapon.SetActive(false);    
    }

}
