using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBehaviour : MonoBehaviour
{
    PlayerControls controls;

    public GameObject pistolSpriteR, pistolSpriteL;
    public float pistolRadius;
    public float pistolRotationSpeed;

    private LineRenderer lineRenderer;
    private double pistolAngle;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        pistolAngle = 0;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //trajectory line
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        lineRenderer.SetPosition(0, transform.position);
        if (hit.collider == null) lineRenderer.SetPosition(1, transform.position + (20 * transform.right));
        else lineRenderer.SetPosition(1, hit.point);

        //set pistol sprite orientation
        if (pistolAngle < 90 || pistolAngle > 270)
        {
            pistolSpriteR.SetActive(true);
            pistolSpriteL.SetActive(false);
        }
        else
        {
            pistolSpriteR.SetActive(false);
            pistolSpriteL.SetActive(true);
        }

        SetPistolTransform();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void SetPistolTransform()
    {
        if (player)
        {
            Vector3 playerPosition = player.GetComponent<Transform>().position;
            Transform pistolTransform = GetComponent<Transform>();
            float pistolX = (float)Math.Cos(pistolAngle * Mathf.Deg2Rad);
            float pistolY = (float)Math.Sin(pistolAngle * Mathf.Deg2Rad);
            Vector3 pistolDirection = new Vector3(pistolX, pistolY);
            pistolTransform.position = playerPosition + (pistolRadius * pistolDirection);
            pistolTransform.rotation = Quaternion.AngleAxis((float)pistolAngle, Vector3.forward);
        }
    }

    public void CalculatePistolAngle(Vector2 aim)
    {
        if (aim != Vector2.zero)
        {
            double angleDestination = Math.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            if (angleDestination < 0) angleDestination += 360;

            float timeFraction = pistolRotationSpeed * Time.deltaTime;
            //force range [0..360]
            double absDistance = Math.Abs(angleDestination - pistolAngle);
            if (Math.Abs(angleDestination - pistolAngle) > 5)
            {
                if (absDistance < 180)
                {
                    pistolAngle = Mathf.Lerp((float)pistolAngle, (float)angleDestination, timeFraction);
                }
                else
                {
                    double realDistance = 360 - absDistance;
                    if (angleDestination - pistolAngle > 0)
                    {
                        pistolAngle -= realDistance * timeFraction;
                    }
                    else
                    {
                        pistolAngle += realDistance * timeFraction;
                    }
                }
                if (pistolAngle < 0) pistolAngle += 360;
                pistolAngle %= 360;
                //Debug.Log(new Vector2((float)pistolAngle, (float)angleDestination));
            }

            GetComponent<PhotonView>().RPC("RPCSetPistolAngle", RpcTarget.Others, pistolAngle);
        }
    }

    [PunRPC]
    void RPCSetPistolAngle(double pistolAngle)
    {
        this.pistolAngle = pistolAngle;
    }

    /*[PunRPC]
    void RPCInstantiateBullet(Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate("Bullet", position, rotation);
    }*/

    public void Shot()
    {
        Quaternion bulletRotation = Quaternion.AngleAxis((float)pistolAngle - 90, Vector3.forward);
        PhotonNetwork.Instantiate("Bullet", transform.position, bulletRotation);
        //GetComponent<PhotonView>().RPC("RPCInstantiateBullet", RpcTarget.All, transform.position, bulletRotation);
    }
}
