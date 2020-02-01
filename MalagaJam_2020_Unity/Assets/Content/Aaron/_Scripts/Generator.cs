using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

namespace MalagaJam.Object
{
    public enum GeneratorState
    {
        idle,
        pressed,
        locked
    }

    public class Generator : RepairBase
    {
        //TODO Keep track which player already has pressed the button

        [Header("Button Settings")] 
        [SerializeField] Slider slider;
        bool singlePlayer = true;
        public Door door;
        public Generator otherGenerator;
        bool _SkillCheck = false;
        

        public GeneratorState generatorState;

        protected override void Update()
        {
            ButtonBehaviour();
            SkillCheckBehaviour();
            base.Update();
        }

        float _T = 0, _SliderValue = 0, _Cooldown = 0, _CooldownVal = 5, _TimeOut = 3;
        void SkillCheckBehaviour()
        {
            if (_Cooldown <= 0)
            {
                if (_SkillCheck)
                {
                    _T += Time.deltaTime;
                }

                slider.value = Mathf.PingPong(_T, 1);

                if (ObjectsInRange.Count > 0 && XCI.GetButtonDown(XboxButton.B, ObjectsInRange[0].GetController()))
                {
                    ObjectsInRange[0].m_moveable = false;
                    if (slider.value > 0.33 && slider.value < 0.66)
                    {
                        door.UnlockDoor();
                        ObjectsInRange[0].m_moveable = true;
                        _T = _TimeOut;
                    }
                    else
                    {
                        _SkillCheck = false;
                        _Cooldown = _CooldownVal;
                    }
                }

                if (_T >= _TimeOut)
                {
                    _T = 0;
                    _SkillCheck = false;
                    _Cooldown = _CooldownVal;
                    ObjectsInRange[0].m_moveable = true;
                }
            }
            else
            {
                _Cooldown -= Time.deltaTime;
            }
        }

        void ButtonBehaviour()
        {
            if (ObjectsInRange.Count > 0 && objectRepairState == ObjectRepairState.Repaired)
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
            #if UNITY_EDITOR
            if ((Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.B, ObjectsInRange[0].GetController())) && generatorState != GeneratorState.locked)
#else
            if (XCI.GetButtonDown(XboxButton.B, ObjectsInRange[0].GetController()) && buttonState != ButtonState.locked)
#endif
            {
                generatorState = GeneratorState.locked;
                door.UnlockDoor();
            } else if (XCI.GetButtonDown(XboxButton.B) && generatorState == GeneratorState.locked)
            {
                _SkillCheck = true;
            }
        }

        float _Timer;

        void MultiButtonBehaviour()
        {
            if (generatorState == GeneratorState.locked) return;

            if (_Timer >= 1)
            {
                generatorState = GeneratorState.idle;
                _Timer = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space)) generatorState = GeneratorState.pressed;
            if (generatorState == GeneratorState.pressed)
            {
                _Timer += Time.deltaTime;
            }

            if (generatorState == GeneratorState.pressed && otherGenerator.generatorState == GeneratorState.pressed)
            {
                door.UnlockDoor();
                generatorState = GeneratorState.locked;
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
            if (otherGenerator != null)
            {
                Vector3 otherCurPos = otherGenerator.transform.position;
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