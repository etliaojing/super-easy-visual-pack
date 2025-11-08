using System;
using System.Collections;
using SuperEasy.AnimationSystem.Constants;
using UnityEngine;

namespace SuperEasy.EffectSystem.FloatingText.Views
{
	public class FloatingBaseView : MonoBehaviour
	{
		[SerializeField] private Animator _animator;
		
		public void Pop(Action onComplete)
		{
			_animator.SetTrigger(AnimationTriggerConstants.Pop);
			StartCoroutine(CoWaitForPopComplete(onComplete));
		}

		private static IEnumerator CoWaitForPopComplete(Action onComplete)
		{
			yield return new WaitForSeconds(1f);
			onComplete?.Invoke();
		}

		public virtual void CleanUp()
		{
		}

		private void OnDestroy()
		{
			CleanUp();
		}
	}
}