using System;
using System.Collections.Generic;

namespace SuperEasy.TransitionSystem.Core.Events
{
	public class SuperEasyTransitionInEvent
	{
		public object Key;
		public List<string> ScenesToLoad;
		public List<string> ScenesToUnload;
		public Action OnTransitionInComplete;
	}
}