using UnityEngine;
using System.Collections.Generic;

namespace Orbox.Signals
{
    public class MBEventPublisher : MonoBehaviour, IEventPublisher
    {
        private List<IUpdatable> List = new List<IUpdatable>();

        public void Add(IEventSubscriber subscriber)
        {
            if(subscriber is IUpdatable updatable)
                List.Add(updatable);

            //TODO: add more events
        }

        private void Update()
        {
            // it is possible when new subscriber would be added while loop is not completed
            // so foreach cannot be used

            for (int i = 0; i < List.Count; i++)
            {
                List[i].Update();
            }
        }
    }
}
