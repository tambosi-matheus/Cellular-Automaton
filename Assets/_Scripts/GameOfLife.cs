using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOfLife : MonoBehaviour
{
    [Range(1, 200)] public int FPS;
    private int _resolution = 1024;
    public bool showMoving;
    [HideInInspector] public int Resolution
    {
        get => _resolution;
        set
        {
            if (_resolution == value) return;
            _resolution = value;
            GOFCreate();
        }
    }


    public ComputeShader _shader;
    [HideInInspector] public bool isPlaying = true;
    [SerializeField] RawImage _image;

    private RenderTexture[] _textures = new RenderTexture[3];
    private int _currentTexture = 0, _prevTexture;
    private int _loopKernel, _createGridKernel;

    private void Awake()
    {
        _createGridKernel = _shader.FindKernel("Create");
        _loopKernel = _shader.FindKernel("Update");
        
    }

    void OnEnable()
    {        
        GOFCreate();        
    }

    void OnDestroy()
    {
        foreach (var t in _textures)
            if (t != null) t.Release();        
        //Array.ForEach(_textures, t => t?.Release());
        _textures = null;
        StopAllCoroutines();
    }


    public void GOFCreate()
    {
        StopAllCoroutines();
        for (int i = 0; i < _textures.Length; i++)
        {
            _textures[i]?.Release();
            _textures[i] = new RenderTexture(Resolution, Resolution, 24);
            _textures[i].enableRandomWrite = true;
            _textures[i].Create();
        }
        _shader.SetTexture(_createGridKernel, "Result", _textures[_currentTexture]);
        _shader.Dispatch(_createGridKernel, Resolution / 8, Resolution / 8, 1);

        _image.texture = _textures[_currentTexture];

        StartCoroutine(CSGOFUpdate());
    }

    IEnumerator CSGOFUpdate()
    {
        while (true)
        {
            if (isPlaying)
            {
                GOFUpdate();
                _image.texture = _textures[_currentTexture];
            }
            yield return new WaitForSeconds(1f / FPS);
        }
    }

    private void GOFUpdate()
    {
        _prevTexture = _currentTexture;
        _currentTexture = (_currentTexture + 1) % 2;
        _shader.SetTexture(_loopKernel, "Prev", _textures[_prevTexture]);
        _shader.SetTexture(_loopKernel, "Result", _textures[_currentTexture]);
        _shader.Dispatch(_loopKernel, Resolution / 8, Resolution / 8, 1);        
    }
}
