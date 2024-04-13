using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthTester : MonoBehaviour
{
    public UnityEvent<float> onGotHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onGotHit?.Invoke(10.0f);
        }
    }
    public void CurrHealth(float currHealth)
    {
        print(currHealth);
    }
}
