using NodeCanvas.Tasks.Conditions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class AttackToHit : MonoBehaviour {
    [System.Serializable]
    public class Pair
    {
        public GameObject AttackPoint;
        public int HitID;
    }

    public Pair[] map;

    public int GetValue(GameObject hit) {
        
        for(int i = 0; i < map.Length; i++)
        {
            if (hit.Equals(map[i].AttackPoint))
            {
                return map[i].HitID;
            }
        }
        return 0;
    }
    

}
