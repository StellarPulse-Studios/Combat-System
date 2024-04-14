using NodeCanvas.BehaviourTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using VERS;


public class OrcTrigger : MonoBehaviour
{

    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject player;
    [SerializeField] private float coolDownTime = 5.5f;
    [SerializeField] private IntReference attackTokenReference;
    [SerializeField] private IntReference restTokenReference;
    [SerializeField] private int noOfAttackToken;
    private float elaspedTime = 0f;

    void Start()
    {
        attackTokenReference.Value = noOfAttackToken;
        restTokenReference.Value = 0;
    }
    void CallTrigger(GameObject enemy)
    {
        enemy.GetComponent<BehaviourTreeOwner>().SetExposedParameterValue("AllowAttack", true);
        // Debug.Log(enemy.name);
    }
    // Update is called once per frame
    void Update()
    {
        
        List<GameObject> list = new List<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, detectionRadius,enemyLayer);
        foreach (Collider collider in colliders)
        {
            // Filter out the GameObjects that are not needed
            if (collider.gameObject != gameObject && collider.gameObject.tag.Equals("Enemy"))
            {
                // Do something with the detected GameObjects
                list.Add(collider.gameObject.transform.parent.gameObject);
            }
        }
        if (list.Count != 0 && attackTokenReference.Value > 0)
        {
            int randomNumber = Random.Range(0, list.Count);
            attackTokenReference.Value -= 1;
            // Debug.Log(attackTokenReference.Value+ " : " + randomNumber);
            CallTrigger(list[randomNumber]);
            elaspedTime = 0f;
        }
        if(restTokenReference.Value == noOfAttackToken)
        {
            CallRest();
        }
    }

    private void CallRest()
    {
        if(elaspedTime >= coolDownTime)
        {
            attackTokenReference.Value = restTokenReference.Value;
            restTokenReference.Value = 0;
            return;
        }
        elaspedTime += Time.deltaTime; 
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.transform.position, detectionRadius);
    }
}
