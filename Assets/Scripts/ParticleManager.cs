using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] slideParticles;

    PlayerMovement player;

    void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }

    void OnEnable()
    {
        player.OnSlide += PlaySlide;
    }

    void OnDisable()
    {
        player.OnSlide -= PlaySlide;
    }

    private void PlaySlide()
    {
        foreach (ParticleSystem particle in slideParticles)
        {
            particle.Play();
        }
    }
}
