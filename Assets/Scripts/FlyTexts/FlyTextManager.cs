using Orbox.Utils;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace FlyTexts
{
	public class FlyTextManager
	{
		private const int SortingOrderOffset = 5;
		private readonly IResourceManager ResourceManager;
		private readonly CameraManager CameraManager;

		private FlyText UniqueFlyText;
		private List<FlyText> ActiveFlyTexts = new List<FlyText>();

		public FlyTextManager(IResourceManager resourceManager, CameraManager cameraManager)
		{
			ResourceManager = resourceManager;
			CameraManager = cameraManager;
		}

		public void Spawn(RectTransform parent, string text, Vector3 localPos = default, float scale = 1, float time = 6, bool isUnique = true)
		{
			if (isUnique && UniqueFlyText != null)
				UniqueFlyText.Interrupt();

			var flyText = CreateAndInit(parent, text, localPos, scale);

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

		private FlyText CreateAndInit(RectTransform parent, string text, Vector2 localPos = default, float scale = 1)
		{
			var flyText = ResourceManager.GetFromPool<EFlyTexts, FlyText>(EFlyTexts.DefaultFlyText, parent);
			flyText.SetText(text);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, CameraManager.Camera, out Vector2 pos);
			flyText.GetComponent<RectTransform>().anchoredPosition = pos + localPos + new Vector2 (Screen.width, Screen.height) * 0.5f;
			//flyText.SetWorldPosition(parent.position + localPos);
			flyText.SetScale(scale * Vector3.one);

			var sortingOrder = GetParentCanvasOrder(parent);
			flyText.SetSortingOrder(sortingOrder + SortingOrderOffset);

			return flyText;
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
