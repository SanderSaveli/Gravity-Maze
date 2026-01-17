using UnityEditor;
using UnityEngine;
using System.IO;

public static class LevelPreviewGenerator
{
    private const int PREVIEW_SIZE = 256;
    private const float PADDING = 1.2f; // Отступ (20%)

    [MenuItem("Assets/Generate Level Preview", true)]
    static bool ValidateGeneratePreview()
    {
        return Selection.activeObject is GameObject;
    }

    [MenuItem("Assets/Generate Level Preview")]
    static void GeneratePreview()
    {
        GameObject prefab = Selection.activeObject as GameObject;
        if (prefab == null)
            return;

        string prefabPath = AssetDatabase.GetAssetPath(prefab);
        if (!prefabPath.EndsWith(".prefab"))
            return;

        // Инстанс префаба
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        instance.transform.position = Vector3.zero;

        Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("В префабе нет Renderer");
            Object.DestroyImmediate(instance);
            return;
        }

        // Вычисляем Bounds
        Bounds bounds = CalculateBounds(renderers);

        // Камера
        Camera cam = new GameObject("PreviewCamera").AddComponent<Camera>();
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.Color;
        cam.backgroundColor = Color.clear;
        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 100f;

        cam.transform.position = new Vector3(
            bounds.center.x,
            bounds.center.y,
            -10f
        );

        float sizeX = bounds.extents.x;
        float sizeY = bounds.extents.y;
        cam.orthographicSize = Mathf.Max(sizeX, sizeY) * PADDING;

        // RenderTexture
        RenderTexture rt = new RenderTexture(PREVIEW_SIZE, PREVIEW_SIZE, 16);
        cam.targetTexture = rt;

        RenderTexture.active = rt;
        cam.Render();

        Texture2D tex = new Texture2D(PREVIEW_SIZE, PREVIEW_SIZE, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, PREVIEW_SIZE, PREVIEW_SIZE), 0, 0);
        tex.Apply();

        RenderTexture.active = null;

        // Папка для превью
        const string previewFolder = "Assets/LevelPreviews";
        if (!AssetDatabase.IsValidFolder(previewFolder))
        {
            AssetDatabase.CreateFolder("Assets", "LevelPreviews");
        }

        string previewName = $"Preview_{prefab.name}.png";
        string previewPath = $"{previewFolder}/{previewName}";

        File.WriteAllBytes(previewPath, tex.EncodeToPNG());
        AssetDatabase.ImportAsset(previewPath);

        // Назначаем иконку
        Texture2D previewTex = AssetDatabase.LoadAssetAtPath<Texture2D>(previewPath);
        EditorGUIUtility.SetIconForObject(prefab, previewTex);

        // Очистка
        Object.DestroyImmediate(cam.gameObject);
        Object.DestroyImmediate(instance);
        Object.DestroyImmediate(rt);

        Debug.Log($"Preview создан: {previewPath}");
    }

    private static Bounds CalculateBounds(Renderer[] renderers)
    {
        Bounds bounds = renderers[0].bounds;
        foreach (var r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }
}
