using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy{
    Boolean hasDetectedByPlayer{get; set;}
    GameObject currentGameObject{get;}

    Rigidbody2D currentRigidbody2D{get;}
}
