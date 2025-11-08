using System.Collections;
using System.Collections.Generic;
using EffectSystem.Core.Views;
using EffectSystem.FloatingText.Events;
using UnityEngine;
using UnityEngine.Pool;

namespace SuperEasy.EffectSystem.FloatingText.Views
{
	public class FloatingTextPanelView : SuperEasyEffectPanelView
	{
		[SerializeField] private RectTransform _vfxContainer;
		[SerializeField] private FloatingBaseView _floatingTextTemplate;
		
		private IObjectPool<FloatingBaseView> _floatingTextPool;
		
		public void DisplayFloatingText(List<DisplayFloatingTextEvent> events)
		{
			StartCoroutine(CoDisplay(events));
		}

		private IEnumerator CoDisplay(List<DisplayFloatingTextEvent> events)
		{
			foreach (var e in events)
			{
				NewFloatingText(e);
				yield return new WaitForSeconds(0.3f);
			}
		}

		private void NewFloatingText(DisplayFloatingTextEvent e)
		{
			var floatingText = _floatingTextPool.Get();

			switch (floatingText)
			{
				case FloatingIconTextView iconTextView:
					iconTextView.SetUp(e.Icon, e.Delta);
					break;
				case FloatingTextView _textView:
					_textView.SetUp(e.Delta);
					break;
			}

			if (e.IsLocalPosition)
			{
				floatingText.transform.localPosition = e.Position;
			}
			else
			{
				floatingText.transform.position = e.Position;
			}
			floatingText.Pop(() => { _floatingTextPool.Release(floatingText); });
		}
		
		private void Awake()
		{
			Initialise();
		}

		private void Initialise()
		{
			_floatingTextPool = new ObjectPool<FloatingBaseView>(
				OnCreateFloatingTextPool,
				OnTakeFloatingTextPool,
				OnReturnFloatingTextPool,
				OnDestroyFloatingTextPool,
				true, 5, 10);
		}

		private void OnDestroyFloatingTextPool(FloatingBaseView obj)
		{
			Destroy(obj.gameObject);
		}

		private void OnReturnFloatingTextPool(FloatingBaseView obj)
		{
			obj.CleanUp();
			obj.gameObject.SetActive(false);
		}

		private void OnTakeFloatingTextPool(FloatingBaseView obj)
		{
			obj.transform.SetAsLastSibling();
			obj.gameObject.SetActive(true);
		}

		private FloatingBaseView OnCreateFloatingTextPool()
		{
			var instance = Instantiate(_floatingTextTemplate, _vfxContainer);
			return instance;
		}
	}
}