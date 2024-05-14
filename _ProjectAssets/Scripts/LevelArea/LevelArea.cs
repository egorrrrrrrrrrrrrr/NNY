using Narratore.Extensions;
using UnityEngine;

public class LevelArea : MonoBehaviour
{
    public Vector2 Size => _size;


    [SerializeField] private Vector2 _size;
    [SerializeField] private float _z = 0.01f;
    [SerializeField] private MeshFilter _line1;
    [SerializeField] private MeshFilter _line2;
    [SerializeField] private MeshFilter _line3;
    [SerializeField] private MeshFilter _line4;


    public void UpdateView()
    {
        Vector2 halfSize = _size / 2;
        Vector2[] corners = new Vector2[]
        {
            -halfSize,
            new Vector2(halfSize.x, -halfSize.y),
            halfSize,
            new Vector2(-halfSize.x, halfSize.y)
        };


        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 startPoint = corners[i].To3D(_z);
        }
    }
}
