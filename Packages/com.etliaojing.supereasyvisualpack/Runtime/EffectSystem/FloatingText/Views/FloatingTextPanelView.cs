using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using SuperEasy.EffectSystem.Commands;
using UnityEngine;
using UnityEngine.Pool;

namespace SuperEasy.EffectSystem.FloatingText.Views
{
	public class FloatingTextPanelView : View
	{
		[SerializeField] private RectTransform _vfxContainer;
		[SerializeField] private FloatingBaseView _floatingTextTemplate;
		
		private IObjectPool<FloatingBaseView> _floatingTextPool;
		
		public void DisplayFloatingText(List<FloatingTextRequest> requests)
		{
			StartCoroutine(CoDisplay(requests));
		}

		private IEnumerator CoDisplay(List<FloatingTextRequest> requests)
		{
			foreach (var request in requests)
			{
				NewFloatingText(request.Delta, request.Icon, request.WorldPosition);
				yield return new WaitForSeconds(0.3f);
			}
		}

		private void NewFloatingText(int delta, Sprite icon, Vector3 worldPos)
		{
			var floatingText = _floatingTextPool.Get();

			switch (floatingText)
			{
				case FloatingIconTextView iconTextView:
					iconTextView.SetUp(icon, delta);
					break;
				case FloatingTextView _textView:
					_textView.SetUp(delta);
					break;
			}
			
			floatingText.transform.position = worldPos;
			floatingText.Pop(() => { _floatingTextPool.Release(floatingText); });
		}
		
		protected override void Awake()
		{
			base.Awake();
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