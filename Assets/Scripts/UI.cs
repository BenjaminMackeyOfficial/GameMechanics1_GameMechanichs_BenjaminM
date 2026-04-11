using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UI : MonoBehaviour
{
    public static UI Instance;
    
    private TMP_Text uiText;
    void Start()
    {
        Instance = this;
        uiText = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
        Debug.Log(uiText);
    }
    public void ChangeText(string text)
    {
        if (uiText == null) return;
        uiText.text = text;
    }
   
}
