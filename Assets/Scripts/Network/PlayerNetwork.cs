using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    public MonoBehaviour[] codesToIgnore;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            foreach(MonoBehaviour code in codesToIgnore)
            {
                code.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
