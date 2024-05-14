using Narratore;
using Narratore.Input;
using UnityEngine;

public class PlayerCharacterJoystickMover : IBeginnedUpdatable
{
    public PlayerCharacterJoystickMover(PlayerCharacterMover mover, Joystick joystick)
    {
        _mover = mover;
        _joystick = joystick;
    }



    private readonly PlayerCharacterMover _mover;
    private readonly Joystick _joystick;

    

    public void Tick()
    {
        if (_joystick.TryMoveStick(out Vector2 offset, true))
            _mover.SetInput(offset);
        else
            _mover.SetInput(null);
    }
}
