using Orbox.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace FlyTexts
{
	public class FlyTextManager
	{
		private const int SortingOrderOffset = 5;
		private readonly IResourceManager ResourceManager;

		private FlyText UniqueFlyText;
		private List<FlyText> ActiveFlyTexts = new List<FlyText>();

		public FlyTextManager(IResourceManager resourceManager)
		{
			ResourceManager = resourceManager;
		}

		public void Spawn(Transform parent, string text, Vector3 localPos = default, float scale = 1, float time = 6, bool isUnique = true)
		{
			if (isUnique && UniqueFlyText != null)
				UniqueFlyText.Cancel();

			var flyText = ResourceManager.GetFromPool<EFlyTexts, FlyText>(EFlyTexts.Default, parent);
			flyText.SetText(text);
			flyText.SetWorldPosition(parent.position + localPos);
			flyText.SetScale(scale * Vector3.one);

			var sortingOrder = GetParentCanvasOrder(parent);
			flyText.SetSortingOrder(sortingOrder + SortingOrderOffset);

			if (isUnique)
			{
				UniqueFlyText = flyText;
				UniqueFlyText.StartFly(time)
					.Done(DisposeUnique);
			}
			else
			{
				flyText.StartFly(time)
					.Done(() => Dispose(flyText));

				ActiveFlyTexts.Add(flyText);
			}
		}



		private int GetParentCanvasOrder(Transform parent)
		{
			var canvas = parent.GetComponentInParent<Canvas>();
			return canvas != null ? canvas.sortingOrder : 0;
		}

		public void DisposeAllFlyTexts()
		{
			DisposeUnique();

			ActiveFlyTexts.ForEach(t => t.gameObject.SetActive(false));
			ActiveFlyTexts.Clear();
		}

		private void DisposeUnique()
		{
			if (UniqueFlyText != null)
			{
				UniqueFlyText.gameObject.SetActive(false);
				UniqueFlyText = null;
			}
		}

		private void Dispose(FlyText flyText)
		{
			flyText.gameObject.SetActive(false);
			ActiveFlyTexts.Remove(flyText);
		}
	}
}
