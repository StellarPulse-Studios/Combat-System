using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class HitScreenShake : MonoBehaviour
{
    [SerializeField] private MMFeedbacks m_HitFeedbacks;

    public void ShakeScreen()
    {
        if (m_HitFeedbacks)
            m_HitFeedbacks.PlayFeedbacks();
    }
}
