using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VERS;



public class OrcVRES : MonoBehaviour
{
    // Start is called before the first frame update
    public IntReference restToken;
    NavMeshAgent agent;

    public void incrementByOne()
    {
        
        if (restToken != null)
        {
            restToken.Value += 1;
        }
    }
}



