using Core;
using Saving;
using UnityEngine;

namespace UI.Menus
{
	public class TitresMenuView : BaseMenuView
	{
		[SerializeField] private Counter _counter;

		private ISaveManager SaveManager => Root.SaveManager;
		
		public void ShowCounter()
		{
			var canShow = CanShowCounter();
			_counter.gameObject.SetActive(canShow);
			
			if (canShow)
				_counter.Init(SaveManager.BeautySaveData.CoinsCount);
		}
		
		private bool CanShowCounter() 
		{
			return SaveManager.HasSavedData && 
			       SaveManager.BeautySaveData.CoinsCount > 0;
		}
	}
}
