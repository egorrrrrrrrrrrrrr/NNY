namespace Narratore.DI
{
    public class InfoCanvasHandler : IBegunGameHandler
    {
        public InfoCanvasHandler(InfoCanvas canvas)
        {
            _canvas = canvas;
        }


        private readonly InfoCanvas _canvas;
       

        public void BegunGame(LevelConfig config)
        {
            if (config.Mode == null)
                _canvas.Switch(InfoCanvasState.DefaultCounter);
            else
                _canvas.Switch(InfoCanvasState.RecordCounter);
        }
    }
}

