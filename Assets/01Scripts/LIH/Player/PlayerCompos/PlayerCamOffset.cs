using UnityEngine;

public class PlayerCamOffset : MonoBehaviour , IPlayerCompo
{
    [SerializeField] private GameEventChannelSO _cameraEventChannel;
    [SerializeField] private float _radius = 1.5f;
    private Player _player;
    
    public void Initialize(Player player)
    {
        _player = player;
        _player.PlayerInput.MouseMoveEvent += HandleMouseMove;
    }

    private void OnDestroy()
    {
        _player.PlayerInput.MouseMoveEvent -= HandleMouseMove;
    }

    private void HandleMouseMove(Vector2 mousePos)
    {
        var evt = CameraEvents.CamOffsetChangeEvent;
        evt.radius = _radius;
        evt.postion = _player.transform.position;
        evt.targetPos = mousePos;
        _cameraEventChannel.RaiseEvent(evt);
    }
}
