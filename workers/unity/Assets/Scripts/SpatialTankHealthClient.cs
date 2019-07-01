using System.Collections;
using System.Collections.Generic;
using Improbable;
using Improbable.Worker;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Transform;
using Improbable.Gdk;
using System.Collections;
using System.Collections.Generic;
using BlankProject;
using Improbable.Gdk.Subscriptions;
using Tankspatial;
using UnityEngine;
using UnityEngine.UI;

[WorkerType(WorkerUtils.UnityClient)]
public class SpatialTankHealthClient : MonoBehaviour
{


    [Require] private TankHealthReader tankHealthReader;

    public GameObject _ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
    public Slider _Slider;                             // The slider to represent how much health the tank currently has.
    public Image _FillImage;                           // The image component of the slider.
    public Color _FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color _ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
    private float _CurrentHealth;                      // How much health the tank currently has.
    private float _StartingHealth;               // The amount of health each tank starts with.
    private bool _Dead;                                // Has the tank been reduced beyond zero health yet?


    private ParticleSystem _ExplosionParticles;        // The particle system the will play when the tank is destroyed.

    // Start is called before the first frame update
    private void Awake()
    {
        _ExplosionParticles = Instantiate(_ExplosionPrefab).GetComponent<ParticleSystem>();
        _ExplosionParticles.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        _StartingHealth = tankHealthReader.Data.MaxHealth;
        _CurrentHealth = tankHealthReader.Data.CurrentHealth;        
        _Dead = false;
        SetHealthUI();
    }

    // Update is called once per frame

    private void SetHealthUI()
     {
            _Slider.value = tankHealthReader.Data.CurrentHealth;
            _FillImage.color = Color.Lerp(_ZeroHealthColor, _FullHealthColor, _CurrentHealth / _StartingHealth);
     }
    
}
