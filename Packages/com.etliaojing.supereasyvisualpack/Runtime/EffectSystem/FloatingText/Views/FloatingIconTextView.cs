using UnityEngine;
using UnityEngine.UI;

namespace SuperEasy.EffectSystem.FloatingText.Views
{
	public class FloatingIconTextView : FloatingTextView
	{
		[SerializeField] private Image _icon;

		public void SetUp(Sprite sprite, int delta)
		{
			base.SetUp(delta);
			
			_icon.sprite = sprite;
		}

		public override void CleanUp()
		{
			base.CleanUp();
			_icon.sprite = null;
		}
	}
}