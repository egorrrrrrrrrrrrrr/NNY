using Narratore.CameraTools;
using Narratore.Enums;
using Narratore.Extensions;
using Narratore.WorkWithMesh;
using System;
using UnityEngine;


public interface IPlayerUnitRotator
{
    Vector3 Forward { get; }


    void Rotate(Vector3 forward);
}



public interface ICameraMover
{
    void UpdateCameraPos();
}


public class PlayerCharacterMover : IPlayerUnitRotator, IPlayerLastMoveDirection, ICameraMover
{
    public event Action Moved;


    public PlayerCharacterMover(IPlayerMovableUnit unit, ICurrentCameraGetter camera, MeshFrame area)
    {
        _unit = unit;
        _camera = camera;
        _cameraOffset = _camera.Position - _unit.Root.position;

        _levelAreaBounds = new Bounds(area.transform.position, area.Size.To3D(TwoAxis.XZ, 10f));
        LastMoveDirection = Vector3.forward;
    }


    public Vector3 LastMoveDirection { get; private set; }
    public Vector3 Forward => _unit.Root.forward;




    private readonly IPlayerMovableUnit _unit;
    private readonly ICurrentCameraGetter _camera;
    private readonly Vector3 _cameraOffset;
    private readonly Bounds _levelAreaBounds;
    private bool _isMoving;
    

    public void UpdateCameraPos() => _camera.Transform.position = _unit.Root.position + _cameraOffset;
    public void SetInput(Vector2? input)
    {
        if (input.HasValue)
        {
            Vector3 direction = input.Value.To3D(TwoAxis.XZ, 0).normalized;
            Vector3 cachePosition = _unit.Root.position;
            Vector3 newPosition = _unit.Root.position + direction * _unit.MoveSpeed.Get() * Time.deltaTime;

            if (!_levelAreaBounds.Contains(newPosition))
                return;

            _unit.Root.position = newPosition;
            UpdateCameraPos();

            if (!_isMoving)
            {
                _unit.FootsAnimator.Enable();
                _isMoving = true;
            }

            LastMoveDirection = (_unit.Root.position - cachePosition).normalized;
            Moved?.Invoke();
        }
        else if (_isMoving)
        {
            _unit.FootsAnimator.Disable();
            _isMoving = false;
        }
    }

    public void Rotate(Vector3 forward)
    {
        _unit.Root.forward = forward;
    }
}
