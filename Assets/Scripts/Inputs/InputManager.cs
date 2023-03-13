using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Inputs _inputs;

    private void Awake()
    {
        _inputs = new Inputs();
    }

    private void OnEnable()
    {
        var playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _inputs.Gameplay.Movement.performed += ctxMove =>
        {
            playerController.SetMoveDirection(ctxMove.ReadValue<Vector2>());
        };
        _inputs.Gameplay.Jump.performed += ctxJump =>
        {
            playerController.SetJump(ctxJump.ReadValueAsButton());
        };
        _inputs.Gameplay.Sprint.performed += ctxSprint =>
        {
            playerController.SetSprint(ctxSprint.ReadValueAsButton());
        };
        
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
}
