using AudioSources;
using ButtonListeners;
using Core;
using Orbox.Async;
using UIWrappers;
using UnityEngine;

namespace GameManagers
{
    public class DemoLevel : MonoBehaviour
    {
        private enum State
        {
            WaitingForPlayer,
            StylingStarted,
        }
        
        [SerializeField] private ButtonListener Button;
        [SerializeField] private Counter _counter;
        [SerializeField] private Player _player;
        [SerializeField] private StyleProgress _styleProgress;
        
        [Space]
        [SerializeField] private int CustomersToEnd = 10;
        [SerializeField] private int InitialMoney = 5;
        [SerializeField] private int PriceForColoring = 1;
        [SerializeField] private int PaymentAmount = 10;
        [SerializeField] private Vector2 SpawnPlayerDelay = new Vector2(2, 5);
        
        [Space]
        [SerializeField] private Transform PlayerSpawnPoint;
        [SerializeField] private Transform SitPoint;
        
        [Header("Money")]
        [SerializeField] private Transform MoneySpawnPoint;
        [SerializeField] private float MoneySpawnRange = 0.5f;
        [SerializeField] private SpriteRendererWrapper Money;
        
        private FadeManager FadeManager => Root.FadeManager;
        private AudioManager AudioManager => Root.AudioManager;
        private ITimerService TimerService => Root.TimerService;

        private Promise EndLevelPromise = new Promise();
        
        private State _state = State.WaitingForPlayer;
        private int _colorChanges = 0;
        
        private void Start()
        {
            Button.AddFunction(OnButtonClick);
            _player.ObDragEnded += OnPlayerDragEnded;
        }

        private void OnPlayerDragEnded(bool isCollidedWithChair)
        {
            if (isCollidedWithChair)
            {
                _player.SetPosition(SitPoint.position);
                StartStyling();
            }
            else
            {
                _player.SetPosition(PlayerSpawnPoint.position);
            }
        }

        public IPromise StartLevel()
        {
            gameObject.SetActive(true);
            FadeManager.FadeIn();
            
            Init();
            
            return EndLevelPromise;
        }

        private void Init()
        {
            _state = State.WaitingForPlayer;
            Button.gameObject.SetActive(false);
            _styleProgress.Init();
            
            _counter.Init(InitialMoney);
            Money.gameObject.SetActive(false);
            
            _player.SetDraggingActive(false);
            _player.gameObject.SetActive(false);
            SpawnPlayer(1);
        }
        
        private void SpawnPlayer(float? delay = null)
        {
            delay ??= Random.Range(SpawnPlayerDelay.x, SpawnPlayerDelay.y);
            TimerService.Wait(delay.Value)
                .Done(() => 
                {
                    _player.SetPosition(PlayerSpawnPoint.position);
                    _player.gameObject.SetActive(true);
                    _player.Show()
                        .Done(() => _player.SetDraggingActive(true));
                });
        }
        
        private void StartStyling()
        {
            _state = State.StylingStarted;
            _colorChanges = 0;
            
            _player.SetDraggingActive(false);
            Button.gameObject.SetActive(true);

            _styleProgress.WaitForEnd()
                .Done(EndStyling);
        }
        
        private void EndStyling()
        {
            _state = State.WaitingForPlayer;
            Button.gameObject.SetActive(false);
            
            if (CustomersToEnd <= 0)
            {
                EndLevel();
                return;
            }

            CustomersToEnd--;
            
            if (_colorChanges > 0)
                Pay();
            
            _player.Hide()
                .Done(() => SpawnPlayer());
        }
        
        private void OnButtonClick()
        {
            if (_state is not State.StylingStarted)
                return;
            
            if (_counter.CanSpend(PriceForColoring))
            {
                _counter.SpendValue(PriceForColoring);
                _player.ChangeHairColor();
                _colorChanges++;
                
                AudioManager.PlaySound(ESounds.Click);
            };
        }

        private void EndLevel()
        {
            AudioManager.StopMusic(fadeTime: 1);

            FadeManager.ResetFadeCenter();
            FadeManager.FadeOut(duration: 1)
                .Done(() => EndLevelPromise.Resolve());
        }

        private void Pay()
        {
            var offset = Random.insideUnitSphere * MoneySpawnRange;
            Money.transform.position = MoneySpawnPoint.position + offset;

            _counter.AddValue(PaymentAmount);
            Money.gameObject.SetActive(true);
            Money.Show()
                .Then(Money.Hide)
                .Done(() => Money.gameObject.SetActive(false));
            
            AudioManager.PlaySound(ESounds.Money);
        }
    }
}
