using UnityEngine;
using System.Collections;

using PlayerState = PlayerController.PlayerState;

public class PlayerAnimationController : MonoBehaviour {

    private Animator m_animator;
    private PlayerController m_player;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_player = GetComponent<PlayerController>();

        m_player.StateChanged += OnPlayerStateChanged;
    }

    private void OnPlayerStateChanged(object _sender, PlayerState _oldState, PlayerState _newState)
    {
        switch(_newState)
        {
            case PlayerState.WALKING:
                m_animator.SetBool("Walking", true);
                break;

            case PlayerState.JUMPING:
                m_animator.SetTrigger("Jump");
                break;

            case PlayerState.DEAD:
                m_animator.SetTrigger("Die");
                break;

            default:
                m_animator.SetBool("Walking", false);
                break;
        }

        if (_newState == PlayerState.IDLE && _oldState == PlayerState.DEAD)
            m_animator.SetTrigger("Resurect");
    }

    private void Update()
    {
        if(m_player.State == PlayerState.WALKING)
        {
            float blend = (Vector3.Dot(m_player.transform.right, m_player.Speed.normalized) + 1.0f) / 2.0f; // getting blend between 0 and 1
            m_animator.SetFloat("WalkBlend", blend);
        }

    }
}
