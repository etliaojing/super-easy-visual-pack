using System;
using System.Collections;
using UnityEngine;

namespace EffectSystem.Core.Views
{
	public abstract class SuperEasyEffectView : MonoBehaviour
	{
		public float DestroyInSeconds = 3f;

		public virtual void Show(Action onComplete = null)
		{
			StartCoroutine(CoShow(onComplete));
		}

		private IEnumerator CoShow(Action onComplete)
		{
			yield return new WaitForSeconds(DestroyInSeconds);
			onComplete?.Invoke();
		}
		
		public virtual void CleanUp()
		{
		}
	}
}