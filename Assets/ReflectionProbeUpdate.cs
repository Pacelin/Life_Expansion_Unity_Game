using DG.Tweening;
using UnityEngine;

public class ReflectionProbeUpdate : MonoBehaviour
{
    [SerializeField]
    private ReflectionProbe _probe;
    [SerializeField]
    private float _updateInterval = 1f;
    
    private Tween _tween;
    
    private void OnEnable()
    {
        if (!_probe)
            return;

        _tween = DOTween
                 .Sequence()
                 .AppendInterval(_updateInterval)
                 .AppendCallback(() => _probe.RenderProbe())
                 .SetLoops(-1);
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }
}
