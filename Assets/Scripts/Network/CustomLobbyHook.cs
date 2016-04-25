using UnityEngine;
using System.Collections;
using UnityStandardAssets.Network;
using UnityEngine.Networking;

public class CustomLobbyHook : LobbyHook {
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        PlayerInformations infos = lobbyPlayer.GetComponent<PlayerInformations>();
        gamePlayer.GetComponent<PlayerInformations>().SetType(infos.Type);
    }
}
