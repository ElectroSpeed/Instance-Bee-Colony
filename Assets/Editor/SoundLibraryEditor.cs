using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

[CustomEditor(typeof(SoundLibrary))]
public class SoundLibraryEditor : Editor
{
    private const string _enumPath = "Assets/Script/Audio/SoundType.cs";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);

        if (GUILayout.Button("Mettre à jour l'Enum SoundType"))
            UpdateEnum();
    }

    private void UpdateEnum()
    {
        SoundLibrary library = (SoundLibrary)target;

        var soundNames = library._musicSounds
            .Concat(library._sfxSounds)
            .Where(sound => sound != null && !string.IsNullOrEmpty(sound._name))
            .Select(sound => SanitizeEnumName(sound._name))
            .Distinct()
            .OrderBy(n => n)
            .ToList();

        WriteEnum("SoundType", soundNames);
    }

    private void WriteEnum(string enumName, System.Collections.Generic.List<string> values)
    {
        string enumCode = $@"public enum {enumName}
{{
    None{(values.Count > 0 ? "," : "")}
    {string.Join(",\n    ", values)}
}}";

        File.WriteAllText(_enumPath, enumCode);
        AssetDatabase.Refresh();
    }

    private string SanitizeEnumName(string input)
    {
        string clean = new string(input
            .Where(c => char.IsLetterOrDigit(c) || c == '_')
            .ToArray());

        if (string.IsNullOrEmpty(clean))
            clean = "Unnamed";

        if (char.IsDigit(clean[0]))
            clean = "_" + clean;

        return clean;
    }
}