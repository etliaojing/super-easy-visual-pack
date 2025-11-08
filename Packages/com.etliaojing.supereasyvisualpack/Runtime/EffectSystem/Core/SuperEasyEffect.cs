using System.Collections.Generic;
using EffectSystem.Core.Views;
using EffectSystem.FloatingText.Events;
using SuperEasy.EffectSystem.FloatingText.Views;
using UnityEngine;

namespace EffectSystem.Core
{
	public static class SuperEasyEffect
	{
		private static Dictionary<object, SuperEasyEffectPanelView> _panelDictionary;

		public static SuperEasyEffectPanelView RegisterEffectPanel(object key, SuperEasyEffectPanelView panel)
		{
			if (_panelDictionary.TryGetValue(key, out var value))
			{
				return value;
			}

			_panelDictionary.Add(key, panel);
			return panel;
		}

		public static bool UnregisterEffectPanel(object key)
		{
			return _panelDictionary.Remove(key);
		}

		public static void ShowFloatingText(object targetPanelKey, List<DisplayFloatingTextEvent> events)
		{
			if (!_panelDictionary.TryGetValue(targetPanelKey, out var panel))
			{
				Debug.LogWarning($"[{targetPanelKey}] does not exist, have you registered it?");
			}

			if (panel is FloatingTextPanelView floatingTextPanelView)
			{
				floatingTextPanelView.DisplayFloatingText(events);
			}
			else
			{
				Debug.LogWarning($"The target panel [{panel.name}] is not a Floating Text panel");
			}
		}
	}
}