using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DEngine
{
    public class InputController : MonoBehaviour
    {
        private PlayerController m_player;
        private Interactor m_interactor;
        private CarryController m_carry;
        private CameraControl m_cameraControl;

        public float VerticalSensibility = 1.0f;

        private void Start()
        {
            m_player = GetComponent<PlayerController>();
            m_interactor = GetComponent<Interactor>();
            m_carry = GetComponent<CarryController>();
            m_cameraControl = GetComponent<CameraControl>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 move = m_player.transform.forward * vertical + m_player.transform.right * horizontal;
            m_player.MoveTo(move, Mathf.Clamp01(move.magnitude));


            float forwardChange = Input.GetAxis("LookHorizontal");
            float angle = m_player.MaxRotationSpeed * Mathf.Deg2Rad * forwardChange;
            float sinAngle = Mathf.Sin(angle / 2.0f);
            float cosAngle = Mathf.Cos(angle / 2.0f);
            Quaternion rotation = new Quaternion(transform.up.x * sinAngle, transform.up.y * sinAngle, transform.up.z * sinAngle, cosAngle);
            Vector3 newForward = rotation * transform.forward;
            m_player.RotateToward(newForward, Mathf.Abs(forwardChange));

            float angleChange = Input.GetAxis("LookVertical") * VerticalSensibility;
            m_cameraControl.TargetAngle += angleChange;

            if (Input.GetButton("Jump"))
                m_player.Jump();

            if (m_carry.IsCarrying())
            {
                if (Input.GetButtonDown("Carry"))
                    m_carry.Release();
                if (Input.GetButtonDown("Interact"))
                    m_carry.Shoot();
            }
            else
            {

                if (Input.GetButtonDown("Carry"))
                    m_carry.Carry();

                if (Input.GetButtonDown("Interact"))
                    m_interactor.Interact();
            }
        }
    }
}