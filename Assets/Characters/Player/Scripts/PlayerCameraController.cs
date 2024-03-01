using UnityEngine;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField]
        private float m_CameraLookSensitivity = 5.0f;
        [SerializeField]
        private float m_MaxTopAngle = 70.0f;
        [SerializeField]
        private float m_MaxBottomAngle = -30.0f;
        [SerializeField]
        private Transform m_CameraFollowTarget;
        [SerializeField]
        private PlayerInputController m_Input;

        private Vector2 m_CameraRotation;

        private void Start()
        {
            m_CameraRotation = m_CameraFollowTarget.eulerAngles;
        }

        private void LateUpdate()
        {
            Vector2 lookInput = m_Input.look;

            if (lookInput.sqrMagnitude >= 0.01f * 0.01f)
            {
                m_CameraRotation.x += lookInput.y * m_CameraLookSensitivity;
                m_CameraRotation.y += lookInput.x * m_CameraLookSensitivity;
            }

            m_CameraRotation.x = ClampAngleInDegree(m_CameraRotation.x, m_MaxBottomAngle, m_MaxTopAngle);
            m_CameraRotation.y = ClampAngleInDegree(m_CameraRotation.y, float.MinValue, float.MaxValue);

            m_CameraFollowTarget.rotation = Quaternion.Euler(m_CameraRotation.x, m_CameraRotation.y, 0.0f);
        }

        private float ClampAngleInDegree(float angle, float minAngle, float maxAngle)
        {
            if (angle < -360.0f)
                angle += 360.0f;

            if (angle > 360.0f)
                angle -= 360.0f;

            return Mathf.Clamp(angle, minAngle, maxAngle);
        }
    }
}
