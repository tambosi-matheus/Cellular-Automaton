using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameOfLife))]
public class GameOfLifeEditor : Editor
{
    GameOfLife gof;

    // Texture
    string[] textureName = new string[] { "256", "512", "1024", "2048", "4096"};
    int[] textureSize = { 256, 512, 1024, 2048, 4096};

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        gof.Resolution = EditorGUILayout.IntPopup("Texture Size: ", gof.Resolution, textureName, textureSize);
        
        if (GUILayout.Button("Reset Simulation"))        
            gof.GOFCreate();
        

        if (gof.isPlaying)
        {
            if (GUILayout.Button("Pause simulation"))
                gof.isPlaying = !gof.isPlaying;
        }
        else
        {
            if (GUILayout.Button("Resume Simulation"))
                gof.isPlaying = !gof.isPlaying;
        }        
    }
    void OnEnable()
    {
        gof = (GameOfLife)target;
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }
}