using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    GameObject player;
    GameObject otherPlayer;
    int numPlayer;

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Update()
    {

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
        return PhotonNetwork.IsConnectedAndReady && PhotonNetwork.CurrentLobby != null;
    }

    public override void OnJoinedRoom()
    {
        player = PhotonNetwork.Instantiate("Player", new Vector2(0, 0), Quaternion.identity);
        numPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonView playerPhotonView = player.GetComponent<PhotonView>();
        playerPhotonView.RPC("RPCSetOtherPlayer", RpcTarget.OthersBuffered, playerPhotonView.ViewID);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public int GetNumPlayer()
    {
        return numPlayer;
    }

    public GameObject GetOtherPlayer()
    {
        return otherPlayer;
    }

    public void SetOtherPlayer(GameObject otherPlayer)
    {
        this.otherPlayer = otherPlayer;
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.CurrentRoom != null) PhotonNetwork.LeaveRoom();
    }
}
