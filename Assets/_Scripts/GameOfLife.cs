using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameOfLife : MonoBehaviour
{
    [Range(1, 200)] public int FPS;
    private int _resolution = 1024;
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
    [HideInInspector] public bool _isPlaying = true;
    [SerializeField] RawImage _image;

    private RenderTexture[] _textures = new RenderTexture[3];
    private RenderTexture _colorTexture;
    private int _currentTexture = 0, _prevTexture;
    private int _loopKernel, _createGridKernel, _showKernel;

    [Range(0, 1)] public float _decayRate;
    public bool _randomRegeneration;

    private void Awake()
    {
        _createGridKernel = _shader.FindKernel("Create");
        _loopKernel = _shader.FindKernel("Update");
        _showKernel = _shader.FindKernel("Show");

    }

    void OnEnable()
    {        
        GOFCreate();        
    }

    void OnDestroy()
    {
        foreach (var t in _textures)
            if (t != null) t.Release();
        _colorTexture.Release();
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
        _colorTexture = new RenderTexture(Resolution, Resolution, 24) { enableRandomWrite = true};
        _colorTexture.Create();
        _shader.SetTexture(_createGridKernel, "Result", _textures[_currentTexture]);
        _shader.Dispatch(_createGridKernel, Resolution / 8, Resolution / 8, 1);

        _image.texture = _textures[_currentTexture];

        StartCoroutine(CSGOFUpdate());
    }

    IEnumerator CSGOFUpdate()
    {
        while (true)
        {
            if (_isPlaying)
            {
                GOFUpdate();
                GOFShow();
                _image.texture = _colorTexture;
            }
            yield return new WaitForSeconds(1f / FPS);
        }
    }

    private void GOFUpdate()
    {
        _prevTexture = _currentTexture;
        _currentTexture = (_currentTexture + 1) % 2;
        _shader.SetFloat("decayRate", _decayRate);
        _shader.SetBool("randomRegeneration", _randomRegeneration);
        _shader.SetTexture(_loopKernel, "Prev", _textures[_prevTexture]);
        _shader.SetTexture(_loopKernel, "Result", _textures[_currentTexture]);
        _shader.Dispatch(_loopKernel, Resolution / 8, Resolution / 8, 1);        
    }

    private void GOFShow()
    {
        _shader.SetTexture(_showKernel, "Prev", _textures[_prevTexture]);
        _shader.SetTexture(_showKernel, "Result", _textures[_currentTexture]);
        _shader.SetTexture(_showKernel, "ColorMap", _colorTexture);
        _shader.Dispatch(_showKernel, Resolution / 8, Resolution / 8, 1);

    }
}
