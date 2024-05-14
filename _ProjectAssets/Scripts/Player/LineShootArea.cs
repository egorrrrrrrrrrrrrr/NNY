using Narratore.Data;
using Narratore.WorkWithMesh;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class LineShootArea : ShootArea
{
    [SerializeField] private ReadValue<float> _maxDistance;
    [SerializeField] private float _width;
    [SerializeField] private BoxCollider _collider;

    private bool _isInited;


    public override void Show() => 
        _area.NewMesh(new MeshShootingAreaLineConfig(_maxDistance.Get(), _width));

    public override bool IsInside(Vector3 point, float radius)
    {
        TryInit();
        Vector3 closest = _collider.ClosestPoint(point);
        float toColliderSqrDistance = (closest - point).sqrMagnitude;

        return toColliderSqrDistance < radius * radius;
    }


    private void TryInit()
    {
        if (_isInited) return;

        Vector3 size = new Vector3(_width, 1, _maxDistance.Get());

        _collider.size = size;
        _collider.center = new Vector3(0, 0, _maxDistance.Get() / 2);
    }
}
