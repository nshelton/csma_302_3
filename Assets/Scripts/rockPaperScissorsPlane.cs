using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockPaperScissorsPlane : MonoBehaviour
{
    [SerializeField] Material _planeMaterial;

    [SerializeField] ComputeShader _shader;
    [SerializeField] ComputeShader _clearShader;
    [SerializeField] ComputeShader _drawShader;

    RenderTexture _resultA;
    RenderTexture _resultB;

    [SerializeField] int _width = 512;
    [SerializeField] int _height = 512;

    int threadDim = 8;

    Vector3 _mousePos;

    Vector4 _currentColor;
    bool _mouseDown;
         
    RaycastHit hit;


    void Start()
    {
        _resultA = new RenderTexture(_width, _height, 0);
        _resultA.enableRandomWrite = true;
        _resultA.Create();

        _resultB = new RenderTexture(_width, _height, 0);
        _resultB.enableRandomWrite = true;
        _resultB.Create();

        _currentColor = new Vector4(1, 0, 0, 1);
    }

    private void OnDestroy()
    {
        if (_resultA != null) { _resultA.Release(); }
        if (_resultB != null) { _resultB.Release(); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentColor = new Vector4(1, 0, 0, 1);    // red
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentColor = new Vector4(0, 1, 0, 1);    // green
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentColor = new Vector4(0, 0, 1, 1);    // blue
        }

        _mouseDown = Input.GetMouseButton(0);

        // you need to press space before drawing to see RPS in 3D
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _clearShader.SetTexture(0, "_Result", _resultA);
            _clearShader.Dispatch(0, _width / threadDim, _height / threadDim, 1);
        }
        
        // from in-class reactionDiffusionPlane.cs
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
   
        if (Physics.Raycast(ray, out hit))
        {
            var worldPoint = hit.point;

            worldPoint = transform.InverseTransformPoint(worldPoint);
            float x = -worldPoint.x / 10.0f + 0.5f;
            float z = -worldPoint.z / 10.0f + 0.5f;

            var pixelCoord = new Vector2(_width * x, _height * z);

            _mousePos = pixelCoord;
        }

        // user input
        if (_mouseDown)
        {
            _drawShader.SetVector("_mouse", _mousePos);
            _drawShader.SetVector("_Color", _currentColor);
            _drawShader.SetTexture(0, "_destination", _resultA);
            _drawShader.Dispatch(0, 1, 1, 1);
        }

        _shader.SetTexture(0, "_source", _resultA);
        _shader.SetTexture(0, "_destination", _resultB);
        _shader.SetFloat("_width", _width);
        _shader.SetFloat("_height", _height);
        _shader.Dispatch(0, _width / threadDim, _height / threadDim, 1);
        Swap(ref _resultB, ref _resultA);

        _planeMaterial.SetTexture("_DispTex", _resultA);
        _planeMaterial.SetTexture("_MainTex", _resultA);
    }

    static void Swap(ref RenderTexture x, ref RenderTexture y)
    {
        RenderTexture tempswap = x;
        x = y;
        y = tempswap;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(_resultA, destination);
    }

    private void OnGUI()
    {
        var size = 200;
        GUI.DrawTexture(new Rect(0, 0, size, size), _resultA);
        GUI.DrawTexture(new Rect(size, 0, size, size), _resultB);
    }
}