using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

namespace MalagaJam.Object
{
    public enum ObjectRepairState
    {
        Destroyed,
        Repairing,
        Repaired
    }

    public class RepairBase : MonoBehaviour, IRepairable
    {
        /** TODO
         * Set the repairing to go on a timer?
         */

        [Header("Object Settings")] [SerializeField]
        protected XboxButton repairButton; // Move this to a general Input script?

        [SerializeField] Sprite brokenSprite, repairedSprite;

        [Space] [Header("DEBUG")] [SerializeField]
        protected ObjectRepairState objectRepairState;

        protected readonly List<Player> ObjectsInRange = new List<Player>();
        SpriteRenderer _Sr;

        void Start()
        {
            if (!TryGetComponent(out _Sr))
            {
                Debug.LogError("This object does not have an spriterenderer!", gameObject);
            }
            else
            {
                _Sr.sprite = brokenSprite;
            }
        }

        protected virtual void Update()
        {
            // This is ugly af but in case we forget to remove this before release this will prevent it from getting into the build
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space) || (ObjectsInRange.Count > 0 && XCI.GetButtonDown(repairButton, ObjectsInRange[0].GetController())) && objectRepairState != ObjectRepairState.Repaired)
#else
            if ((ObjectsInRange.Count > 0 && XCI.GetButtonDown(repairButton, ObjectsInRange[0].GetController())) && objectRepairState != ObjectRepairState.Repaired)
#endif
            {
                Repair();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Player temp;
            if (other.TryGetComponent(out temp))
            {
                ObjectsInRange.Add(temp);
                print(ObjectsInRange.Count);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Player temp;
            if (other.TryGetComponent(out temp))
            {
                ObjectsInRange.Remove(temp);
                print(ObjectsInRange.Count);
            }
        }

        public virtual void Repair()
        {
            if (objectRepairState != ObjectRepairState.Destroyed)
                return; // When the object is repaired is is being repaired we simply return this function

            objectRepairState = ObjectRepairState.Repairing;
            objectRepairState = ObjectRepairState.Repaired;
            _Sr.sprite = repairedSprite;
        }
    }
}