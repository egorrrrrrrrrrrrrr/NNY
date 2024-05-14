using Narratore.Abstractions;
using Narratore.Components;
using UnityEngine;

public class SecondHandState : Switcheble<SecondHandState.StateKey> 
{
    [SerializeField] private TwoLegsLoopedRotators _animator;
    [SerializeField] private LoopedRotator _hand;

    [Header("ROTATIONS")]
    [SerializeField] private Transform _shoulder;
    [SerializeField] private Transform _elbow;
    [SerializeField] private Vector3 _shoulderRotate;
    [SerializeField] private Vector3 _elbowRotate;
    [SerializeField] private Vector3 _freeShoulderRotate;
    [SerializeField] private Vector3 _freeElbowRotate;


    protected override void ChangedState(StateKey state)
    {
        if (state == StateKey.Free)
        {
            _animator.SetHand(_hand);
            _shoulder.localRotation = Quaternion.Euler(_freeShoulderRotate);
            _elbow.localRotation = Quaternion.Euler(_freeElbowRotate);
        }
        else
        {
            _animator.SetHand(null);
            _shoulder.localRotation = Quaternion.Euler(_shoulderRotate);
            _elbow.localRotation = Quaternion.Euler(_elbowRotate);
        }
    }

    public enum StateKey
    {
        Free,
        WithGun
    }
}
