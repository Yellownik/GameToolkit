using ButtonListeners;
using Core;
using UnityEngine;

namespace GameManagers
{
    public class DemoLevel : MonoBehaviour
    {
        [SerializeField] private ButtonListener Button;
        [SerializeField] private int ClicksToEnd = 10;
        [SerializeField] private float RandomRange = 100;
        [SerializeField] private bool IsUniqueFlyText = true;

        void Start()
        {
            Button.AddFunction(OnClick);
        }

        private void OnClick()
        {
            Root.FlyTextManager.Spawn(Button.transform, ClicksToEnd.ToString(), Random.insideUnitCircle * RandomRange, isUnique: IsUniqueFlyText);
            ClicksToEnd--;
        }
    }
}
