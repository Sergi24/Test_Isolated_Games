using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtOtherPlayer : MonoBehaviour
{
    public GameObject spriteLplayer1, spriteRplayer1;
    public GameObject spriteLplayer2, spriteRplayer2;

    [PunRPC]
    void RPCSetOtherPlayer(int playerPhotonId)
    {
        PhotonView otherPlayerPhotonView = PhotonNetwork.GetPhotonView(playerPhotonId);
        if (otherPlayerPhotonView != null)
        {
            GameManager.instance.photonManager.SetOtherPlayer(otherPlayerPhotonView.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject otherPlayer;
        if (GetComponent<PhotonView>().IsMine)
        {
            otherPlayer = GameManager.instance.photonManager.GetOtherPlayer();
            if (GameManager.instance.photonManager.GetNumPlayer() == 1)
            {
                LookOtherPlayer(otherPlayer, spriteRplayer1, spriteLplayer1);
            }
            else
            {
                LookOtherPlayer(otherPlayer, spriteRplayer2, spriteLplayer2);
            }
        }
        else
        {
            otherPlayer = GameManager.instance.photonManager.GetPlayer();
            if (GameManager.instance.photonManager.GetNumPlayer() == 1)
            {
                LookOtherPlayer(otherPlayer, spriteRplayer2, spriteLplayer2);
            }
            else
            {
                LookOtherPlayer(otherPlayer, spriteRplayer1, spriteLplayer1);
            }
        }
    }

    void LookOtherPlayer(GameObject otherPlayer, GameObject spriteR, GameObject spriteL) 
    {
        if (otherPlayer != null)
        {
            //Debug.Log(otherPlayer.transform.position.x - transform.position.x);
            if ((otherPlayer.transform.position.x - transform.position.x) > 0)
            {
                spriteR.SetActive(true);
                spriteL.SetActive(false);
            }
            else
            {
                spriteR.SetActive(false);
                spriteL.SetActive(true);
            }
        }
        else
        {
            spriteR.SetActive(true);
            spriteL.SetActive(false);
        }
    }
}
