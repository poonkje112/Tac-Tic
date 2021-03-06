using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

namespace MalagaJam.Object
{
    public enum GeneratorState
    {
        idle,
        pressed,
        locked,
        finished
    }

    public class Generator : RepairBase
    {
        //TODO Keep track which player already has pressed the button

        [Header("Button Settings")] 
        [SerializeField] Slider slider;
        [SerializeField] GameObject canvas;
        [SerializeField] bool singlePlayer = true;
        [SerializeField] Rotation[] Gears;
        public Door door;
        public Generator otherGenerator;
        bool _SkillCheck = false;
        

        public GeneratorState generatorState;

        protected override void Start()
        {
            base.Start();
            canvas.SetActive(false);
            print(Gears.Length);
            foreach (Rotation rot in Gears)
            {
                rot.rotate = false;
            }
        }

        protected override void Update()
        {
            if(objectRepairState == ObjectRepairState.Repaired)
            {
                ButtonBehaviour();
                SkillCheckBehaviour();
            }
            base.Update();
        }

        float _T = 0, _SliderValue = 0, _Cooldown = 0, _CooldownVal = 1, _TimeOut = 1;
        bool _SkillCheckGoing = false;
        Player _P;
        void SkillCheckBehaviour()
        {
            if (_Cooldown <= 0 && generatorState != GeneratorState.finished)
            {
                if (_SkillCheck)
                {
                    _T += Time.deltaTime;
                }

                slider.value = Mathf.PingPong(_T, 1);

                if (ObjectsInRange.Count > 0 && XCI.GetButtonDown(repairButton, ObjectsInRange[0].GetController()))
                {
                    if (slider.value > 0.33 && slider.value < 0.66 && _SkillCheckGoing)
                    {
                        ResetSkillcheck();
                        generatorState = GeneratorState.finished;
                        _Sr.sprite = repairedSprite;

                        foreach (Rotation rot in Gears)
                        {
                            rot.rotate = true;
                        }
                        _T = _TimeOut;
                    }
                    else
                    {
                        if (_SkillCheckGoing)
                        {
                            ResetSkillcheck();
                            _Cooldown = _CooldownVal;
                        }
                        else
                        {
                            _P = ObjectsInRange[0];
                            _P.m_moveable = false;
                            _SkillCheck = true;
                            _SkillCheckGoing = true;
                            canvas.SetActive(true);
                            generatorState = GeneratorState.locked;
                        }
                    }
                }

                if (_T >= _TimeOut)
                {
                    ResetSkillcheck();
                    _Cooldown = _CooldownVal;
                }
            }
            else if( generatorState != GeneratorState.finished)
            {
                _Cooldown -= Time.deltaTime;
            }    
        }

        void ResetSkillcheck()
        {
            if (_P != null)
            {
                _P.m_moveable = true;
                _P = null;
            }

            _T = 0;
            _SkillCheck = false;
            _SkillCheckGoing = false;
            
            if(generatorState != GeneratorState.finished)
                generatorState = GeneratorState.idle;

            canvas.SetActive(false);
        }

        void ButtonBehaviour()
        {
            if (ObjectsInRange.Count > 0 && objectRepairState == ObjectRepairState.Repaired && _Cooldown <= 0)
            {
                SingleButtonBehaviour();
            }
        }

        void SingleButtonBehaviour()
        {
            if (XCI.GetButtonDown(XboxButton.B, ObjectsInRange[0].GetController()) && generatorState != GeneratorState.locked)
            {
                _SkillCheck = true;
            }
        }

        float _Timer;

        void MultiButtonBehaviour()
        {
            if (generatorState == GeneratorState.locked) return;

            if (_Timer >= _TimeOut+2)
            {
                _Timer = 0;
            }

            if (XCI.GetButtonDown(XboxButton.B, ObjectsInRange[0].GetController())) _SkillCheck = true;
            if (generatorState == GeneratorState.pressed)
            {
                _Timer += Time.deltaTime;
            }

            if (generatorState == GeneratorState.finished && otherGenerator.generatorState == GeneratorState.finished)
            {
//                door.UnlockDoor();
                generatorState = GeneratorState.locked;
                _Timer = 0;
            }
            else
            {
//                generatorState = GeneratorState.idle;
//                otherGenerator.generatorState = GeneratorState.idle;
            }
        }

        public override void Repair()
        {
            if (ObjectsInRange.Count > 1) return;
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