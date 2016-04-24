using UnityEngine;
using System.Collections;
using System;

namespace DEngine
{
    public class Door : MonoBehaviour, IActivable
    {

        private Animator m_animator;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        void Open(bool _open)
        {
            m_animator.SetBool("Open", _open);
        }

        public void Activate()
        {
            Open(true);
        }

        public void Deactivate()
        {
            Open(false);
        }
    }
}
