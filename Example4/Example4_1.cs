using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// View part изменил подписку на нажатие кнопки, чтобы не создавать анонимные методы каждый раз
// Учитывая что имеет смысл использовать пул объектов, то выключение и включение в такой реализации не будет накапливать подпоски на кнопки
public class CheatElementBehaviour : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Button _button;

    private CheatActionDescription _description;
    
    // Устанавливаем описание чита и действие, которое будет выполняться при нажатии на кнопку
    public void Setup(CheatActionDescription description)
    {
        _text.text = description.name;
        _description = description;
    }
    
    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }
    
    private void OnButtonClick()
    {
        _description?.cheatAction();
    }
}

// Data part
public class CheatActionDescription
{
    public readonly string name;
    public readonly Action cheatAction;

    public CheatActionDescription(string name, Action cheatAction)
    {
        this.name = name;
        this.cheatAction = cheatAction;
    }
}

public interface ICheatProvider
{
    IEnumerable<CheatActionDescription> GetCheatActions();
}

// Singleton класс для управления отображением панели читов
public class CheatManager
{
    // Можно использовать отложенную инициализацию, чтобы не создавать экземпляр CheatManager, если он не понадобится
    // public static CheatManager Instance => _instance ??= new CheatManager();
    public static readonly CheatManager Instance = new CheatManager();

    private readonly List<ICheatProvider> _providers = new List<ICheatProvider>();

    private GameObject _panelPrefab;
    private CheatElementBehaviour _cheatElementPrefab;

    private GameObject _panel;

    // Думаю что можно было бы не передавать панель и элементы читов в Setup, а например загружать их из ресурсов, например при первом обращении.
    public void Setup(GameObject panelPrefab, CheatElementBehaviour cheatElementPrefab)
    {
        _panelPrefab = panelPrefab;
        _cheatElementPrefab = cheatElementPrefab;
    }

    // Не хватает по моему метода UnregisterProvider для удаления провайдера, думаю что такое может быть например при переходе между сценами
    // Хорошей практикой было бы проверить что мы не добавляем один и тот же провайдер несколько раз
    public void RegProvider(ICheatProvider provider)
    {
        _providers.Add(provider);
    }

    // Можно было использовать асинхронную загрузку панели, чтобы не тормозить игру при открытии панели
    // Также можно было бы использовать пул объектов для панели и элементов читов, чтобы не создавать их каждый раз заново
    // Достаточно просто скрыть и показать панель, чтобы не создавать ее каждый раз заново, а объекты читов создавать по мере необходимости.
    public void ShowCheatPanel()
    {
        if (_panel != null)
            return;

        _panel = UnityEngine.Object.Instantiate(_panelPrefab);
        foreach (var provider in _providers)
        {
            foreach (var cheatAction in provider.GetCheatActions())
            {
                var element = UnityEngine.Object.Instantiate(_cheatElementPrefab, _panel.transform);

                element.Setup(cheatAction);
            }
        }
    }

    // Тут вместо удаления все панелей хорошо было бы использовать пул объектов, перед выключением панели данные можно было бы сбросить в исходное состояние
    public void HideCheatPanel()
    {
        if (_panel == null)
            return;

        UnityEngine.Object.Destroy(_panel);
        _panel = null;
    }
}

// Эта реализация мне нравится больше, поскольку не обязывает нас использовать CheatManager как компонент GO.
public class SomeManagerWithCheats : CheatManager.ICheatProvider
{
    private int _health;

    public void Setup()
    {
        CheatManager.Instance.RegProvider(this);
    }

    //Тут мне нравиться что объекты читов создаются по мере необходимости, а не все сразу
    IEnumerable<CheatManager.CheatActionDescription> CheatManager.ICheatProvider.GetCheatActions()
    {
        yield return new CheatManager.CheatActionDescription("Cheat health", () => _health++);
        yield return new CheatManager.CheatActionDescription("Reset health", () => _health = 0);
    }
}
