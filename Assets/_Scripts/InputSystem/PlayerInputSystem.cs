using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{

    private PlayerInputControl playerInputControl;
    void Awake(){
        playerInputControl = new PlayerInputControl();
        playerInputControl.playerMouseControl.Enable();
    }
    public Vector2 GetMouseAxisVector(){
        return playerInputControl.playerMouseControl.MouseAxis.ReadValue<Vector2>();
        
    }
}
