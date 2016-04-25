using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Network;

public class PlayerInformations : NetworkBehaviour {

    public enum PlayerType
    {
        SCIENTIST,
        ROBOT,
    }

    [SyncVar(hook = "OnTypeSync")]
    private PlayerType m_type;

    public PlayerType Type
    {
        get { return m_type; }
        set { m_type = value; UpdateLobbyThing(); }
    }

    private void UpdateLobbyThing()
    {
        LobbyPlayer player = GetComponent<LobbyPlayer>();
        if(player != null)
            player.UpdateDisplayType(Type == PlayerType.SCIENTIST ? "Scientist" : "Robot");
    }

    public void SetType(PlayerType _type)
    {
        Type = _type;
    }

    private void OnTypeSync(PlayerType _type)
    {
        Type = _type;
    }
}
