using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    [SerializeField] ComputeShader _shader;
    [SerializeField] ComputeShader _clearShader;
    RenderTexture _result;

    [SerializeField] int _width = 512;
    [SerializeField] int _height = 512;

    [SerializeField, Range(0, 255)] int _rule;

    int threadDim = 8;

    int _frameNum = 0;

    void Start()
    {
        _result = new RenderTexture(_width, _height, 0);
        _result.enableRandomWrite = true;
        _result.Create();
    }

    private void OnDestroy()
    {
        if (_result != null) { _result.Release(); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _frameNum = 0;
            _clearShader.SetTexture(0, "_Result", _result);
            _clearShader.Dispatch(0, _width / threadDim, _height / threadDim, 1);
            _rule = (int)(Random.value * 255);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_frameNum < 512)
        {
            _shader.SetInt("_FrameNum", _frameNum);
            _shader.SetInt("_Width", _width);
            _shader.SetInt("_Height", _height);

            _shader.SetInt("_Rule", _rule);

            _shader.SetTexture(0, "_Result", _result);
            _shader.Dispatch(0, _width / threadDim, 1, 1);
            _frameNum++;
        }

        Graphics.Blit(_result, destination);
    }
}