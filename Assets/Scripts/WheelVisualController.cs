using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class WheelVisualController : MonoBehaviour
{
    [Header("Visual Effects")] [SerializeField]
    private ParticleSystem spinParticles;

    [SerializeField] private ParticleSystem winParticles;
    [SerializeField] private Transform glowEffect;

    [Header("Animation Settings")] [SerializeField]
    private float glowPulseSpeed = 1f;

    [SerializeField] private float glowMinScale = 0.9f;
    [SerializeField] private float glowMaxScale = 1.1f;

    private void Start()
    {
        if (glowEffect != null)
        {
            AnimateGlow();
        }
    }

    private void OnEnable()
    {
        GameEvents.OnWheelSpinComplete += OnSpinComplete;
    }

    private void OnDisable()
    {
        GameEvents.OnWheelSpinComplete -= OnSpinComplete;
    }

    public void PlaySpinEffect()
    {
        if (spinParticles != null)
        {
            spinParticles.Play();
        }
    }

    public void StopSpinEffect()
    {
        if (spinParticles != null)
        {
            spinParticles.Stop();
        }
    }

    private void OnSpinComplete(int sliceIndex)
    {
        StopSpinEffect();

        if (winParticles != null)
        {
            winParticles.Play();
        }
    }

    private void AnimateGlow()
    {
        transform.DOScale(glowMaxScale, glowPulseSpeed)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}