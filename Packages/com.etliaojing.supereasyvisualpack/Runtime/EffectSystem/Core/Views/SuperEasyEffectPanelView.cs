using System.Collections;
using System.Collections.Generic;
using EffectSystem.Core.Events;
using UnityEngine;
using UnityEngine.Pool;

namespace EffectSystem.Core.Views
{
	public abstract class SuperEasyEffectPanelView : MonoBehaviour
	{
		public int DefaultPoolSize = 5;
		public int MaxPoolSize = 5;
		
		[SerializeField] protected RectTransform _vfxContainer;
		[SerializeField] protected SuperEasyEffectView _template;
		
		protected IObjectPool<SuperEasyEffectView> _effectPool;
		
		public void Display(List<SuperEasyEffectEvent> events)
		{
			StartCoroutine(CoDisplay(events));
		}

		private IEnumerator CoDisplay(List<SuperEasyEffectEvent> events)
		{
			foreach (var e in events)
			{
				NewFloatingText(e);
				yield return new WaitForSeconds(0.3f);
			}
		}

		private void NewFloatingText(SuperEasyEffectEvent e)
		{
			var effect = _effectPool.Get();
			effect.transform.position = e.Position;
			effect.Show(() => { _effectPool.Release(effect); });
		}
		
		private void Awake()
		{
			Initialise();
		}

		private void Initialise()
		{
			_effectPool = new ObjectPool<SuperEasyEffectView>(
				OnCreateEffect,
				OnTakeEffect,
				OnReturnEffect,
				OnDestroyEffect,
				true, DefaultPoolSize, MaxPoolSize);
		}

		private void OnDestroyEffect(SuperEasyEffectView obj)
		{
			Destroy(obj.gameObject);
		}

		private void OnReturnEffect(SuperEasyEffectView obj)
		{
			obj.CleanUp();
			obj.gameObject.SetActive(false);
		}

		private void OnTakeEffect(SuperEasyEffectView obj)
		{
			obj.transform.SetAsLastSibling();
			obj.gameObject.SetActive(true);
		}

		private SuperEasyEffectView OnCreateEffect()
		{
			var instance = Instantiate(_template, _vfxContainer);
			return instance;
		}
	}
}