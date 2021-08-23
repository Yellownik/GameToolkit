using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace ButtonListeners
{
	[ExecuteInEditMode]
	public class ChangeStateGroup : MonoBehaviour
	{
		[SerializeField] private List<ChangeStateElement> StateElements;

		[Space]
		[SerializeField] private ChangeStateEvent ChangeStateEvent;
		[SerializeField] private bool IsStateActive = true;

		public bool SelectedState { get; private set; }
		public bool LockedState { get; private set; }
		
		public void SetSelected(bool selected)
		{
			SelectedState = selected;
			BroadcastEvent(ChangeStateEvent.ElementSelected, selected);
		}
		
		public void SetLocked(bool locked)
		{
			LockedState = locked;
			BroadcastEvent(ChangeStateEvent.ElementLocked, LockedState);
		}
		
		private void BroadcastEvent(ChangeStateEvent evType, bool state)
		{
			foreach (var elementItem in StateElements)
				elementItem.ReceiveEvent(evType, state);
		}

		[Button]
		private void BroadcastEvent_Editor()
		{
			BroadcastEvent(ChangeStateEvent, IsStateActive);
			IsStateActive = !IsStateActive;
		}

		[Button]
		private void InitChangeElements()
		{
			gameObject.GetComponentsInChildren<ChangeStateElement>(true, StateElements);
		}
	}
}