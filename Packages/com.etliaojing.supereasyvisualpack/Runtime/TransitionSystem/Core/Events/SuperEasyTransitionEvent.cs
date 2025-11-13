using System.Collections.Generic;

namespace SuperEasy.TransitionSystem.Core.Events
{
	public class SuperEasyTransitionEvent
	{
		public object TransitionViewKey;
		public List<string> ScenesToLoad;
		public List<string> ScenesToUnload;
		public bool AutoTransitionOut = true;
	}
}