using System.Collections;
using System.Collections.Generic;
using Effects;
using Effects.Status;
using UnityEngine;

public class PlayerEffectVisual : MonoBehaviour
{
    public EffectObject effect;
    public GameObject FirePar;
    public GameObject Binded;
    public GameObject Shieled;
    public GameObject Slow;

    void Start()
    {
        effect = GetComponent<EffectObject>();
    }

    void Update()
    {
        foreach (var item in effect.statusEffects)
        {
            Binded.SetActive(item.TryGetComponent<Bounded>(out Bounded bounded));
            FirePar.SetActive(item.TryGetComponent<DOT>(out DOT dot));
            Shieled.SetActive(item.TryGetComponent<Shielded>(out Shielded shielded));
            Slow.SetActive(item.TryGetComponent<Slowed>(out Slowed slowed));
        }
    }
}
