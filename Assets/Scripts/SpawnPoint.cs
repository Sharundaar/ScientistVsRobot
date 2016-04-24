using UnityEngine;
using System.Collections.Generic;
using PlayerState = PlayerController.PlayerState;
using System.Collections;

public class SpawnPoint : MonoBehaviour, IActivable {

    public enum SpawnPointType
    {
        HUMAN,
        ROBOT,
    }

    public bool LevelStart = false;
    public SpawnPointType Type
    {
        get { return m_type; }
        private set { m_type = value; }
    }

    [SerializeField]
    private SpawnPointType m_type = SpawnPointType.HUMAN;

	// Use this for initialization
	void Start () {
        if (LevelStart)
            Activate();
	}

    void OnPlayerStateChange(object _sender, PlayerState _old, PlayerState _new)
    {
        if(_new == PlayerState.DEAD)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(2.0f);

        PlayerController player = Type == SpawnPointType.HUMAN ? GameManager.Instance.Human : GameManager.Instance.Robot;
        player.Reset();
        player.transform.position = transform.position;
    }

	public void Activate()
	{
        if(Type == SpawnPointType.HUMAN)
        {
            GameManager.Instance.Human.StateChanged += OnPlayerStateChange;
        } else if(Type == SpawnPointType.ROBOT)
        {
            GameManager.Instance.Robot.StateChanged += OnPlayerStateChange;
        }

        GameManager.Instance.SetCurrentSpawnPoint(this);
	}

	public void Deactivate()
	{
        if (Type == SpawnPointType.HUMAN)
        {
            GameManager.Instance.Human.StateChanged -= OnPlayerStateChange;
        }
        else if (Type == SpawnPointType.ROBOT)
        {
            GameManager.Instance.Robot.StateChanged -= OnPlayerStateChange;
        }
    }
}
