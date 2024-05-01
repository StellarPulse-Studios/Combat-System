using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class FramerateHandler : MonoBehaviour
    {
        [SerializeField] private int m_TargetFramerate = 60;

        private void Start()
        {
            Application.targetFrameRate = m_TargetFramerate;
        }
    }
}
