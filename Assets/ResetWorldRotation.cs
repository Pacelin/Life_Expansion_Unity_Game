using Unity.Mathematics;
using UnityEngine;

public class ResetWorldRotation : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = quaternion.identity;
    }
}
