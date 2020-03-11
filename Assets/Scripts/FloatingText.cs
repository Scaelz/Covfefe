using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] Color color;
    public Color currentColor;
    public Color decayColor;
    [SerializeField] float speed;
    [SerializeField] float decaySpeed;
    [SerializeField] Camera cam;
    [SerializeField] TextMeshPro textMesh;
    Vector3 defaultPosition;
    IdleConfig config;
    bool isActive = false;


    private void Start()
    {
        config = FindObjectOfType<IdleConfig>();
        defaultPosition = transform.position;
        SetupDecayColor();
        Reset();
    }

    public void StartFloat()
    {
        Reset();
        isActive = true;
    }


    void SetupDecayColor()
    {
        decayColor = color;
        decayColor.a = 0;
    }

    private void Update()
    {
        if (isActive)
        {
            MoveUp();
            ColorDecay();
            textMesh.color = currentColor;
            if (currentColor.a == 0)
            {
                Reset();
            }
        }
    }

    private void LateUpdate()
    {
        LookToCamera();
    }

    private void Reset()
    {
        isActive = false;
        double rounded = Math.Floor(config.coinsClickValue);
        textMesh.text = $"+ {rounded.ToString()} $";
        textMesh.text = Exponent.SetExponentText(rounded);
        transform.position = defaultPosition;
        currentColor = color;
        textMesh.color = decayColor;
    }

    private void ColorDecay()
    {
        currentColor = Color.Lerp(currentColor, decayColor, decaySpeed);
    }

    private void MoveUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * speed, transform.position.z);
    }

    void LookToCamera()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
