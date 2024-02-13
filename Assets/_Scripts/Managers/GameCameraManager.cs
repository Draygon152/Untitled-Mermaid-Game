using UnityEngine;

public class GameCameraManager : SceneSingleton<GameCameraManager>
{
    [SerializeField] private Camera _gameCamera = null;
    public Camera gameCamera => _gameCamera;
}