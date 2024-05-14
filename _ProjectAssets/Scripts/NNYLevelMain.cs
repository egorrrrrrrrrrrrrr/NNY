using Narratore;
using Narratore.Abstractions;
using Narratore.DI;
using Narratore.Helpers;
using Narratore.Interfaces;
using Narratore.MetaGame;
using Narratore.Solutions.Battle;
using Narratore.Solutions.Windows;
using Narratore.UI;
using System.Threading;
using UnityEngine;



public sealed class NNYLevelMain : LevelMain
{
    public NNYLevelMain(IUnitsWavesSpawner spawner,
                        LevelConfig config,
                        ShopWindow shopWindow,
                        LoseWindow loseWindow,
                        WinWindow winWindow,
                        IWithHp playerUnit,
                        WalletProvider wallet,
                        CurrencyDescriptor currency,
                        IsShootingWith2Hands isShootingWith2Hands,
                        GameStateEventsHandlers events,
                        ShieldResurrection shield,
                        TouchArea touchArea,
                        RecordResultWindow recordResultWindow,
                        RecordProvider recordCounter,
                        FullscreenAds fullscreenAds,
                        RewardedAds rewardedAds) : base(config, events)
    {
        _spawner = spawner;
        _loseConditionUnit = playerUnit;

        _loseWindow = loseWindow;
        _shopWindow = shopWindow;
        _winWindow = winWindow;

        _touchArea = touchArea;
        _touchArea.GettedInput += OnCatchedPlayerInput;

        _shopWindow.Open();
        _wallet = wallet;
        _currency = currency;
        _isShootingWith2Hands = isShootingWith2Hands;
        _shield = shield;
        _recordResultWindow = recordResultWindow;
        _recordCounter = recordCounter;
        _fullscreenAds = fullscreenAds;
        _rewardedAds = rewardedAds;
    }


    public override float Relation => 1f;


    private readonly IUnitsWavesSpawner _spawner;
    private readonly IWithHp _loseConditionUnit;
    private readonly TouchArea _touchArea;
    private readonly ShopWindow _shopWindow;
    private readonly LoseWindow _loseWindow;
    private readonly WinWindow _winWindow;
    private readonly RecordResultWindow _recordResultWindow;
    private readonly IsShootingWith2Hands _isShootingWith2Hands;
    private readonly ShieldResurrection _shield;
    private readonly IntProvider _recordCounter;
    private readonly WalletProvider _wallet;
    private readonly CurrencyDescriptor _currency;
    private readonly FullscreenAds _fullscreenAds;
    private readonly RewardedAds _rewardedAds;

    private CancellationTokenSource _cts;
    private bool _isEndedGame;


    protected override void BeginGameImpl()
    {
        _spawner.Dead += OnDeadEnemy;
        _loseConditionUnit.DecreasedHp += OnDecreasedHp;

        _shopWindow.Close();
        _spawner.Spawn();
    }

    private void OnCatchedPlayerInput()
    {
        if (Config.DeviceType == DeviceType.Handheld)
            _touchArea.gameObject.SetActive(false);

        _touchArea.GettedInput -= OnCatchedPlayerInput;
        CatchUserInput();
    }

    private async void OnDecreasedHp()
    {
        if (_isEndedGame) return;

        if (_loseConditionUnit.Hp.Current <= 0)
        {
            if (Config.Mode == null)
                LoseDefaultModeHandler();
            else
                LoseRecordModeHandler();
        }
    }

    private async void LoseRecordModeHandler()
    {
        PauseGame();

        _recordResultWindow.Open(_spawner.Killed, _recordCounter.Value);
        await _recordResultWindow.ShowingTask;

        if (_spawner.Killed > _recordCounter.Value)
            _recordCounter.Change(_spawner.Killed);

        //_isShootingWith2Hands.Set(false);

        ContinueGame();
        LosePlayer();
    }

    private async void LoseDefaultModeHandler()
    {
        bool isAviableAds = Application.isEditor ? true : _rewardedAds.IsCanShow();

        _loseWindow.Open(isAviableAds);
        PauseGame();

        LoseWindow.Result result = await _loseWindow.LoseWindowTask;
        if (result == LoseWindow.Result.Resurrect)
        {
            if (_rewardedAds.TryShow())
                await _rewardedAds.ShowingTask;

            _loseConditionUnit.Hp.Maximize();

            _shield.Create();
            ContinueGame();
        }
        else
        {
            _isEndedGame = true;

            if (_fullscreenAds.TryShow())
                await _fullscreenAds.ShowingTask;

            // Need return time scale before go to main menu
            ContinueGame();
            LosePlayer();

            //_isShootingWith2Hands.Set(false);
        }

        _loseWindow.Close();
    }

    private async void OnDeadEnemy(IWithId unit)
    {
        if (_isEndedGame || Config.Mode != null) return;

        if (_spawner.LivingCount == 0 && _spawner.LeftSpawnCount == 0)
        {
            _isEndedGame = true;
            _cts = new CancellationTokenSource();

            bool isCanceled = await UniTaskHelper.Delay(3f, _cts.Token);
            if (isCanceled) return;

            bool isAviableAds = Application.isEditor ? true : _rewardedAds.IsCanShow();
            _winWindow.Open(isAviableAds);
            PauseGame();

            WinWindow.Result result = await _winWindow.WinWindowTask;
            int award = 100;
            if (result == WinWindow.Result.AdsContinue)
            {
                if (_rewardedAds.TryShow())
                    await _rewardedAds.ShowingTask;

                award = 300;
            }

            _winWindow.CoinsFlyer.ToFly(award);

            isCanceled = await _winWindow.CoinsFlyer.FirstCoinCompleteTask.SuppressCancellationThrow();
            if (isCanceled) return;

            _wallet.TryApplyDelta(_currency, award);

            isCanceled = await _winWindow.CoinsFlyer.FullCompleteTask.SuppressCancellationThrow();
            if (isCanceled) return;

            
            isCanceled = await UniTaskHelper.Delay(1f, true, _cts.Token);
            if (isCanceled) return;

            if (result == WinWindow.Result.Continue && _fullscreenAds.TryShow())
                await _fullscreenAds.ShowingTask;

            _winWindow.Close();
            //_isShootingWith2Hands.Set(false);

            // Need return time scale before go to main menu
            ContinueGame();
            WinPlayer();
        }
    }


    public override void Dispose()
    {
        _touchArea.GettedInput -= OnCatchedPlayerInput;
        _loseConditionUnit.DecreasedHp -= OnDecreasedHp;
        _spawner.Dead -= OnDeadEnemy;

        _cts?.Dispose();
    }
}
