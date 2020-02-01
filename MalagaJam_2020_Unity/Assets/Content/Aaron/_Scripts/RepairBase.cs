using System;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

namespace MalagaJam.Object
{
    enum ObjectRepairState
    {
        Destroyed,
        Repairing,
        Repaired
    }

    public class RepairBase : MonoBehaviour, IRepairable
    {
        /** TODO
         * Set the list to use the player object instead of a GameObject
         * When the player is done, grab the player controller id
         * Set the repairing to go on a timer?
         * 
         */

        [Header("Object Settings")] [SerializeField]
        XboxButton repairButton; // Move this to a general Input script?
        [Space] [Header("DEBUG")] [SerializeField]
        ObjectRepairState objectRepairState;

        protected readonly List<GameObject> ObjectsInRange = new List<GameObject>();

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(repairButton, XboxController.Any))
            {
                Repair();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            ObjectsInRange.Add(other.gameObject);
            print(ObjectsInRange.Count);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            ObjectsInRange.Remove(other.gameObject);
            print(ObjectsInRange.Count);
        }

        public virtual void Repair()
        {
            if (objectRepairState != ObjectRepairState.Destroyed)
                return; // When the object is repaired is is being repaired we simply return this function

            objectRepairState = ObjectRepairState.Repairing;
            objectRepairState = ObjectRepairState.Repaired;
        }
    }
}