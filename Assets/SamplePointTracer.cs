using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePointTracer : MonoBehaviour
{
    [SerializeField]
    private Transform m_SamplePivot;
    [SerializeField]
    private Transform m_ResultPivot;
    [SerializeField]
    private bool m_Flip;

    [Header("Debug")]
    [SerializeField]
    private float m_SamplePointRadius = 0.025f;
    [SerializeField]
    private Color m_SamplePointColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    [SerializeField]
    private Color m_MotionLineColor = new Color(0.0f, 1.0f, 1.0f, 0.3f);
    [SerializeField]
    private float m_SamplePointLocalAxisLength = 0.2f;
    [SerializeField]
    private Color m_SamplePointLocalRightColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    [SerializeField]
    private Color m_SamplePointLocalUpColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    [SerializeField]
    private Color m_SamplePointLocalForwardColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
    [SerializeField]
    private float m_ResultPivotRadius = 0.05f;
    [SerializeField]
    private Color m_ResultPivotColor = new Color(1.0f, 0.0f, 1.0f, 0.5f);
    [SerializeField]
    private float m_ResultPivotAxisLength = 0.5f;
    [SerializeField]
    private Color m_ResultPivotRightColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    [SerializeField]
    private Color m_ResultPivotUpColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    [SerializeField]
    private Color m_ResultPivotForwardColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);

    private bool m_IsActive;
    private bool m_CanCalculateResultantPivot;
    private readonly List<Vector3> m_Points = new List<Vector3>();

    private void Start()
    {
        m_IsActive = false;
        m_CanCalculateResultantPivot = false;
    }

    private void Update()
    {
        if (m_IsActive)
        {
            m_CanCalculateResultantPivot = true;
            if (m_SamplePivot)
                m_Points.Add(m_SamplePivot.position);
        }
        else
        {
            if (m_CanCalculateResultantPivot)
            {
                m_CanCalculateResultantPivot = false;
                CalculateResultantPivot();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (m_Points.Count > 1)
        {
            DrawSamplePointMotion();
        }
    }

    private void EnableMoving()
    {

    }

    private void DisableMoving()
    {

    }

    private void EnableCharging()
    {

    }

    private void DisableCharging()
    {

    }

    private void EnableHitBox()
    {
        m_Points.Clear();
        m_IsActive = true;
    }

    private void DisableHitBox()
    {
        m_IsActive = false;
    }

    private void CalculateResultantPivot()
    {
        if (m_Points.Count == 0)
            return;

        Vector3 pivotPosition = m_Points[^1];
        Vector3 pivotUp = Vector3.zero;
        Vector3 pivotRight = Vector3.zero;
        Vector3 pivotForward = Vector3.zero;

        if (m_Points.Count < 2)
        {
            SetResultantPivot(pivotPosition, Vector3.up, Vector3.forward, Vector3.right);
            return;
        }

        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        Vector3 up = Vector3.up;

        for (int i = 0; i < m_Points.Count - 1; i++)
        {
            Vector3 currPoint = m_Points[i];
            Vector3 nextPoint = m_Points[i + 1];

            forward = (nextPoint - currPoint).normalized;
            right = Vector3.Cross(Vector3.up, forward).normalized;
            up = Vector3.Cross(forward, right);

            pivotPosition += currPoint;
            pivotUp += up;
            pivotRight += right;
            pivotForward += forward;
        }

        pivotPosition /= m_Points.Count;

        pivotUp /= (m_Points.Count - 1) * (m_Flip ? -1.0f : 1.0f);
        pivotRight /= m_Points.Count - 1;
        pivotForward /= m_Points.Count - 1;
        
        pivotUp.Normalize();
        pivotRight.Normalize();
        pivotForward.Normalize();

        SetResultantPivot(pivotPosition, pivotUp, pivotRight, pivotForward);
    }

    private void SetResultantPivot(Vector3 position, Vector3 up, Vector3 forward, Vector3 right)
    {
        if (!m_ResultPivot)
            return;

        m_ResultPivot.position = position;
        //m_ResultPivot.forward = forward;
        m_ResultPivot.up = up;
        //m_ResultPivot.right = right;
    }

    private void DrawSamplePointMotion()
    {
        Vector3 pivotPosition = Vector3.zero;
        Vector3 pivotUp = Vector3.zero;
        Vector3 pivotRight = Vector3.zero;
        Vector3 pivotForward = Vector3.zero;

        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        Vector3 up = Vector3.up;

        for (int i = 0; i < m_Points.Count - 1; i++)
        {
            Vector3 currPoint = m_Points[i];
            Vector3 nextPoint = m_Points[i + 1];

            forward = (nextPoint - currPoint).normalized;
            right = Vector3.Cross(Vector3.up, forward).normalized;
            up = Vector3.Cross(forward, right);

            pivotPosition += currPoint;
            pivotUp += up;
            pivotRight += right;
            pivotForward += forward;

            // Drawing sample points
            Gizmos.color = m_SamplePointColor;
            Gizmos.DrawSphere(currPoint, m_SamplePointRadius);

            // Drawing motion lines
            Gizmos.color = m_MotionLineColor;
            Gizmos.DrawLine(currPoint, nextPoint);

            // Drawing forward axis
            Gizmos.color = m_SamplePointLocalForwardColor;
            Gizmos.DrawRay(currPoint, forward * m_SamplePointLocalAxisLength);

            // Drawing right axis
            Gizmos.color = m_SamplePointLocalRightColor;
            Gizmos.DrawRay(currPoint, right * m_SamplePointLocalAxisLength);

            // Drawing right axis
            Gizmos.color = m_SamplePointLocalUpColor;
            Gizmos.DrawRay(currPoint, up * m_SamplePointLocalAxisLength);
        }

        pivotPosition += m_Points[^1];

        pivotPosition /= m_Points.Count;
        pivotUp /= (m_Points.Count - 1) * (m_Flip ? -1.0f : 1.0f);
        pivotRight /= m_Points.Count - 1;
        pivotForward /= m_Points.Count - 1;

        pivotUp.Normalize();
        pivotRight.Normalize();
        pivotForward.Normalize();

        pivotForward = Vector3.Cross(pivotUp, Vector3.up).normalized;
        pivotRight = Vector3.Cross(pivotUp, pivotForward);

        // Drawing sample points
        Gizmos.color = m_SamplePointColor;
        Gizmos.DrawSphere(m_Points[^1], m_SamplePointRadius);

        // Drawing forward axis
        Gizmos.color = m_SamplePointLocalForwardColor;
        Gizmos.DrawRay(m_Points[^1], forward * m_SamplePointLocalAxisLength);

        // Drawing right axis
        Gizmos.color = m_SamplePointLocalRightColor;
        Gizmos.DrawRay(m_Points[^1], right * m_SamplePointLocalAxisLength);

        // Drawing right axis
        Gizmos.color = m_SamplePointLocalUpColor;
        Gizmos.DrawRay(m_Points[^1], up * m_SamplePointLocalAxisLength);

        // Drawing resultant pivot
        Gizmos.color = m_ResultPivotColor;
        Gizmos.DrawSphere(pivotPosition, m_ResultPivotRadius);

        // Drawing resultant pivot right axis
        Gizmos.color = m_ResultPivotRightColor;
        Gizmos.DrawRay(pivotPosition, pivotRight * m_ResultPivotAxisLength);

        // Drawing resultant pivot up axis
        Gizmos.color = m_ResultPivotUpColor;
        Gizmos.DrawRay(pivotPosition, pivotUp * m_ResultPivotAxisLength);

        // Drawing resultant pivot forward axis
        Gizmos.color = m_ResultPivotForwardColor;
        Gizmos.DrawRay(pivotPosition, pivotForward * m_ResultPivotAxisLength);
    }

    
}
