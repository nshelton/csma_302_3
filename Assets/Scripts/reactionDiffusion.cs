
using System.Collections;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reactionDiffusion : MonoBehaviour
{
    [SerializeField] ComputeShader _shader;
    [SerializeField] ComputeShader _clearShader;
    [SerializeField] ComputeShader _drawShader;
    [SerializeField] Material _visualizationMaterial;

    RenderTexture _resultA;
    RenderTexture _resultB;

    [SerializeField] int _width = 512;
    [SerializeField] int _height = 512;

    [SerializeField, Range(0,0.13f)] float _feed = 0.018f;
    [SerializeField, Range(0,0.1f)] float _kill = 0.053f;
    [SerializeField] float _diffusionRateA = 0.1f;
    [SerializeField] float _diffusionRateB = 0.1f;
    [SerializeField, Range(0,20)] int _iterations = 10;

    int threadDim = 8;
    int _frameNum = 0;

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

        // run simulation
        _shader.SetFloat("_f", _feed);
        _shader.SetFloat("_k", _kill);
        _shader.SetFloat("_diffusionRateA", _diffusionRateA);
        _shader.SetFloat("_diffusionRateB", _diffusionRateB);
        _shader.SetFloat("_dt", Time.deltaTime);

        for(int i = 0; i < _iterations; i ++)
        {
            _shader.SetTexture(0, "_source", _resultA);
            _shader.SetTexture(0, "_destination", _resultB);
            _shader.Dispatch(0, _width / threadDim, _height / threadDim, 1);
            Swap(ref _resultB, ref _resultA);
        }

        Graphics.Blit(_resultA, destination, _visualizationMaterial);
    }
}
