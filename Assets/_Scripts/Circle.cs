using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Circle : MonoBehaviour
{
    GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnCorrectClick += OnCorrectClick;
    }

    void OnCorrectClick()
    {
        Vector3 endPos = new Vector3(7, -4, 0);

        if (_gameManager.Count == 30)
        {
            transform.DOMove(endPos, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutExpo);
            transform.DOScale(Vector3.one, 1.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
