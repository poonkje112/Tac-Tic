using System;
using UnityEngine;

namespace MalagaJam.Object
{
    enum DoorState
    {
        Locked,
        Unlocked
    }
    public class Door : MonoBehaviour
    {
        DoorState _State;
        [SerializeField] Generator[] generators;

        void Update()
        {
            int c = 0;
            foreach (Generator g in generators)
            {
                if (g.generatorState == GeneratorState.finished) c++;
            }

            if (c == generators.Length)
            {
                UnlockDoor();
            }
        }

        public void UnlockDoor()
        {
            _State = DoorState.Unlocked;
            GetComponent<Animator>().SetTrigger("Open");
        }
    }
}