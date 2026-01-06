using System;

namespace SuperEasy.TransitionSystem.Core.Events
{
	public class SuperEasyTransitionOutEvent
	{
		public object Key;
		public Action OnTransitionOutComplete;
	}
}