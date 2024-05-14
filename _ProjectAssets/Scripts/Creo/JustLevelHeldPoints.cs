using Narratore.CameraTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class JustLevelHeldPoints : RandomOutCameraHeldPoints
{
    public JustLevelHeldPoints(RandomOutCameraHeldPointsConfig config, ICurrentCameraGetter camera) : base(config, camera)
    {
    }
}
