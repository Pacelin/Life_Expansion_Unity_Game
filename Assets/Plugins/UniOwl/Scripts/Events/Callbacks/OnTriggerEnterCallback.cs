using UnityEngine;
using UnityEngine.Events;

namespace UniOwl.Events
{
	public class OnTriggerEnterCallback : MonoBehaviour
	{
		[SerializeField] private UnityEvent callback;

		private void OnTriggerEnter(Collider other)
		{
			callback?.Invoke();
		}
	}
}
