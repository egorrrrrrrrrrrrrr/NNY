using Narratore.Localization;
using Narratore.Solutions.Windows;
using UnityEngine;

public class RecordResultWindow : UiWindow
{
    [SerializeField] private LocalizableLabel _scored;
    [SerializeField] private LocalizableLabel _record;


    public void Open(int scored, int record)
    {
        _scored.ChangeParams(scored.ToString());
        _record.ChangeParams(record.ToString());

        Open();
    }
}
