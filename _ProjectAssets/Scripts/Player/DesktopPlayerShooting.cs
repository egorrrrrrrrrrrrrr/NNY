using Narratore;
using Narratore.UI;

public class DesktopPlayerShooting : IBeginnedUpdatable
{
    public DesktopPlayerShooting(PlayerShooting shooting, TouchArea touchArea)
    {
        _shooting = shooting;
        _touchArea = touchArea;
    }


    private readonly PlayerShooting _shooting;
    private readonly TouchArea _touchArea;

    

    public void Tick()
    {
        _shooting.SetInput(_touchArea.IsHold);
    }
}
