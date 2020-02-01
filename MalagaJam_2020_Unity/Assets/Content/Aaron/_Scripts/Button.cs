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

        [Header("Button Settings")] 
        [SerializeField] bool singlePlayer = true;
        [SerializeField] Button otherButton;
        [SerializeField] UnityEvent onPressedEvent;

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
                onPressedEvent?.Invoke();
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
                onPressedEvent?.Invoke();
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

            Gizmos.DrawLine(transform.position, otherButton.transform.position);
        }
    }
}