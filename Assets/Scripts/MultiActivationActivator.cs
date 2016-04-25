using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DEngine
{
    public class MultiActivationActivator : MonoBehaviour, IActivable
    {
        [SerializeField]
        private GameObject[] Targets;

        public void Activate()
        {
            foreach(var target in Targets)
            {
                target.GetComponent<IActivable>().Activate();
            }
        }

        public void Deactivate()
        {
            foreach (var target in Targets)
            {
                target.GetComponent<IActivable>().Deactivate();
            }
        }
    }
}
