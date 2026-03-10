using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{

    [SerializeField] Ability[] AbilityPrefabs;

    private List<Ability> abilities;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        abilities = new List<Ability>();
        foreach (Ability item in AbilityPrefabs)
        {
            Ability newItm = Instantiate(item);
            newItm.Initialize(gameObject);
            abilities.Add(newItm);
        }
    }

    // Update is called once per frame
    void Update()
    {
        abilities[0].Update();
    }
}
