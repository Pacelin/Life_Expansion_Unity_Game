using UnityEngine;
using UnityEngine.Events;

namespace UniOwl.Events
{
	public class OnCollisionStayCallback : MonoBehaviour
	{
		[SerializeField] private UnityEvent callback;

		private void OnCollisionStay()
		{
			callback?.Invoke();
		}
	}
}
