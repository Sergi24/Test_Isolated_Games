using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int life;
    public Sprite heart;
    public Color heartInitialColor;
    public GameObject heartPosition1, heartPosition2, heartPosition3;

    private GameObject lastBulletHit;

    // Start is called before the first frame update
    void Start()
    {
        setHeartsColor(true, true, true);
    }


    void setHeartColor(GameObject gameObject, bool actived)
    {
        if (actived) gameObject.GetComponent<SpriteRenderer>().color = heartInitialColor;
        else gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }

    void setHeartsColor(bool heart1, bool heart2, bool heart3)
    {
        setHeartColor(heartPosition1, heart1);
        setHeartColor(heartPosition2, heart2);
        setHeartColor(heartPosition3, heart3);
    }

    [PunRPC]
    void RPCRemoveLife(int photonView)
    {
        PhotonView bulletPhotonView = PhotonNetwork.GetPhotonView(photonView);
        if (bulletPhotonView != null && bulletPhotonView.IsMine)
        {
            PhotonNetwork.Destroy(bulletPhotonView);
        }

        life -= 1;

        Debug.Log("life " + life);

        if (life == 2) setHeartsColor(true, true, false);
        else if (life == 1) setHeartsColor(true, false, false);
        else if (life <= 0)
        {
            setHeartsColor(false, false, false);
            GameManager.instance.GameOverMode();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            if (lastBulletHit != collider.gameObject)
            {
                if (collider.gameObject.tag == "Bullet")
                {
                    lastBulletHit = collider.gameObject;

                    Debug.Log("COLLIDER LIFE MANAGER");
                    PhotonView PV = GetComponent<PhotonView>();
                    int bulletPhotonViewId = collider.gameObject.GetComponent<PhotonView>().ViewID;
                    PV.RPC("RPCRemoveLife", RpcTarget.All, bulletPhotonViewId);
                }
            }
        }
    }
}
