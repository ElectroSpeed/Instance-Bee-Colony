using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

[CustomEditor(typeof(PowerLibrary))]
public class PowerLibraryEditor : Editor
{
    private const string _enumPath = "Assets/Script/Power/PowerType.cs";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);

        if (GUILayout.Button("Mettre à jour l'Enum PowerType"))
            UpdateEnum();
    }

    private void UpdateEnum()
    {
        PowerLibrary library = (PowerLibrary)target;

        var powerNames = library._powers
            .Where(power => power != null && !string.IsNullOrEmpty(power._name))
            .Select(power => SanitizeEnumName(power._name))
            .Distinct()
            .OrderBy(n => n)
            .ToList();

        WriteEnum("PowerType", powerNames);
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