using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DisolvingController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinnedMaterials;
    private void ResetDissolve()
    {
        skinnedMaterials[0].SetFloat("_DissolveAmount", 0);
        skinnedMaterials[1].SetFloat("_DissolveAmount", 0);
    }





    // Start is called before the first frame update
    void Start()
    {
        VFXGraph.Stop();
        if (skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine(DissolveCo());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetDissolve();
        }
    }

    IEnumerator DissolveCo()
    {
        if (skinnedMaterials.Length > 0)
        {
            if(VFXGraph != null) {
                VFXGraph.Play();
            
            }

            float counter = 0;
            while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for(int i=0;i< skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
