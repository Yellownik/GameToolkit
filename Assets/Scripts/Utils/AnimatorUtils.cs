using Core;
using Orbox.Async;
using UnityEngine;

namespace Utils
{
	public static class AnimatorUtils
	{
		public static IPromise WaitForAnimationEnd(Animator animator, int stateHash)
		{
			var promise = new Deferred();
			Root.Timers.WaitForCondition(() => animator == null || animator.GetCurrentAnimatorStateInfo(0).shortNameHash == stateHash)
				.Done(() =>
				{
					if (animator == null)
						return;

					float waitTime = animator.GetCurrentAnimatorStateInfo(0).length;
					Root.Timers.Wait(waitTime)
						.Done(() => promise.Resolve());
				});
			return promise;
		}

		public static float GetAnimationLength(Animator animator, string name)
		{
			AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
			float waitTime = 0;
			foreach (var animationClip in animationClips)
			{
				if (animationClip.name == name)
				{
					waitTime = animationClip.length;
					break;
				}
			}

			return waitTime;
		}
	}
}
