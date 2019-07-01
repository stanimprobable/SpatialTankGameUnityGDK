using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{

    public float _StartingHealth = 100f ;               // The amount of health each tank starts with.
    public Slider _Slider;                             // The slider to represent how much health the tank currently has.
    public Image _FillImage;                           // The image component of the slider.
    public Color _FullHealthColor = Color.green ;       // The color the health bar will be when on full health.
    public Color _ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
    public GameObject _ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.


    private ParticleSystem _ExplosionParticles;        // The particle system the will play when the tank is destroyed.
    private float _CurrentHealth;                      // How much health the tank currently has.
    private bool _Dead;                                // Has the tank been reduced beyond zero health yet?

    // Start is called before the first frame update

    private void Awake()
    {
     
        _ExplosionParticles = Instantiate(_ExplosionPrefab).GetComponent<ParticleSystem>();
        _ExplosionParticles.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        _CurrentHealth = _StartingHealth;
        _Dead = false;
        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        _CurrentHealth -= amount;
        SetHealthUI();
        if (_CurrentHealth < 0 && !_Dead)
        {
            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        _Slider.value = _CurrentHealth;
        _FillImage.color = Color.Lerp(_ZeroHealthColor, _FullHealthColor, _CurrentHealth / _StartingHealth);
    }

    private void OnDeath()
    {
        _Dead = true;
        _ExplosionParticles.transform.position = transform.position;
        _ExplosionParticles.gameObject.SetActive(true);

        _ExplosionParticles.Play();

        gameObject.SetActive(false);


    }

}
