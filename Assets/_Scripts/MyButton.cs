using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;

public class MyButton : Button
{
    GameManager _gameManager;

    Label _text;

    IVisualElementScheduledItem _successAnimation;
    VisualElement _animationContainer;
    List<Sprite> _animationSprites = new();
    int _spriteIndex;
    Action _currentCallback;
    public MyButton(string buttonText = null, Action callback = null)
    {
        var ss = GameManager.Instance.GetComponent<AddressableManager>().GetStyleSheetByName(StyleSheetType.ButtonStyles);
        if (ss != null)
            styleSheets.Add(ss);

        _gameManager = GameManager.Instance;
        _gameManager.OnCorrectClick += OnCorrectClick;
        _gameManager.OnWrongClick += OnWrongClick;

        _animationSprites = _gameManager.GameDatabase.GetAllAnimationSprites();

        _animationContainer = new();
        Add(_animationContainer);
        _animationContainer.style.position = Position.Absolute;
        _animationContainer.style.width = 100;
        _animationContainer.style.height = 100;
        _animationContainer.style.visibility = Visibility.Hidden;


        _text = new Label(buttonText);
        Add(_text);


        if (callback != null)
        {
            _currentCallback = callback;
            clicked += callback;
        }

        SetColor();
    }

    public void SetColor()
    {
        style.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        style.backgroundColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public void ChangeCallback(Action newCallback)
    {
        clickable = new Clickable(() => { });
        clicked += newCallback;
    }

    public void ClearCallbacks()
    {
        clickable = new Clickable(() => { });
    }


    void OnCorrectClick()
    {
        if (_gameManager.Count >= 20)
            _gameManager.GameDatabase.GetRandomCorrectSound().Play(_gameManager.EffectAudioSource);

        if (_gameManager.Count >= 25)
        {
            _animationContainer.style.visibility = Visibility.Visible;
            if (_successAnimation != null)
            {
                _spriteIndex = 0;
                _successAnimation.Resume();
                return;
            }
            _successAnimation = _animationContainer.schedule.Execute(SuccessAnimation).Every(50);
        }


        if (_gameManager.Count == 35)
        {
            style.position = Position.Absolute;
            style.left = Length.Percent(Random.Range(0, 100));
            style.top = Length.Percent(Random.Range(0, 100));

            int percentLeft = 0;
            if (Random.value > 0.5f)
                percentLeft = 90;
            int percentTop = 0;
            if (Random.value > 0.5f)
                percentTop = 90;

            if (Random.value > 0.5f)
                DOTween.To(() => style.left.value.value,
                        x => style.left = Length.Percent(x), percentLeft, Random.Range(1.5f, 3.5f))
                        .SetLoops(-1, LoopType.Yoyo);
            else
                DOTween.To(() => style.top.value.value,
                        x => style.top = Length.Percent(x), percentTop, Random.Range(1.5f, 3.5f))
                        .SetLoops(-1, LoopType.Yoyo);

            DOTween.To(x => transform.scale = x * Vector3.one, 1, 2, Random.Range(1.5f, 3.5f));
        }

        if (_gameManager.Count == 45)
        {
            RemoveFromClassList("unity-button");
            AddToClassList("button__main");
        }
    }

    void SuccessAnimation()
    {
        if (_spriteIndex == _animationSprites.Count)
        {
            _successAnimation.Pause();
            _animationContainer.style.visibility = Visibility.Hidden;
            return;
        }

        _animationContainer.style.backgroundImage = new StyleBackground(_animationSprites[_spriteIndex]);
        _spriteIndex++;
    }

    void OnWrongClick()
    {
        SetColor();
        if (_gameManager.Count > 20)
            _gameManager.GameDatabase._wrongSound.Play(_gameManager.EffectAudioSource);

    }

    public void ChangeText(string txt)
    {
        _text.text = txt;
    }



}
