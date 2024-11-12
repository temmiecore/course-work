using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Light light;
    private PlayerController player;

    private float lightningRoll;
    public float lightningChance = 0.01f;
    private float lightIntensity = 0;
    public float lightFadeout = 0.01f;

    public int maxParticles = 1000;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        light = GetComponentInChildren<Light>();
        player = GameManager.instance.playerController;
    }

    private void Update()
    {
        lightningRoll = Random.Range(0f, 1f);
        if (lightningRoll < lightningChance)
            lightIntensity = 10f;

        if (lightIntensity > 0)
            lightIntensity -= lightFadeout;

        light.intensity = lightIntensity;

        if (player != null)
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }
}
