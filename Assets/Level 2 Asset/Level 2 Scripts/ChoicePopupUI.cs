using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;

public class ChoicePopupUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonB;
    [SerializeField] private Text textA;
    [SerializeField] private Text textB;

    public void Show(string a, string b, System.Action onA, System.Action onB)
    {
        root.SetActive(true);

        textA.text = a;
        textB.text = b;

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();

        buttonA.onClick.AddListener(() => onA());
        buttonB.onClick.AddListener(() => onB());
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
