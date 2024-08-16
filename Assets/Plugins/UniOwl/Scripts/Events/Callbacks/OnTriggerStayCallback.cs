using UnityEngine;
using UnityEngine.Events;

namespace UniOwl.Events
{
	public class OnTriggerStayCallback : MonoBehaviour
	{
		[SerializeField] private UnityEvent callback;

		private void OnTriggerStay(Collider other)
		{
			callback?.Invoke();
		}
	}
}
