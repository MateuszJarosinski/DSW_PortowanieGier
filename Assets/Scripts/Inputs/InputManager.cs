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
        var player = GameObject.FindGameObjectWithTag("Player");

        _inputs.Gameplay.Movement.performed += ctxMove =>
        {
            Debug.Log(ctxMove.ReadValue<Vector2>());
        };
        _inputs.Gameplay.Jump.performed += ctxJump =>
        {
            Debug.Log(ctxJump.ReadValueAsButton());
        };
        _inputs.Gameplay.Sprint.performed += ctxSprint =>
        {
            Debug.Log(ctxSprint.ReadValueAsButton());
        };
        
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
}
