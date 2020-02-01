using System;
using System.Collections.Generic;
using UnityEngine;

namespace MalagaJam.Object
{
    public class RepairBase : MonoBehaviour, IRepairable
    {
        List<GameObject> _ObjectsInRange = new List<GameObject>();

        void Start()
        {
        }

        void Update()
        {
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            _ObjectsInRange.Add(other.gameObject);
            print(_ObjectsInRange.Count);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            _ObjectsInRange.Remove(other.gameObject);
            print(_ObjectsInRange.Count);
        }

        public void Repair()
        {
        }
    }
}