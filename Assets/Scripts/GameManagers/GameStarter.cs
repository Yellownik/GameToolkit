using Core;
using UnityEngine;

namespace GameManagers
{
    public class GameStarter : MonoBehaviour
    {
        void Start()
        {
            Root.GameManager.Run();
        }
    }
}
