using UnityEngine;

[System.Serializable]
public class CreoSpiralPlayerUnitConfig
{
    public CreoSpiralPlayerUnitConfig(float rotateSpeed, float startDelay, bool isCanMove)
    {
        _rotateSpeed = rotateSpeed;
        _startDelay = startDelay;
        _isCanMove = isCanMove;
    }


    public float RotateSpeed => _rotateSpeed;
    public float StartDelay => _startDelay;
    public bool IsCanMove => _isCanMove;


    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _startDelay;
    [SerializeField] private bool _isCanMove;
}
