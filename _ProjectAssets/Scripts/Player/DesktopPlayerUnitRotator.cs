using Narratore;
using Narratore.CameraTools;
using Narratore.Extensions;
using UnityEngine;


public class DesktopPlayerUnitRotator : IBeginnedUpdatable
{
    public DesktopPlayerUnitRotator(ICurrentCameraGetter camera, 
                                    IPlayerUnitRotator unitRotator, 
                                    IPlayerUnitShooting unitShooting, 
                                    LayerMask layerMask)
    {
        _camera = camera;
        _unitRotator = unitRotator;
        _unitShooting = unitShooting;
        _layerMask = layerMask;
    }


    private readonly ICurrentCameraGetter _camera;
    private readonly IPlayerUnitRotator _unitRotator;
    private readonly IPlayerUnitShooting _unitShooting;
    private readonly LayerMask _layerMask;
    

    public void Tick()
    {
        // Исходим из того, что террейн находиться в 0 точке и так как камера под углом 
        // то высоты недостаточно, поэтому умножаем на 2 - такой небольшой костыль)
        float distance = _camera.Transform.position.y * 2;
        Ray ray = _camera.Get.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, distance, _layerMask))
        {
            Vector3 direction = (hit.point - _unitShooting.Position).WithY(0).normalized;
            _unitRotator.Rotate(direction);
        }
    }
}
