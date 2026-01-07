using System;
using UnityEngine;

namespace SuperEasy.TransitionSystem.Core.Views
{
	public abstract class SuperEasyTransitionView : MonoBehaviour
	{
		public event Action OnTransitionInComplete;
		public event Action OnTransitionOutComplete;
		internal void StartTransitionIn()
		{
			TransitionIn();
		}

		internal void StartTransitionOut()
		{
			TransitionOut();
		}
		protected abstract void TransitionIn();

		protected void CompleteTransitionIn()
		{
			OnTransitionInComplete?.Invoke();
			OnTransitionInComplete = null;
		}
		protected abstract void TransitionOut();

		protected void CompleteTransitionOut()
		{
			OnTransitionOutComplete?.Invoke();
			OnTransitionOutComplete = null;
		}
	}
}