using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPlayerInput : MonoBehaviour
{
    [Header("----- Keys -----")]
    public string keyUp;
    public string keyDown;
    public string keyRight;
    public string keyLeft;
    public string keyJump;
    public string keyDash;

    [Header("----- Signals -----")]
    public float dRight, dUp;
    public bool jump;
    public bool dash;
    public bool attack;

    void Start()
    {
        
    }


    void Update()
    {
        SetUpVirtualAxis();
        jump = Input.GetKeyDown(keyJump);
        dash = Input.GetKeyDown(keyDash);
        attack = Input.GetMouseButtonDown(0);
    }

    public void SetUpVirtualAxis()
    {
        dRight = (Input.GetKey(keyRight) ? 1f : 0) - (Input.GetKey(keyLeft) ? 1f : 0);
        dUp = (Input.GetKey(keyUp) ? 1f : 0) - (Input.GetKey(keyDown) ? -1f : 0);
    }
}
