using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DEngine
{
    public class AccumulatorActivator : MonoBehaviour, IActivable
    {
        [SerializeField]
        private GameObject Target;

        [SerializeField]
        private int Count;

        [SerializeField]
        private bool LockAfterActivation = false;

        private int m_currentCount = 0;
        private bool m_activated = false;

        public void Activate()
        {
            m_currentCount++;
            if (m_currentCount >= Count)
            {
                if(!m_activated)
                {
                    Target.GetComponent<IActivable>().Activate();
                    m_activated = true;
                }
            }
        }

        public void Deactivate()
        {
            m_currentCount--;
            if (m_currentCount < Count)
            {
                if(!LockAfterActivation && m_activated)
                {
                    Target.GetComponent<IActivable>().Deactivate();
                    m_activated = false;
                }
            }
        }
    }
}
