using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using UnityEngine.SocialPlatforms.Impl;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Direct")]
    [Description("Get the rotation angle and direction")]
    public class GetAngleDirection : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> angle;
        public BBParameter<int> direction;

        protected override void OnUpdate()
        {
            Vector3 directionToTarget = target.value.transform.position - agent.position;

            float angleToRotate = Vector3.SignedAngle(agent.forward, directionToTarget, Vector3.up);

            angle.value = angleToRotate;

            int rotationDirection = Mathf.RoundToInt(Mathf.Sign(angleToRotate));

            direction.value = rotationDirection;
            //Debug.Log("Angle: " + angle);
            //Debug.Log("Direction: " + direction);
            EndAction();
        }
    }
}
