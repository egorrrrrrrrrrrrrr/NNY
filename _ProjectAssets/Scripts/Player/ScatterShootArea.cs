using Narratore.Data;
using Narratore.WorkWithMesh;
using UnityEngine;
using Narratore.Solutions.Battle;
using Narratore.Extensions;
using Narratore.Enums;

public class ScatterShootArea : ShootArea
{
    [SerializeField] private ReadValue<float> _maxDistance;
    [SerializeField] private Gun _gun;


    public override void Show() =>
       _area.NewMesh(new MeshShootingAreaScatterConfig(_gun.CurrentShell.ScatterDegreezAmplitude.y / 2, _maxDistance.Get(), 10));

    public override bool IsInside(Vector3 point, float radius)
    {
        Vector3 nearesPoint = point + (Center.WithY(0) - point.WithY(0)).normalized * radius;
        Vector3 delta = nearesPoint.WithY(0) - Center.WithY(0);
        float sqrDistance = delta.sqrMagnitude;

        if (sqrDistance > _maxDistance.Get() * _maxDistance.Get()) 
            return false;

        Vector3 toTarget = delta.normalized;
        float angle = Vector3.Angle(toTarget, Forward.WithY(0));

        return angle < _gun.CurrentShell.ScatterDegreezAmplitude.y / 2;
    } 
}
