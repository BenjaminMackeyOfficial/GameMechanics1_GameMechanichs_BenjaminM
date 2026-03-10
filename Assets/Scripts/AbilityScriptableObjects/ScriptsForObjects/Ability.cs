using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LoadUIStrategy", menuName = "Scriptable Objects/LoadUIStrategy")]
public abstract class Ability : ScriptableObject
{
    public abstract void Initialize(GameObject parent);
    public abstract void Update();
    public abstract void Excecute();
}
