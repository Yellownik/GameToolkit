using Core;
using UnityEngine;

namespace GameManagers
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private EGameLevels _gameLevel = EGameLevels.DemoLevel;
        
        private void Start()
        {
            Root.GameManager.Run(_gameLevel);
        }
    }
}
