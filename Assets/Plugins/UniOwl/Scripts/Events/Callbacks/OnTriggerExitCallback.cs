using UnityEngine;
using UnityEngine.Events;

namespace UniOwl.Events
{
	public class OnTriggerExitCallback : MonoBehaviour
	{
		[SerializeField] private UnityEvent callback;

		private void OnTriggerExit(Collider other)
		{
			callback?.Invoke();
		}
	}
}
