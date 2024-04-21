using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MobEventManager : MonoBehaviour
{
    public GameObject weapon;
    public ParticleSystem attackVFX;
    public AudioSource attackSound;
    public AudioClip[] sounds;
    public VisualEffect deathVFX;
    public float dissolveRate = 0.0125f;
    public SkinnedMeshRenderer skinnedMesh;
    public Material OrcDissolveMaterial;

    private Material[] skinnedMaterials;
    public void EnableMoving() { }
    public void DisableMoving() { }


    public void EnableHitBox() { 
        GetComponent<CollisionDetection>().EnableHitBox();
        attackVFX.Play();
        int index = Random.Range(0, sounds.Length);
        attackSound.clip = sounds[index];
        attackSound.Play();
    }
    public void DisableHitBox() {
        GetComponent<CollisionDetection>().DisableHitBox();
    }

    public void OnDeathVFX()
    {
        StartCoroutine(Dissolve());
    }
    IEnumerator Dissolve()
    {
        skinnedMesh.sharedMaterial = OrcDissolveMaterial;
        skinnedMaterials = skinnedMesh.materials;
        deathVFX.Stop();
        deathVFX.Play();
        float counter = 0;
        while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
            }
            yield return null;
        }
    }
}
