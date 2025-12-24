using SanderSaveli.GravityMaze;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeCreatorWindow : EditorWindow
{
    private SpriteShapeController _targetController;
    private ShapeCreator _shapeCreator = new PolygonShapeCreator();
    private bool _isAutoFill;

    [MenuItem("Tools/SpriteShape Polygon Creator")]
    public static void ShowWindow()
    {
        GetWindow<SpriteShapeCreatorWindow>("Polygon Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert SpriteShape to Polygon", EditorStyles.boldLabel);

        _targetController = Selection.activeGameObject?.GetComponent<SpriteShapeController>();

        if (_targetController == null)
        {
            EditorGUILayout.HelpBox("Select a GameObject with a SpriteShapeController.", MessageType.Warning);
            return;
        }
        _shapeCreator.Draw();
        GUILayout.Space(100);
        _isAutoFill = GUILayout.Toggle(_isAutoFill, "Auto Create", EditorStyles.miniButton);
        if (_isAutoFill)
        {
            _shapeCreator.Create(_targetController);
        }

        if (GUILayout.Button("Create"))
        {
            CreatePolygon();
        }
    }

    private void CreatePolygon()
    {
        _shapeCreator.Create(_targetController);
    }
}
