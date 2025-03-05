using System;
using UnityEngine;

public class EntityStats : MonoBehaviour {

    public Stat strength;
    public Stat damage;
    public Stat maxHealth;

    //VISIBLE ONLY DURING TEST PHASE
    [SerializeField] private int currentHealth;

    protected virtual void Start() {
        currentHealth = maxHealth.GetValue();
    }

    void Update() {
    }

    public virtual void DoDamage(EntityStats _targetStats){
        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage){
        currentHealth -= _damage;

        Debug.Log(_damage);

        if(currentHealth <= 0){
            Die();
        }
    }

    protected virtual void Die(){
    }
}
