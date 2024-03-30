using NodeCanvas.Tasks.Conditions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class AttackToHit : MonoBehaviour {
    [System.Serializable]
    public class Pair
    {
        public int AttackID;
        public int HitID;
    }

    public Pair[] map;

    public int GetValue(int attackID) {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i].AttackID == attackID) return map[i].HitID;
        }
        return 0;
    }
    

}
