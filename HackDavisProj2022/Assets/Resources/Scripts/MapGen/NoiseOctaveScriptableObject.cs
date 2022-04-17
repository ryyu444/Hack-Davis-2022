using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="NoiseOctaveScriptableObject")]
public class NoiseOctaveScriptableObject : ScriptableObject
{
    public List<NoiseOctave> noiseOctaves;
}

[System.Serializable]
public class NoiseOctave
{
    public float amplitude;
    public float frequency;
    public Vector2 offset;
    public float exponent = 1;
}
