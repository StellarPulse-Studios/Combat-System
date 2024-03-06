using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public bool sprint;
        public bool jump;
        public bool crouch;
        public Blackboard blackboard;

        private void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
            blackboard.move = move;
        }

        private void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
            blackboard.look = look;
        }

        private void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
            blackboard.sprint = sprint;
        }

        private void OnJump(InputValue value)
        {
            jump = value.isPressed;
            blackboard.jump = jump;
        }

        private void OnCrouch(InputValue value)
        {
            crouch = value.isPressed;
            blackboard.isCrouched = !blackboard.isCrouched;
        }

        private void OnExit(InputValue value)
        {
            Application.Quit();
        }

        public void RumbleGamepad(float low, float high)
        {
            Gamepad pad = Gamepad.current;
            if (pad == null)
                return;

            pad.SetMotorSpeeds(low, high);
        }
    }
}
