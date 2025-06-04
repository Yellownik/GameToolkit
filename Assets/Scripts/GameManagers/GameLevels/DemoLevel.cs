using ButtonListeners;
using Core;
using FlyTexts;
using UnityEngine;

namespace GameManagers
{
    public class DemoLevel : BaseLevel
    {
        [SerializeField] private ButtonListener Button;
        
        [SerializeField] private int ClicksToEnd = 10;
        [SerializeField] private float RandomRange = 100;
        [SerializeField] private bool IsUniqueFlyText = true;
        
        private FlyTextManager FlyTextManager => Root.FlyTextManager;
        
        private void Start()
        {
            Button.AddFunction(OnClick);
        }
        
        private void OnClick()
        {
            if (ClicksToEnd <= 0)
            {
                EndLevel();
            }
            else
            {
                SpawnFlyText();
                ClicksToEnd--;
            }
        }

        private void SpawnFlyText()
        {
            var offset = Random.onUnitSphere * RandomRange;
            offset.z = Button.transform.position.z;

            FlyTextManager.Spawn(Button.GetComponent<RectTransform>(), ClicksToEnd.ToString(), offset, isUnique: IsUniqueFlyText);
        }
    }
}
