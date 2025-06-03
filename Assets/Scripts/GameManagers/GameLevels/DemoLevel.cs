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
        [SerializeField] private ButtonListener Button;
        [SerializeField] private Counter _counter;
        [SerializeField] private Player _player;
        
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
        
        private void Start()
        {
            Button.AddFunction(OnClick);
            _player.ObDragEnded += OnPlayerDragEnded;
        }

        private void OnPlayerDragEnded(bool isCollidedWithChair)
        {
            if (isCollidedWithChair)
            {
                _player.SetPosition(SitPoint.position);
                _player.SetDraggingActive(false);
            }
            else
                _player.SetPosition(PlayerSpawnPoint.position);
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
                    _player.gameObject.SetActive(true);
                    _player.SetPosition(PlayerSpawnPoint.position);
                    _player.Show();
                });
        }
        
        private void OnClick()
        {
            if (CustomersToEnd <= 0)
            {
                EndLevel();
            }
            else
            {
                Pay();
                CustomersToEnd--;
            }
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

            _player.Hide()
                .Done(() => SpawnPlayer());
        }
    }
}
