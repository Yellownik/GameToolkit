using Core;
using UnityEngine;

namespace GameManagers
{
    public class GameStarter : MonoBehaviour
    {
        void Start()
        {
            Root.TimerService.Wait(0.01f)
                .Done(() => Root.GameManager.Run());
        }
    }
}
