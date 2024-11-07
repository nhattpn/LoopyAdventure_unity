using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    // Thay đổi từ single SpriteRenderer sang array
    [SerializeField] private SpriteRenderer[] childSprites;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        // Tự động tìm tất cả SpriteRenderer trong các object con
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        
        // Kiểm tra xem có tìm thấy sprite renderer không
        if (childSprites.Length == 0)
        {
            Debug.LogWarning("No SpriteRenderers found in children of " + gameObject.name);
        }
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("grounded", true);
                anim.SetTrigger("die");
                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        if (childSprites.Length == 0) yield break;
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            // Áp dụng hiệu ứng flash cho tất cả sprite renderer
            foreach (var sprite in childSprites)
            {
                if (sprite != null)
                    sprite.color = new Color(1, 0, 0, 0.5f);
            }
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

            foreach (var sprite in childSprites)
            {
                if (sprite != null)
                    sprite.color = Color.white;
            }
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public void Respawn()
    {
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());
        dead = false;
        foreach (Behaviour component in components)
            component.enabled = true;
    }
}