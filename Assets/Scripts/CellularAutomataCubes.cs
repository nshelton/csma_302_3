using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomataCubes : MonoBehaviour
{
    [SerializeField] GameObject _primitive;
    [SerializeField] Material _primitiveMaterial;

    [SerializeField] ComputeShader _shader;
    [SerializeField] ComputeShader _clearShader;
    RenderTexture _result;

    [SerializeField] int _width = 128;
    [SerializeField] int _height = 128;

    [SerializeField, Range(0, 255)] int _rule;

    [SerializeField] bool _drawDebugTexture;

    int threadDim = 8;
    int _frameNum = 0;

    void Start()
    {
        _result = new RenderTexture(_width, _height, 0);
        _result.enableRandomWrite = true;
        _result.Create();

        var cubeParent = new GameObject("cubes");

        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var obj = GameObject.Instantiate(_primitive, cubeParent.transform);
                obj.transform.position = new Vector3(
                    x - _width/2, 
                    0, 
                    y - _height/2);

                obj.transform.localScale = Vector3.one * 0.9f;

                obj.GetComponent<MeshRenderer>().material = _primitiveMaterial;

                var block = new MaterialPropertyBlock();
                block.SetVector("_xy", new Vector2(
                    (float)(x + 0.5) / 128.0f, 
                    (float)(y + 0.5) / 128.0f));
                obj.GetComponent<MeshRenderer>().SetPropertyBlock(block);
            }
        }

        _primitiveMaterial.SetTexture("_cellularData", _result);
    }

    private void OnDestroy()
    {
        if ( _result != null) { _result.Release(); }
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
    }

    private void OnGUI()
    {
        if (_drawDebugTexture )
        {
            GUI.DrawTexture(new Rect(0, 0, _width * 3, _height * 3), _result);
        }
    }
}
