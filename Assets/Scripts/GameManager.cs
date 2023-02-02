using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Threading.Tasks;

public class GameManager : PersistentSingleton<GameManager>
{
    public VisualElement Root { get; private set; }
    VisualElement _mainRightPanel;
    Label _mainLabel;
    Button _firstButton;
    Button _secondButton;
    VisualElement _mask;
    VisualElement _target;


    List<MyButton> _buttons = new();

    [field: SerializeField] public int Count { get; private set; }

    [SerializeField] Sound Music;

    AudioSource _musicAudioSource;
    public AudioSource EffectAudioSource { get; private set; }

    public GameDatabase GameDatabase;

    IEnumerator _coroutineTest;

    public event Action OnCorrectClick;
    public event Action OnWrongClick;
    protected override void Awake()
    {
        base.Awake();

        Root = GetComponent<UIDocument>().rootVisualElement;
        _mainRightPanel = Root.Q<VisualElement>("mainRightPanel");
        _mainLabel = Root.Q<Label>("mainLabel");
        _mainLabel.text = $"{Count}";

        _firstButton = Root.Q<Button>("firstButton");
        _firstButton.clickable.clicked += FirstButtonClick;

        _secondButton = Root.Q<Button>("secondButton");
        _secondButton.clickable.clicked += SecondButtonClick;

        _mask = Root.Q<VisualElement>("mask");
        _target = Root.Q<VisualElement>("target");

        Root.Q<Label>("test1Label").style.unityFontStyleAndWeight = FontStyle.Bold;

        _musicAudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        _musicAudioSource.loop = true;
        EffectAudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
    }

    // TEST 1
    void FirstButtonClick()
    {
        if (Count % 2 == 0)
        {
            Count++;
            _mainLabel.text = $"{Count}";
            _firstButton.text = "Don't click me";
            _secondButton.text = "Click me";
        }

        if (Count >= 5)
        {
            Root.Q<Label>("test1Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test2Label").style.unityFontStyleAndWeight = FontStyle.Bold;

            _mainRightPanel.Remove(_firstButton);
            _mainRightPanel.Remove(_secondButton);
            AddButtonTest2();
        }
    }

    void SecondButtonClick()
    {
        if (Count % 2 == 1)
        {
            Count++;
            _mainLabel.text = $"{Count}";
            _secondButton.text = "Don't click me";
            _firstButton.text = "Click me";
        }
    }



    // TEST 2
    void AddButtonTest2()
    {
        for (int i = 0; i < Random.Range(5, 10); i++)
            AddButton();
        ChangeButtonCallbacks();
    }

    MyButton AddButton()
    {
        MyButton button = new("Click me", null);
        _buttons.Add(button);
        _mainRightPanel.Add(button);
        return button;
    }



    // TEST 3
    void RemoveButtonsRandomly()
    {
        if (_buttons.Count < 5)
            return;
        for (int i = _buttons.Count - 1; i >= 0; i--)
        {
            if (Random.value < 0.1f)
            {
                _mainRightPanel.Remove(_buttons[i]);
                _buttons.Remove(_buttons[i]);
            }
        }
        ChangeButtonCallbacks();
    }


    void CorrectClick()
    {
        Count++;
        _mainLabel.text = $"{Count}";
        OnCorrectClick?.Invoke();

        if (Count == 10)
        {
            Root.Q<Label>("test2Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test3Label").style.unityFontStyleAndWeight = FontStyle.Bold;
        }
        if (Count == 15)
        {
            Root.Q<Label>("test3Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test4Label").style.unityFontStyleAndWeight = FontStyle.Bold;
            Music.Play(_musicAudioSource);
        }
        if (Count == 20)
        {
            Root.Q<Label>("test4Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test5Label").style.unityFontStyleAndWeight = FontStyle.Bold;
        }
        if (Count == 25)
        {
            Root.Q<Label>("test5Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test6Label").style.unityFontStyleAndWeight = FontStyle.Bold;
        }
        if (Count == 30)
        {
            Root.Q<Label>("test6Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test7Label").style.unityFontStyleAndWeight = FontStyle.Bold;
            Keep5Buttons();
        }
        if (Count == 35)
        {
            Root.Q<Label>("test7Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test8Label").style.unityFontStyleAndWeight = FontStyle.Bold;
            _coroutineTest = CoroutineTest();
            StartCoroutine(_coroutineTest);
        }
        if (Count == 40)
        {
            Root.Q<Label>("test8Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test9Label").style.unityFontStyleAndWeight = FontStyle.Bold;
        }
        if (Count == 45)
        {
            Root.Q<Label>("test9Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test10Label").style.unityFontStyleAndWeight = FontStyle.Bold;
            SaveTest();
        }
        if (Count == 50)
        {
            Root.Q<Label>("test10Label").style.backgroundColor = Color.green;
            Root.Q<Label>("test11Label").style.unityFontStyleAndWeight = FontStyle.Bold;
            AsyncAwaitTest();
        }


        if (Count < 10)
            AddButtonTest2();
        if (Count >= 10 && Count < 15)
            RemoveButtonsRandomly();

        if (Count > 15 && Count < 30)
        {
            if (Random.value < 0.5f)
                AddButtonTest2();
            else
                RemoveButtonsRandomly();
        }

        if (Count > 30)
            ChangeButtonCallbacks();

    }

    void WrongClick()
    {
        OnWrongClick?.Invoke();
        ChangeButtonCallbacks();
    }

    void Keep5Buttons()
    {
        if (_buttons.Count < 5)
        {
            for (int i = 0; i < 5 - _buttons.Count; i++)
            {
                AddButton();
            }
        }

        if (_buttons.Count > 5)
        {
            for (int i = _buttons.Count - 1; i >= 5; i--)
            {
                _mainRightPanel.Remove(_buttons[i]);
                _buttons.Remove(_buttons[i]);
            }
        }
        ChangeButtonCallbacks();
    }

    void ClearButtons()
    {
        DOTween.KillAll();
        for (int i = _buttons.Count - 1; i >= 0; i--)
        {
            _mainRightPanel.Remove(_buttons[i]);
            _buttons.Remove(_buttons[i]);
        }
    }

    void ChangeButtonCallbacks()
    {
        int correctIndex = Random.Range(0, _buttons.Count);
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (i == correctIndex)
            {
                _buttons[i].ChangeCallback(CorrectClick);
                _buttons[i].ChangeText("click me");
            }
            else
            {
                _buttons[i].ChangeCallback(WrongClick);
                _buttons[i].ChangeText("don't click me");
            }
        }
    }

    IEnumerator CoroutineTest()
    {
        ClearButtons();

        // new buttons
        for (int i = 0; i < 8; i++)
        {
            MyButton b = new();

            _mainRightPanel.Add(b);
            _buttons.Add(b);
            b.style.position = Position.Absolute;
            b.transform.position = _mask.layout.position;
        }

        Vector3 endPosition = _target.layout.position;
        for (int i = 0; i < _buttons.Count; i++)
        {
            StartCoroutine(MoveObjectOnArc(_buttons[i], _mask.layout.position, endPosition));
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;

    }

    IEnumerator MoveObjectOnArc(VisualElement el, Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 p0 = startPosition;
        float newX = startPosition.x + (endPosition.x - startPosition.x) * 0.5f;
        float newY = -Screen.height * 1.5f;

        Vector3 p1 = new Vector3(newX, newY);
        Vector3 p2 = endPosition;

        float percent = 0;
        while (percent < 1)
        {
            // https://www.reddit.com/r/Unity3D/comments/5pyi43/custom_dotween_easetypeeasefunction_based_on_four/
            Vector3 i1 = Vector3.Lerp(p0, p1, percent); // p1 is the shared handle
            Vector3 i2 = Vector3.Lerp(p1, p2, percent);
            Vector3 result = Vector3.Lerp(i1, i2, percent); // lerp between the 2 for the result
            el.transform.position = result;

            percent += 0.01f;
            yield return new WaitForSeconds(0.03f);
        }

        yield return MoveObjectBack(el);
    }

    IEnumerator MoveObjectBack(VisualElement el)
    {
        float percent = 0;
        while (percent < 1)
        {
            Vector3 result = Vector3.Lerp(el.transform.position, _mask.layout.position, percent); // p1 is the shared handle
            el.transform.position = result;

            percent += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        Vector3 endPosition = _target.layout.position;
        yield return MoveObjectOnArc(el, _mask.layout.position, endPosition);
    }

    void SaveTest()
    {
        // new save
        string guid = System.Guid.NewGuid().ToString();
        string fileName = guid + ".dat";
        Debug.Log($"Creating file, file name: {fileName}");
        FileManager.CreateFile(fileName);
        PlayerPrefs.SetString("saveName", fileName);
        PlayerPrefs.Save();

        SaveJsonData();
    }

    public void SaveJsonData()
    {
        SaveData sd = new SaveData();
        sd.Count = Count;
        if (FileManager.WriteToFile(PlayerPrefs.GetString("saveName"), sd.ToJson()))
            Debug.Log("Save successful");
    }


    async void AsyncAwaitTest()
    {
        Debug.Log($"It is supposed to take one from the Count every 0.5s.");
        while (Count > 0)
        {
            Count--;
            _mainLabel.text = Count.ToString();
            await Task.Delay(500);
        }
    }


}
