using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerControllerClass : MonoBehaviour
{
    PlayerControls controls;
    public float movementSpeed;
    public float limitMoveX, limitMoveY;
    public GameObject textYou;

    private GameObject currentPistol;
    private Vector2 move, aim;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Aim.performed += ctx => aim = ctx.ReadValue<Vector2>();
        controls.Gameplay.Aim.canceled += ctx => aim = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        textYou.SetActive(true);
        currentPistol = PhotonNetwork.Instantiate("Pistol", transform.position, Quaternion.identity);
        currentPistol.GetComponent<PistolBehaviour>().SetPlayer(gameObject);
        
        controls.Gameplay.Shot.performed += ctx => currentPistol.GetComponent<PistolBehaviour>().Shot();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CalculatePistolPosition();
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
        PistolBehaviour pistolBehaviour = currentPistol.GetComponent<PistolBehaviour>();
        pistolBehaviour.CalculatePistolAngle(aim);
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
