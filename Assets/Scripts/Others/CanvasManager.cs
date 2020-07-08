using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Canvas canvas;
    public Image heart1;
    public Image heart2;
    public Image heart3;


    private void Awake()
    {
        canvas = this.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        heart1 = transform.DeepFind("heart1").GetComponent<Image>();
        heart2 = transform.DeepFind("heart2").GetComponent<Image>();
        heart3 = transform.DeepFind("heart3").GetComponent<Image>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
