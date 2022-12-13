using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Stigmergy : MonoBehaviour
{
    public ComputeShader StigmergyCS;
    int diffuseKernel, moveKernel, colorMapKernel;
    public RawImage image;
    RenderTexture trailTexture, agentsTexture, mainTexture;
    private List<Cell> cells = new List<Cell>();
    ComputeBuffer cellsBuffer;

    [Range(256, 2048)] public int scale = 1024;
    int _resolution = 1024;
    int currResolution;    
    [Range(100, 1000000)] public int CellCount = 1000;
    [Range(0, 1f)] public float decayRate;
    [Range(0, 1f)] public float diffuseRate;
    [Range(1, 3)] public int diffuseRadius;
    [Range(1, 200)] public int FPS;
    public bool hasAtractor;

    public Color TrailColor, AgentColor, BackgroundColor;
    private Color _trailColor, _agentColor, _backgroundColor;
    private float[] _trailFloat, _agentFloat;

    struct Cell
    {
        public float2 position;
        public float4 color;
        public float angle;
        public int mainColor;
    }

    private void OnEnable()
    {
        
    }

    void Start()
    {
        // Get kernels ID's
        colorMapKernel = StigmergyCS.FindKernel("Show");
        moveKernel = StigmergyCS.FindKernel("Move");
        diffuseKernel = StigmergyCS.FindKernel("Diffuse");

        // Initialize textures
        trailTexture = new RenderTexture(_resolution, _resolution, 24) { enableRandomWrite = true };
        trailTexture.Create();
        agentsTexture = new RenderTexture(_resolution, _resolution, 24) {enableRandomWrite = true};
        agentsTexture.Create();
        mainTexture = new RenderTexture(_resolution, _resolution, 24) { enableRandomWrite = true };
        mainTexture.Create();

        // Initiate Cells
        for (int i = 0; i < CellCount; i++)
            cells.Add(CreateCell());

        // Initialize Buffer
        cellsBuffer = new ComputeBuffer(5000000, 8 * 4);
        cellsBuffer.SetData(cells);

        StartCoroutine(CSUpdate());
    }

    private Cell CreateCell()
    {
        //var pos = new float2(Random.Range(0, Resolution), Random.Range(0, Resolution));
        var pos = new float2(_resolution / 2, _resolution / 2);
        var c = Color.HSVToRGB(Random.Range(0, 1f), 1, 1);
        var color = new float4(c.r, c.g, c.b, 1f);
        int mainColor;
        if (color.x > color.y && color.x > color.z)
            mainColor = 0;
        else if (color.y > color.z)
            mainColor = 1;
        else
            mainColor = 2;
        return new Cell() { position = pos, color = color, angle = Random.Range(0, 360), mainColor = mainColor };
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
    }

    void UpdateInputs()
    {
        _resolution = scale + (8 - scale % 8);
        if (_resolution != currResolution)
        {
            currResolution = _resolution;
            trailTexture = new RenderTexture(_resolution, _resolution, 24) { enableRandomWrite = true };
            trailTexture.Create();
            agentsTexture = new RenderTexture(_resolution, _resolution, 24) {enableRandomWrite = true};            
            agentsTexture.Create();
            mainTexture = new RenderTexture(_resolution, _resolution, 24) { enableRandomWrite = true};
            mainTexture.Create();
        }
        if (AgentColor != _agentColor)
        {
            _agentColor = AgentColor;
            _agentFloat = new float[] { _agentColor.r, _agentColor.g, _agentColor.b };            
        }
        if (TrailColor != _trailColor)
        {
            _trailColor = TrailColor;
            _trailFloat = new float[] { _trailColor.r, _trailColor.g, _trailColor.b};
        }
        if(BackgroundColor != _backgroundColor)
        {
            _backgroundColor = BackgroundColor;
            Camera.main.backgroundColor = _backgroundColor;
        }
    }

    IEnumerator CSUpdate()
    {
        while (true)
        {
            Move();
            Diffuse();
            Show();
            image.texture = mainTexture;
            yield return new WaitForSeconds(1f/FPS);
        }
    }

    void Move()
    {
        // Clear the agents texture before next step
        StigmergyCS.SetTexture(3, "agentsMap", agentsTexture);
        StigmergyCS.Dispatch(3, _resolution / 8, _resolution / 8, 1);

        StigmergyCS.SetInt("width", _resolution);
        StigmergyCS.SetInt("height", _resolution);
        StigmergyCS.SetFloat("speed", 1);
        StigmergyCS.SetFloat("time", Time.time);
        StigmergyCS.SetInt("cellCount", CellCount);
        StigmergyCS.SetBool("hasAtraction", hasAtractor);
        StigmergyCS.SetBuffer(moveKernel, "cells", cellsBuffer);
        StigmergyCS.SetFloats("trailColor", _trailFloat);
        StigmergyCS.SetTexture(moveKernel, "trailMap", trailTexture);
        StigmergyCS.SetTexture(moveKernel, "agentsMap", agentsTexture);
        StigmergyCS.Dispatch(moveKernel, (int)(CellCount / 64), 1, 1);
        
    }

    void Diffuse()
    {       
        StigmergyCS.SetFloat("decayRate", decayRate);
        StigmergyCS.SetInt("diffuseRadius", diffuseRadius);
        StigmergyCS.SetFloat("diffuseRate", diffuseRate);
        StigmergyCS.SetTexture(diffuseKernel, "trailMap", trailTexture);

        StigmergyCS.Dispatch(diffuseKernel, _resolution / 8, _resolution / 8, 1);        
    }

    void Show()
    {
        StigmergyCS.SetFloats("trailColor", _trailFloat);
        StigmergyCS.SetFloats("agentColor", _agentFloat);
        StigmergyCS.SetTexture(colorMapKernel, "trailMap", trailTexture);
        StigmergyCS.SetTexture(colorMapKernel, "agentsMap", agentsTexture);
        StigmergyCS.SetTexture(colorMapKernel, "mainMap", mainTexture);
        StigmergyCS.Dispatch(colorMapKernel, _resolution / 8, _resolution / 8, 1);
    }

    private void OnDestroy()
    {
        if(trailTexture != null) trailTexture.Release();
        if (agentsTexture != null) agentsTexture.Release();
        if (cellsBuffer != null) cellsBuffer.Release();
        if (mainTexture != null) mainTexture.Release();
        cellsBuffer = null;
        StopAllCoroutines();
    }
}
