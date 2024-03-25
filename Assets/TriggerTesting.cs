using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTesting : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        print(other.name + " entered");

        // Dark Orc
        test t = other.GetComponent<test>();
        if (t != null)
        {
            t.EnableGotHit();
        }
        else
        {
            Debug.LogError("test script in null");
        }

        // Test Orc
        HitTesting testing = other.GetComponent<HitTesting>();
        if (testing != null)
        {
            testing.GotHit();
        }
        else
        {
            Debug.LogError("HitTesting script in null");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        print(other.name + " exited");
    }
}
