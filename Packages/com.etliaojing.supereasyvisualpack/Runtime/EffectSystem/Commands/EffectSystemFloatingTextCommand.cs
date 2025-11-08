using System.Collections.Generic;
using strange.extensions.command.impl;
using SuperEasy.EffectSystem.FloatingText.Views;
using UnityEngine;

namespace SuperEasy.EffectSystem.Commands
{
	public class EffectSystemFloatingTextCommandObject
	{
		public FloatingTextPanelView TargetPanel;
		public List<FloatingTextRequest> RequestList;
	}
		
	public class FloatingTextRequest
	{
		public Sprite Icon;
		public int Delta;
		public Vector3 Position;
		public bool IsLocalPosition;
	}
	
	public class EffectSystemFloatingTextCommand : EventCommand
	{
		public override void Execute()
		{
			var obj = evt.data as EffectSystemFloatingTextCommandObject;
			obj.TargetPanel.DisplayFloatingText(obj.RequestList);
		}
	}
}