using System;
using UnityEngine;
using UnityEngine.Events;

namespace MalagaJam.Object
{
    public enum ButtonState
    {
        idle,
        pressed,
        locked
    }

    public class Button : RepairBase
    {
        //TODO Keep track which player already has pressed the button

        [Header("Button Settings")] [SerializeField]
        Door door;
        [SerializeField] bool singlePlayer = true;
        [SerializeField] Button otherButton;

        public ButtonState buttonState;
        
        protected override void Update()
        {
            ButtonBehaviour();
            base.Update();
        }

        void ButtonBehaviour()
        {
            if (ObjectsInRange.Count > 0)
            {
                if (singlePlayer)
                {
                    SingleButtonBehaviour();
                }
                else
                {
                    MultiButtonBehaviour();
                }
            }
        }

        void SingleButtonBehaviour()
        {
            if (Input.GetKeyDown(KeyCode.Space) && buttonState != ButtonState.locked)
            {
                buttonState = ButtonState.locked;
                door.UnlockDoor();
            }
        }

        float _Timer;

        void MultiButtonBehaviour()
        {
            if (buttonState == ButtonState.locked) return;

            if (_Timer >= 1)
            {
                buttonState = ButtonState.idle;
                _Timer = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space)) buttonState = ButtonState.pressed;
            if (buttonState == ButtonState.pressed)
            {
                _Timer += Time.deltaTime;
            }

            if (buttonState == ButtonState.pressed && otherButton.buttonState == ButtonState.pressed)
            {
                door.UnlockDoor();
                buttonState = ButtonState.locked;
                _Timer = 0;
            }
        }

        public override void Repair()
        {
            if (!singlePlayer && ObjectsInRange.Count > 1) return;
            base.Repair();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector3 curPos = transform.position;
            if (otherButton != null)
            {
                Vector3 otherCurPos = otherButton.transform.position;
                Vector3 center = Vector3.zero;

                center.x = curPos.x + (otherCurPos.x - curPos.x) / 2;
                center.y = curPos.y + (otherCurPos.y - curPos.y) / 2;;

                Gizmos.DrawLine(curPos, otherCurPos);
                
                Gizmos.color = Color.green;
                if (door != null)
                {
                    Gizmos.DrawLine(center, door.transform.position);
                }
            }
            else if (door != null)
            {
                Gizmos.DrawLine(curPos, door.transform.position);
            }
        }
    }
}