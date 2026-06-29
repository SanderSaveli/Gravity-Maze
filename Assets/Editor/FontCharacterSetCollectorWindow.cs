using Newtonsoft.Json;
using SanderSaveli.GravityMaze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class FontCharacterSetCollectorWindow : EditorWindow
{
    private const string DEFAULT_TEXTS_PATH = "Assets/Resources/Texts.json";
    private const string OUTPUT_FOLDER = "Assets/Source/Fonts/Character Sets";
    private const string COMMON_CHARACTERS = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.,:;!?()[]{}+-*/=%\u00A9&<>_'\"\n\r";

    private string _textsPath = DEFAULT_TEXTS_PATH;
    private Vector2 _scrollPosition;
    private Dictionary<string, string> _characterSets = new Dictionary<string, string>();

    [MenuItem("Tools/Fonts/Collect Character Sets")]
    public static void ShowWindow()
    {
        GetWindow<FontCharacterSetCollectorWindow>("Font Characters");
    }

    private void OnEnable()
    {
        Collect();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("TMP Character Set Collector", EditorStyles.boldLabel);
        _textsPath = EditorGUILayout.TextField("Texts JSON", _textsPath);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Collect"))
        {
            Collect();
        }

        if (GUILayout.Button("Save Txt Files"))
        {
            SaveCharacterSetFiles();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        foreach (KeyValuePair<string, string> pair in _characterSets)
        {
            DrawCharacterSet(pair.Key, pair.Value);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawCharacterSet(string language, string characters)
    {
        EditorGUILayout.LabelField($"{language}: {characters.Length} characters", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button($"Copy {language}", GUILayout.Width(120f)))
        {
            EditorGUIUtility.systemCopyBuffer = characters;
            Debug.Log($"Copied {language} character set. Characters: {characters.Length}");
        }

        EditorGUILayout.TextArea(characters, GUILayout.MinHeight(60f));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    private void Collect()
    {
        if (!File.Exists(_textsPath))
        {
            Debug.LogError($"Texts file not found: {_textsPath}");
            _characterSets.Clear();
            return;
        }

        string rawText = File.ReadAllText(_textsPath, Encoding.UTF8);
        string json = DecodeTextStorage(rawText);
        List<LanguageStringData> rows = JsonConvert.DeserializeObject<List<LanguageStringData>>(json);

        if (rows == null || rows.Count == 0)
        {
            Debug.LogError($"Texts file has no rows: {_textsPath}");
            _characterSets.Clear();
            return;
        }

        _characterSets = new Dictionary<string, string>
        {
            { "latin_cyrillic", BuildCharacterSet(rows, GetLatinCyrillicLanguageValues) },
            { "ja", BuildCharacterSet(rows, item => item.ja) },
            { "ko", BuildCharacterSet(rows, item => item.ko) },
            { "all", BuildCharacterSet(rows, GetAllLanguageValues) }
        };

        Debug.Log($"Collected font character sets from {_textsPath}.");
    }

    private void SaveCharacterSetFiles()
    {
        if (_characterSets.Count == 0)
        {
            Debug.LogError("No character sets collected.");
            return;
        }

        EnsureOutputFolder();

        foreach (KeyValuePair<string, string> pair in _characterSets)
        {
            string path = $"{OUTPUT_FOLDER}/{pair.Key}_characters.txt";
            File.WriteAllText(path, pair.Value, Encoding.UTF8);
        }

        AssetDatabase.Refresh();
        Debug.Log($"Saved font character set files to {OUTPUT_FOLDER}.");
    }

    private string DecodeTextStorage(string rawText)
    {
        string trimmedText = rawText.Trim();

        if (!trimmedText.StartsWith("\"", StringComparison.Ordinal))
            return trimmedText;

        return JsonConvert.DeserializeObject<string>(trimmedText);
    }

    private string BuildCharacterSet(List<LanguageStringData> rows, Func<LanguageStringData, string> getText)
    {
        SortedSet<char> characters = new SortedSet<char>();
        AddCharacters(characters, COMMON_CHARACTERS);

        foreach (LanguageStringData row in rows)
        {
            AddCharacters(characters, getText(row));
        }

        StringBuilder stringBuilder = new StringBuilder(characters.Count);

        foreach (char character in characters)
        {
            stringBuilder.Append(character);
        }

        return stringBuilder.ToString();
    }

    private string GetAllLanguageValues(LanguageStringData row)
    {
        return string.Concat(
            row.en,
            row.ru,
            row.de,
            row.es,
            row.fr,
            row.it,
            row.ja,
            row.ko,
            row.pt);
    }

    private string GetLatinCyrillicLanguageValues(LanguageStringData row)
    {
        return string.Concat(
            row.en,
            row.ru,
            row.de,
            row.es,
            row.fr,
            row.it,
            row.pt);
    }

    private void AddCharacters(SortedSet<char> characters, string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        foreach (char character in text)
        {
            characters.Add(character);
        }
    }

    private void EnsureOutputFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Source/Fonts"))
        {
            AssetDatabase.CreateFolder("Assets/Source", "Fonts");
        }

        if (!AssetDatabase.IsValidFolder(OUTPUT_FOLDER))
        {
            AssetDatabase.CreateFolder("Assets/Source/Fonts", "Character Sets");
        }
    }
}
