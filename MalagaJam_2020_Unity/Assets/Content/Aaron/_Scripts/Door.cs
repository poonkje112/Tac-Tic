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

        public void UnlockDoor()
        {
            _State = DoorState.Unlocked;
        }
    }
}