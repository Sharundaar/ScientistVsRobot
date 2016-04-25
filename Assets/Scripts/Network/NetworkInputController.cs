using UnityEngine;
using System.Collections;
using DEngine;
using UnityEngine.Networking;
using System;

public class NetworkInputController : NetworkBehaviour {

    private PlayerInformations m_informations;

    private DEngine.PlayerController m_player;
    private Interactor m_interactor;
    private CarryController m_carry;
    private CameraControl m_cameraControl;

    [SerializeField]
    private float VerticalSensibility = 1.0f;

    [SyncVar(hook = "DirectionChanged")]
    Vector3 r_direction;

    [SyncVar(hook = "ForwardChanged")]
    Vector4 r_forward;

    [SyncVar(hook = "AngleChanged")]
    float r_angle;

    [SyncVar(hook = "CarryChanged")]
    bool r_carry;

    [SyncVar(hook = "InteractChanged")]
    bool r_interact;

    [SyncVar(hook = "JumpChanged")]
    bool r_jump;

    private void Start()
    {
        m_informations = GetComponent<PlayerInformations>();
    }

    private void Update()
    {
        if (m_player == null)
            Setup();

        else if(isLocalPlayer)
        {
            bool jumped, carry, interact;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 newDirection = m_player.transform.forward * vertical + m_player.transform.right * horizontal;
            m_player.MoveTo(newDirection, Mathf.Clamp01(newDirection.magnitude));

            float forwardChange = Input.GetAxis("LookHorizontal");
            float angle = m_player.MaxRotationSpeed * Mathf.Deg2Rad * forwardChange;
            float sinAngle = Mathf.Sin(angle / 2.0f);
            float cosAngle = Mathf.Cos(angle / 2.0f);
            Quaternion rotation = new Quaternion(m_player.transform.up.x * sinAngle, m_player.transform.up.y * sinAngle, m_player.transform.up.z * sinAngle, cosAngle);
            Vector3 newForward = rotation * m_player.transform.forward;
            m_player.RotateToward(newForward, Mathf.Abs(forwardChange));

            float newAngle = m_cameraControl.TargetAngle + Input.GetAxis("LookVertical") * VerticalSensibility;
            m_cameraControl.TargetAngle = newAngle;

            if (jumped = Input.GetButton("Jump"))
                m_player.Jump();

            if (m_carry.IsCarrying())
            {
                if (carry = Input.GetButtonDown("Carry"))
                    m_carry.Release();
                if (interact = Input.GetButtonDown("Interact"))
                    m_carry.Shoot();
            }
            else
            {

                if (carry = Input.GetButtonDown("Carry"))
                    m_carry.Carry();

                if (interact = Input.GetButtonDown("Interact"))
                    m_interactor.Interact();
            }

            // sync
            if(isServer)
            {
                r_direction = newDirection;
                r_forward = new Vector4(newForward.x, newForward.y, newForward.z, forwardChange);
                r_carry = carry;
                r_jump = jumped;
                r_interact = interact;
                r_angle = newAngle;
            }
            else
            {
                CmdSendData(newDirection,
                    new Vector4(newForward.x, newForward.y, newForward.z, forwardChange), 
                    carry, jumped, interact, newAngle);
            }
        }

    }

    [Command]
    private void CmdSendData(Vector3 _direction, Vector4 _forward, bool _carry, bool _jump, bool _interact, float _angle)
    {
        r_direction = _direction;
        r_forward = _forward;
        r_carry = _carry;
        r_jump = _jump;
        r_interact = _interact;
        r_angle = _angle;

        if (!isLocalPlayer)
        {
            DirectionChanged(r_direction);
            ForwardChanged(r_forward);
            CarryChanged(r_carry);
            JumpChanged(r_jump);
            InteractChanged(r_interact);
            AngleChanged(r_angle);
        }
    }

    public void Setup()
    {
        if(m_informations.Type == PlayerInformations.PlayerType.SCIENTIST)
        {
            m_player = GameManager.Instance.Human;
            gameObject.name = "HumanController";
        }
        else if(m_informations.Type == PlayerInformations.PlayerType.ROBOT)
        {
            m_player = GameManager.Instance.Robot;
            gameObject.name = "RobotController";
        }

        m_interactor = m_player.GetComponent<Interactor>();
        m_carry = m_player.GetComponent<CarryController>();
        m_cameraControl = m_player.GetComponent<CameraControl>();

        if(!isLocalPlayer)
        {
            m_player.GetComponentInChildren<Camera>().enabled = false;
            m_player.GetComponentInChildren<AudioListener>().enabled = false;
        }
        else
        {
            m_player.GetComponentInChildren<Camera>().enabled = true;
            m_player.GetComponentInChildren<AudioListener>().enabled = true;
        }
    }

    private void DirectionChanged(Vector3 _new)
    {
        m_player.MoveTo(_new, Mathf.Clamp01(_new.magnitude));
    }

    private void ForwardChanged(Vector4 _new)
    {
        Vector3 forward = new Vector3(_new.x, _new.y, _new.z);
        m_player.RotateToward(forward, Mathf.Abs(_new.w));
    }

    private void AngleChanged(float _new)
    {
        if (isLocalPlayer)
            return;

        m_cameraControl.TargetAngle = _new;
    }

    private void CarryChanged(bool _new)
    {
        if (m_carry.IsCarrying())
        {
            if (_new)
                m_carry.Release();
        }
        else
        {

            if (_new)
                m_carry.Carry();
        }
    }

    private void InteractChanged(bool _new)
    {
        if (m_carry.IsCarrying())
        {
            if (_new)
                m_carry.Shoot();
        }
        else
        {

            if (_new)
                m_interactor.Interact();
        }
    }

    private void JumpChanged(bool _new)
    {
        if (_new)
            m_player.Jump();
    }
}
