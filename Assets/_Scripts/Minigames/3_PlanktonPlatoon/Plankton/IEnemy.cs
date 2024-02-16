using UnityEngine;

public interface IEnemy{
    bool hasDetectedByPlayer { get; set; }

    GameObject currentGameObject { get; }

    Rigidbody2D currentRigidbody2D { get; }
}