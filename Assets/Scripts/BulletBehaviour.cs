using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed, acceleration;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            Invoke("DestroyBullet", 2);
        }
    }

    void DestroyBullet()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Vector3.up * Time.deltaTime);
        //speed += acceleration * Time.deltaTime;
    }

    /*void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }*/
}
