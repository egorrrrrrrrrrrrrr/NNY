using Narratore.Extensions;
using Narratore.WorkWithMesh;
using UnityEngine;


public abstract class ShootArea : MonoBehaviour
{
    protected Vector3 Center => _transform.position;
    protected Vector3 Forward => _transform.forward;


    [SerializeField] protected MeshShootingArea _area;
    [SerializeField] private Transform _transform;


    public abstract void Show();
    public abstract bool IsInside(Vector3 point, float sqrRadius);
    public void SetPosition(Vector3 point) => _transform.position = point;  
}
