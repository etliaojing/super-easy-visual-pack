using TMPro;
using UnityEngine;

namespace SuperEasy.EffectSystem.FloatingText.Views
{
	public class FloatingTextView : FloatingBaseView
	{
		[SerializeField] private TextMeshProUGUI _delta;

		public void SetUp(Sprite sprite, int delta)
		{
			if (delta == 0)
			{
				Debug.LogWarning("Skip showing vfx because delta is 0");
				return;
			}
			
			_icon.sprite = sprite;
			_delta.text = delta > 0 ? $"+{delta:N0}" : $"{delta:N0}";
		}
	}
}