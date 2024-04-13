using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEventManager : MonoBehaviour
{
    public GameObject weapon;
    public void EnableMoving() { }
    public void DisableMoving() { }

    public void EnableHitBox() { 
        GetComponent<CollisionDetection>().EnableHitBox();
    }
    public void DisableHitBox() {
        GetComponent<CollisionDetection>().DisableHitBox();
    }

}
