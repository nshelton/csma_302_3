<<<<<<< HEAD
using System.Collections;
=======
﻿using System.Collections;
>>>>>>> 6eef7458095dd557ad023465aca658d47e466692
using System.Collections.Generic;
using UnityEngine;

public class conway : MonoBehaviour
{
    [SerializeField] ComputeShader _shader;
    [SerializeField] ComputeShader _clearShader;
    [SerializeField] ComputeShader _drawShader;
    [SerializeField] Material _visualizationMaterial;

    RenderTexture _resultA;
    RenderTexture _resultB;

    [SerializeField] int _width = 512;
    [SerializeField] int _height = 512;


    int threadDim = 8;

    Vector3 _mousePos;

    void Start()
    {
        _resultA = new RenderTexture(_width, _height, 0);
        _resultA.enableRandomWrite = true;
        _resultA.Create();

        _resultB = new RenderTexture(_width, _height, 0);
        _resultB.enableRandomWrite = true;
        _resultB.Create();
    }

    private void OnDestroy()
    {
        if ( _resultA != null) { _resultA.Release(); }
        if ( _resultB != null) { _resultB.Release(); }
    }

    private void Update()
    {
        _mousePos = Input.mousePosition;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _clearShader.SetTexture(0, "_Result", _resultA);
            _clearShader.Dispatch(0, _width / threadDim, _height / threadDim, 1);
        }
    }

    static void Swap(ref RenderTexture x, ref RenderTexture y)
    {
        RenderTexture tempswap = x;
        x = y;
        y = tempswap;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // user input
        _drawShader.SetVector("_mouse", _mousePos);
        _drawShader.SetTexture(0, "_destination", _resultA);
        _drawShader.Dispatch(0, 1, 1, 1);

        _shader.SetTexture(0, "_source", _resultA);
        _shader.SetTexture(0, "_destination", _resultB);
        _shader.SetFloat("_width", _width);
        _shader.SetFloat("_height", _height);
        _shader.Dispatch(0, _width / threadDim, _height / threadDim, 1);
        Swap(ref _resultB, ref _resultA);

        Graphics.Blit(_resultA, destination, _visualizationMaterial);
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 6eef7458095dd557ad023465aca658d47e466692
