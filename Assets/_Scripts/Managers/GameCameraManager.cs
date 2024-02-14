using UnityEngine;

public class GameCameraManager : PersistentSingleton<GameCameraManager>
{
    [SerializeField] private Camera _gameCamera = null;
    public Camera gameCamera => _gameCamera;
}