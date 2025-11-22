using System;
using UnityEngine;

namespace SuperEasy.TransitionSystem.Core.Views
{
	public abstract class SuperEasyTransitionView : MonoBehaviour
	{
		internal bool HasTransitionedIn;
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
			HasTransitionedIn = true;
			OnTransitionInComplete?.Invoke();
		}
		protected abstract void TransitionOut();

		protected void CompleteTransitionOut()
		{
			HasTransitionedIn = false;
			OnTransitionOutComplete?.Invoke();
		}
	}
}