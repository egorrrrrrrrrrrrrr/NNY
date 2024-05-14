using Narratore;
using Narratore.CameraTools;
using Narratore.Enums;
using Narratore.Extensions;
using UnityEngine;


public class SpiralPlayerUnitBot : IPreparedUpdatable
{
    public SpiralPlayerUnitBot(PlayerShooting shooting, CreoSpiralPlayerUnitConfig config, PlayerCharacterMover characterMover)
    {
        _shooting = shooting;

        _config = config;
        _rotation = Quaternion.Euler(0, 90f, 0);

        _delayCounter = config.StartDelay;
        _characterMover = characterMover;
    }


    private readonly PlayerShooting _shooting;
    private readonly PlayerCharacterMover _characterMover;
    private readonly CreoSpiralPlayerUnitConfig _config;
    private Quaternion _rotation;
    private float _delayCounter;

   

    public void Tick()
    {
        if (_delayCounter > 0)
        {
            _delayCounter -= Time.deltaTime;
            return;
        }

        _rotation = _rotation * Quaternion.Euler(0, _config.RotateSpeed * Time.deltaTime, 0);

        if (_config.IsCanMove)
            _characterMover.SetInput((_rotation * Vector3.forward).To2D(TwoAxis.XZ).normalized);
        _characterMover.Rotate(_rotation * Vector3.forward);

        _shooting.SetInput(true);
    }
}

public class CircleHeldPoints : IHeldPoints
{
    public CircleHeldPoints(RandomOutCameraHeldPointsConfig config, ICurrentCameraGetter camera, IPlayerUnitRoot root)
    {
        _config = config;
        _camera = camera;
        _rotationStep = Quaternion.Euler(0, 5, 0);
        _rotation = Quaternion.Euler(0, 90, 0);
        _root = root;
    }


    private readonly RandomOutCameraHeldPointsConfig _config;
    private readonly ICurrentCameraGetter _camera;
    private readonly Quaternion _rotationStep;
    private readonly IPlayerUnitRoot _root;
    private Quaternion _rotation;

    

    public IHeldPoint Get()
    {
        Vector3[] nearCorners;
        if (_camera.Get.orthographic)
            nearCorners = GetOrthographicCorners(_camera.Get.nearClipPlane);
        else
            nearCorners = GetPerspectiveCorners(_camera.Get.nearClipPlane);

        Vector3[] farCorners;
        if (_camera.Get.orthographic)
            farCorners = GetOrthographicCorners(_camera.Get.farClipPlane);
        else
            farCorners = GetPerspectiveCorners(_camera.Get.farClipPlane);


        Vector3 nearCorner0 = _camera.Transform.TransformPoint(nearCorners[0]);
        Vector3 nearCorner2 = _camera.Transform.TransformPoint(nearCorners[2]);
        Vector3 farCorner0 = _camera.Transform.TransformPoint(farCorners[0]);
        Vector3 farCorner2 = _camera.Transform.TransformPoint(farCorners[2]);

        Ray ray0 = new Ray(nearCorner0, (farCorner0 - nearCorner0).normalized);
        Ray ray2 = new Ray(nearCorner2, (farCorner2 - nearCorner2).normalized);
        Ray rayCenter = new Ray(_camera.Position, _camera.Forward);

        if (!Physics.Raycast(ray0, out RaycastHit hit0, _config.CameraToTerrainMaxDistance, _config.TerrainMask) ||
            !Physics.Raycast(ray2, out RaycastHit hit2, _config.CameraToTerrainMaxDistance, _config.TerrainMask) ||
            !Physics.Raycast(rayCenter, out RaycastHit hitCenter, _config.CameraToTerrainMaxDistance, _config.TerrainMask))
            throw new System.Exception("Not can cast from camera to terrain. Try increase parameter CameraToTerrainMaxDistance");

        Vector3 center = _root.Root.position;
        float cameraViewRadius = Mathf.Sqrt(Mathf.Max((hit2.point - center).sqrMagnitude, (hit0.point - center).sqrMagnitude));

        _rotation = _rotation * _rotationStep;

        Vector2 direction = (_rotation * Vector3.forward).To2D(TwoAxis.XZ);
        float radius = UnityEngine.Random.Range(cameraViewRadius, cameraViewRadius + _config.GenerateRingWidth);
        Vector3 point = center + direction.To3D(TwoAxis.XZ) * radius;
        Quaternion rotation = Quaternion.LookRotation((center - point).normalized);

        return new HeldPoint(point, rotation, true);
    }

    private Vector3[] GetOrthographicCorners(float z)
    {
        float height = 2f * _camera.Get.orthographicSize;
        float width = height * _camera.Get.aspect;
        Vector2 halfSize = new Vector2(width / 2, height / 2);

        return new Vector3[]
        {
            new Vector3(-halfSize.x, -halfSize.y, z),
            new Vector3(-halfSize.x, halfSize.y, z),
            new Vector3(halfSize.x, halfSize.y, z),
            new Vector3(halfSize.x, -halfSize.y, z)
        };
    }

    private Vector3[] GetPerspectiveCorners(float z)
    {
        Vector3[] corners = new Vector3[4];
        _camera.Get.CalculateFrustumCorners(new Rect(Vector2.zero, Vector2.one), z, Camera.MonoOrStereoscopicEye.Mono, corners);

        return corners;
    }
}
