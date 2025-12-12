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
				OperateScenesAsync(view, evt);
			};
			view.StartTransitionIn();
		}

		public static void TransitionOut(object key, bool isForce = false)
		{
			if (!KeyViewMapping.TryGetValue(key, out var view))
			{
				Debug.LogError($"Transition key {key} not found. Have you registered before calling?");
				return;
			}

			if (!isForce && !view.HasTransitionedIn)
			{
				Debug.LogError("Requested transition view is not transitioned in");
				return;
			}
			
			view.StartTransitionOut();
		}

		private static async void OperateScenesAsync(SuperEasyTransitionView view, SuperEasyTransitionEvent evt)
		{
			if (evt.ScenesToUnload is { Count: > 0 })
			{
				await UnloadScenes(evt.ScenesToUnload);
			}
			
			if (evt.ScenesToLoad is { Count: > 0 })
			{
				await LoadScenesAdditive(evt.ScenesToLoad);
			}

			if (evt.AutoTransitionOut)
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