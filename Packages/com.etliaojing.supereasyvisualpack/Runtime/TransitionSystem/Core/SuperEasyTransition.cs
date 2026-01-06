using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperEasy.TransitionSystem.Core.Events;
using SuperEasy.TransitionSystem.Core.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuperEasy.TransitionSystem.Core
{
	public static class SuperEasyTransition
	{
		private static readonly Dictionary<object, SuperEasyTransitionView> KeyViewMapping = new();

		public static SuperEasyTransitionView RegisterView(object key, SuperEasyTransitionView view)
		{
			if (KeyViewMapping.TryGetValue(key, out var existingView))
			{
				return existingView;
			}

			KeyViewMapping.Add(key, view);
			return view;
		}

		public static bool UnregisterView(object key)
		{
			return KeyViewMapping.Remove(key);
		}

		[Obsolete("Use TransitionIn instead")]
		public static void StartTransition(SuperEasyTransitionEvent evt)
		{
			if (!KeyViewMapping.TryGetValue(evt.TransitionViewKey, out var view))
			{
				Debug.LogError($"Transition key {evt.TransitionViewKey} not found. Have you registered before calling?");
				return;
			}

			view.OnTransitionInComplete += () =>
			{
				evt.OnTransitionInComplete?.Invoke();
				OperateScenesAsync(view, evt.ScenesToUnload, evt.ScenesToLoad, evt.AutoTransitionOut);
			};
			view.StartTransitionIn();
		}

		public static void TransitionIn(SuperEasyTransitionInEvent evt)
		{
			if (!KeyViewMapping.TryGetValue(evt.Key, out var view))
			{
				Debug.LogError($"Transition key [{evt.Key}] not found. Have you registered before calling?");
				return;
			}

			view.OnTransitionInComplete += evt.OnTransitionInComplete;
			view.OnTransitionInComplete += () =>
			{
				OperateScenesAsync(view, evt.ScenesToUnload, evt.ScenesToLoad);
			};
			view.StartTransitionIn();
		}

		[Obsolete("Use TransitionOut(evt) instead")]
		public static void TransitionOut(object key)
		{
			if (!KeyViewMapping.TryGetValue(key, out var view))
			{
				Debug.LogError($"Transition key {key} not found. Have you registered before calling?");
				return;
			}
			
			view.StartTransitionOut();
		}
		
		public static void TransitionOut(SuperEasyTransitionOutEvent evt)
		{
			if (!KeyViewMapping.TryGetValue(evt.Key, out var view))
			{
				Debug.LogError($"Transition key [{evt.Key}] not found. Have you registered before calling?");
				return;
			}

			view.OnTransitionOutComplete += evt.OnTransitionOutComplete;
			view.StartTransitionOut();
		}

		private static async void OperateScenesAsync(SuperEasyTransitionView view, List<string> scenesToUnload,
			List<string> scenesToLoad, bool autoTransitionOut = false)
		{
			if (scenesToUnload is { Count: > 0 })
			{
				await UnloadScenes(scenesToUnload);
			}

			if (scenesToLoad is { Count: > 0 })
			{
				await LoadScenesAdditive(scenesToLoad);
			}

			if (autoTransitionOut)
			{
				view.StartTransitionOut();
			}
		}

		private static async Task UnloadScenes(List<string> scenes)
		{
			var total = scenes.Count;
			foreach (var scene in scenes)
			{
				var asyncOperation = SceneManager.UnloadSceneAsync(scene);
				asyncOperation.completed += _ =>
				{
					total -= 1;
				};
			}

			while (total > 0)
			{
				await Task.Yield();
			}
		}

		private static async Task LoadScenesAdditive(List<string> scenes)
		{
			var total = scenes.Count;
			foreach (var scene in scenes)
			{
				var asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
				asyncOperation.completed += _ =>
				{
					total -= 1;
				};
			}
			
			while (total > 0)
			{
				await Task.Yield();
			}
		}
	}
}