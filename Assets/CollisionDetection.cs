using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VERS;

public class CollisionDetection : MonoBehaviour
{
    public BoxCollider boxCollider;
    public LayerMask playerLayer;
    public Vector3Reference hitPoint;


    private bool isBoxCasting = false; 
    private HashSet<Collider> colliderSet;
    private Vector3 previousHitBoxPosition;
    private Vector3 gizmosHitPoint;
    private void Start()
    {
        colliderSet = new HashSet<Collider>();

    }
    public void EnableHitBox()
    {
        colliderSet.Clear();
        isBoxCasting = true;
        previousHitBoxPosition = boxCollider.transform.TransformPoint(boxCollider.center);
    }

    public void DisableHitBox()
    {
        isBoxCasting = false;
    }
    private void Update()
    {

        if (isBoxCasting)
        {
            DoDamage();
        }
    }
    private void DoDamage()
    {
        Vector3 center = boxCollider.transform.TransformPoint(boxCollider.center);
        Collider[] colliders = Physics.OverlapBox(center,  Vector3.Scale(boxCollider.size, boxCollider.transform.localScale) * 0.5f, boxCollider.transform.rotation, playerLayer);
        foreach(Collider collider in colliders)
        {
            if (colliderSet.Contains(collider))
                continue;
            colliderSet.Add(collider);

            hitPoint.Value = collider.ClosestPoint(center);

            Vector3 boxCastOffset = center - previousHitBoxPosition;
            Vector3 boxCastDirection = boxCastOffset.normalized;
            if (Physics.BoxCast(previousHitBoxPosition, boxCollider.size * 0.5f, boxCastDirection, out RaycastHit hitInfo, boxCollider.transform.rotation, boxCastOffset.magnitude, playerLayer))
            {
                hitPoint.Value = hitInfo.point;
                Debug.Log(hitInfo.point);
                //UnityEditor.EditorApplication.isPaused = true;
            }

            gizmosHitPoint = hitPoint.Value;

            if (collider.TryGetComponent(out IDamagable damagable))
            {
                // Debug.Log("damage");
                damagable.OnDamage(10.0f);
            }

        }
        previousHitBoxPosition = center;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(gizmosHitPoint, 0.1f);

    }

}
