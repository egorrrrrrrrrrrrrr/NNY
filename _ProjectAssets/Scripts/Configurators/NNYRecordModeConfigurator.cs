using Narratore.Interfaces;
using Narratore.Localization;
using Narratore.Pools;
using Narratore.Solutions.Battle;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Narratore.DI
{

    public class ScoreUiObserver : IDisposable, IInitializable
    {
        public ScoreUiObserver(IUnitsWavesSpawner spawner, LocalizableLabel label)
        {
            _spawner = spawner;
            _label = label;
        }


        public void Initialize()
        {
            _spawner.Dead += OnKilled;
            OnKilled(null);
        }

        public void Dispose()
        {
            _spawner.Dead -= OnKilled;
        }


        private readonly IUnitsWavesSpawner _spawner;
        private readonly LocalizableLabel _label;

       

        private void OnKilled(IWithId _)
        {
            _label.ChangeParams(_spawner.Killed.ToString());
        }
    }


    public class NNYRecordModeConfigurator : LevelConfigurator
    {
        [SerializeField] private RecordResultWindow _window;
        [SerializeField] private RecordProvider _record;
        [SerializeField] private LocalizableLabel _scoreLabel;


        public override void Configure(IContainerBuilder builder, LevelConfig config, SampleData sampleData)
        {
            builder.RegisterInstance(_window);
            builder.RegisterInstance(_record);

            builder.RegisterEntryPoint<ScoreUiObserver>(Lifetime.Singleton).WithParameter(_scoreLabel);
        }
    }
}

