using System;
using System.Collections;
using strange.extensions.mediation.impl;
using SuperEasy.AnimationSystem.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace SuperEasy.EffectSystem.FloatingText.Views
{
	public class FloatingBaseView : View
	{
		[SerializeField] private Animator _animator;
		[SerializeField] protected Image _icon;
		
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

		public void CleanUp()
		{
			_icon.sprite = null;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			CleanUp();
		}
	}
}