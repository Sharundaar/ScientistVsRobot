using UnityEngine;
using System.Collections;
using UnityStandardAssets.Network;
using UnityEngine.Networking;

public class CustomLobbyManager : LobbyManager {

    private GameObject Scientist;
    private GameObject Robot;

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject obj =  base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);

        PlayerInformations player = obj.GetComponent<PlayerInformations>();
        if(Scientist == null)
        {
            player.SetType(PlayerInformations.PlayerType.SCIENTIST);
            Scientist = obj;
        } else if(Robot == null)
        {
            player.SetType(PlayerInformations.PlayerType.ROBOT);
            Robot = obj;
        }

        return obj;
    }
}
