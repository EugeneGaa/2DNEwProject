using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get;  set; }
    public int heroHp = 3;

    [Header("----- Game Related Components -----")]
    public CanvasManager mainCanvasManager;

    [Header("----- Others -----")]
    public Sprite heartsEmpty;
    public Sprite heartsFull;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        mainCanvasManager = GameObject.Find("MainCanvas").GetComponent<CanvasManager>();
    }

    private void Update()
    {
        ChangeHpUi();
    }

    public void ChangeHpUi()
    {
        switch(heroHp)
        {
            case 1:
                mainCanvasManager.heart1.sprite = heartsFull;
                mainCanvasManager.heart2.sprite = heartsEmpty;
                mainCanvasManager.heart3.sprite = heartsEmpty;
                break;
            case 2:
                mainCanvasManager.heart1.sprite = heartsFull;
                mainCanvasManager.heart2.sprite = heartsFull;
                mainCanvasManager.heart3.sprite = heartsEmpty;
                break;
            case 3:
                mainCanvasManager.heart1.sprite = heartsFull;
                mainCanvasManager.heart2.sprite = heartsFull;
                mainCanvasManager.heart3.sprite = heartsFull;
                break;
            case 0:
                mainCanvasManager.heart1.sprite = heartsEmpty;
                mainCanvasManager.heart2.sprite = heartsEmpty;
                mainCanvasManager.heart3.sprite = heartsEmpty;
                break;
        }
    }
}
