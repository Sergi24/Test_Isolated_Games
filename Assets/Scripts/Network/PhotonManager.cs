using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    GameObject player;

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

        GameManager.instance.MenuMode();
    }

    public void JoinRoom()
    {
        if (CanCreateRoom()) 
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default); ;
    }

    public bool CanCreateRoom()
    {
        return PhotonNetwork.CurrentLobby != null;
    }

    public override void OnJoinedRoom()
    {
        player = PhotonNetwork.Instantiate("Player", new Vector2(0, 0), Quaternion.identity);
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.CurrentRoom != null) PhotonNetwork.LeaveRoom();
    }
}
