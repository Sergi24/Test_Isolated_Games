using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using Photon.Pun;

public class PlayerControllerClass : MonoBehaviour
{
    PlayerControls controls;
    public float movementSpeed, pistolRadius;
    public float pistolRotationSpeed;
    public float limitMoveX, limitMoveY;

    private GameObject currentPistol;
    private double pistolAngle;
    private Vector2 move, aim;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Shot.performed += ctx => Shot();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Aim.performed += ctx => aim = ctx.ReadValue<Vector2>();
        controls.Gameplay.Aim.canceled += ctx => aim = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        pistolAngle = 0;
        currentPistol = PhotonNetwork.Instantiate("Pistol", transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CalculatePistolPosition();
    }

    void Shot()
    {
        Quaternion bulletRotation = Quaternion.AngleAxis((float)pistolAngle - 90, Vector3.forward);
        PhotonNetwork.Instantiate("Bullet", currentPistol.transform.position, bulletRotation);
    }

    void MovePlayer()
    {
        float moveX;
        float moveY;

        //screen limits
        if (transform.position.x < -limitMoveX && move.x < 0) moveX = 0;
        else if (transform.position.x > limitMoveX && move.x > 0) moveX = 0;
        else moveX = movementSpeed * move.x * Time.deltaTime;

        if (transform.position.y < -limitMoveY && move.y < 0) moveY = 0;
        else if (transform.position.y > limitMoveY && move.y > 0) moveY = 0;
        else moveY = movementSpeed * move.y * Time.deltaTime;

        transform.Translate(new Vector3(moveX, moveY));
    }

    void CalculatePistolPosition()
    {
        if (aim != Vector2.zero)
        {
            double angleDestination = Math.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            float timeFraction = pistolRotationSpeed * Time.deltaTime;
            //force range [0..360]
            if (angleDestination < 0) angleDestination += 360;
            double absDistance = Math.Abs(angleDestination - pistolAngle);
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

        SetPistolTransform();
    }

    void SetPistolTransform()
    {
        //set transform
        Transform pistolTransform = currentPistol.GetComponent<Transform>();
        float pistolX = (float)Math.Cos(pistolAngle * Mathf.Deg2Rad);
        float pistolY = (float)Math.Sin(pistolAngle * Mathf.Deg2Rad);
        Vector3 pistolDirection = new Vector3(pistolX, pistolY);
        pistolTransform.position = transform.position + (pistolRadius * pistolDirection);
        pistolTransform.rotation = Quaternion.AngleAxis((float)pistolAngle, Vector3.forward);

        //set sprite orientation
        SpriteRenderer pistolSprite = currentPistol.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (pistolAngle < 90 || pistolAngle > 270) pistolSprite.flipY = false;
        else pistolSprite.flipY = true;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
