using Narratore.Abstractions;
using UnityEngine;

namespace Narratore.DI
{

    public enum InfoCanvasState
    {
        Disabled,
        DefaultCounter,
        RecordCounter
    }


    public class InfoCanvas : Switcheble<InfoCanvasState>
    {
        [SerializeField] private GameObject _defaultCounter;
        [SerializeField] private GameObject _recordCounter;



        protected override void ChangedState(InfoCanvasState state)
        {
            if (state == InfoCanvasState.DefaultCounter)
            {
                _defaultCounter.SetActive(true);
                _recordCounter.SetActive(false);
            }
            else if (state == InfoCanvasState.RecordCounter)
            {
                _defaultCounter.SetActive(false);
                _recordCounter.SetActive(true);
            }
            else
            {
                _defaultCounter.SetActive(false);
                _recordCounter.SetActive(false);
            }
        }
    }
}

