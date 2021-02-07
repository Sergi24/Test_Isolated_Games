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
    void RPCRemoveLife()
    {
        life -= 1;

        if (life == 2) setHeartsColor(true, true, false);
        else if (life == 1) setHeartsColor(true, false, false);
        else setHeartsColor(false, false, false);
    }

    void CallSetting()
    {
        PhotonView PV = GetComponent<PhotonView>();
        PV.RPC("RPCRemoveLife", RpcTarget.All);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bullet")
        {
            PhotonNetwork.Destroy(collider.gameObject);
            CallSetting();
        }
    }
}
