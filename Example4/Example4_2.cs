using System;
using UnityEngine;
using UnityEngine.UI;

// View part
public class CommonCheat : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Button _button;

    public void Setup(string name, Action cheatAction)
    {
        _text.text = name;
        _button.onClick.AddListener(() => cheatAction());
    }
}

// Singleton класс для управления отображением панели читов
public class CheatManager
{
    public static readonly CheatManager Instance = new CheatManager();

    private GameObject _panel;

    public GameObject Panel => _panel;
    
    public void Setup(GameObject panel)
    {
        _panel = panel;
        _panel.SetActive(false);
    }

    public void ShowCheatPanel()
    {
        _panel.SetActive(true);
    }

    public void HideCheatPanel()
    {
        _panel.SetActive(false);
    }
}

// Эта реаллизация мне нравится меньше, поскольку обязует нас исползовать текущий менедже как компонент GO.
public class SomeManagerWithCheats : MonoBehaviour
{
    [SerializeField] private CheatManager.CommonCheat _cheatPrefab;

    private int _health;

    public void Setup()
    {
        // Если порядок инициализации будет нарушен и CheatManager.Instance.Panel будет null, то это вызовет NullReferenceException
        var cheat1 = Instantiate(_cheatPrefab, CheatManager.Instance.Panel.transform);
        cheat1.Setup("Cheat health", () => _health++);
        var cheat2 = Instantiate(_cheatPrefab, CheatManager.Instance.Panel.transform);
        cheat2.Setup("Reset health", () => _health = 0);
    }
}
