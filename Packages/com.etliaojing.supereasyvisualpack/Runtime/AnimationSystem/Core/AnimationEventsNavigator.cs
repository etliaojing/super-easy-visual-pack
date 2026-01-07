using System;
using UnityEngine;
using UnityEngine.Events;

namespace SuperEasy.AnimationSystem.Core
{
	public delegate void AnimationEventCallback();
	
	public class AnimationEventsNavigator : MonoBehaviour
	{
		public UnityEvent OnShowStartEvent;
		public UnityEvent OnShowCompleteEvent;
		public UnityEvent OnHideStartEvent;
		public UnityEvent OnHideCompleteEvent;

		public void RegisterEventCallback(AnimationEventTypeEnum type, AnimationEventCallback callback)
		{
			switch (type)
			{
				case AnimationEventTypeEnum.OnShowStart:
					OnShowStartEvent.AddListener(() => callback());
					break;
				case AnimationEventTypeEnum.OnShowComplete:
					OnShowCompleteEvent.AddListener(() => callback());
					break;
				case AnimationEventTypeEnum.OnHideStart:
					OnHideStartEvent.AddListener(() => callback());
					break;
				case AnimationEventTypeEnum.OnHideComplete:
					OnHideCompleteEvent.AddListener(() => callback());
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public void OnShowStart()
		{
			OnShowStartEvent?.Invoke();
		}
		
		public void OnShowComplete()
		{
			OnShowCompleteEvent?.Invoke();
		}

		public void OnHideStart()
		{
			OnHideStartEvent?.Invoke();
		}

		public void OnHideComplete()
		{
			OnHideCompleteEvent?.Invoke();
		}
	}
}