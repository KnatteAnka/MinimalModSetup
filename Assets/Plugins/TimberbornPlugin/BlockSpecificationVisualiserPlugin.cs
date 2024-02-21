#if UNITY_EDITOR
using System;
using System.Reflection;
using Timberborn.BlockSystem;
using UnityEditor;
using UnityEngine;

namespace EditorPlugins
{
  [CustomEditor(typeof(BlockObject), true)]
  public class DrawBlockObjectOccupation : Editor
  {
    private static BlockObject _blockObject;
    private static PlaceableBlockObject _placeableBlockObject;
    private static bool _placeableBlockObjectExists;

    private object _sceneOverlayWindow;
    private MethodInfo _showSceneViewOverlay;

    private static bool _pluginEnabled;
    private static bool _sizeFoldout;
    private static bool _baseZFoldout;
    private static bool _occupationFoldout;
    private static bool _customPivotFoldout;
    
    private static bool _showSize;
    private static Color _sizeColour = new Color(0.5254902f, 0.909804f, 0.9607844f, 1f);
    private static float _sizeFillMultiplier = 0f;
    
    private static bool _showBaseZ = true;
    private static Color _baseZColour = new Color(0.5254902f, 0.651867f, 0.9607843f, 1f);
    private static float _baseZFillMultiplier = 0.5f;
    
    private static bool _showOccupation = true;
    private static Color _occupationColour = new Color(1f, 0.8352942f, 0.1647059f, 1f);
    private static float _occupationFillMultiplier = 0.5f;
    
    private static bool _showCustomPivot = true;
    private static Color _customPivotColour = new Color(1f, 0.4587275f, 0.164705f, 1f);
    private static float _customPivotFillMultiplier = 0.5f;

    private Material _solidColorMaterial;
    private Mesh _cubeMesh;
 
    void OnEnable()
    {
      var unityEditor = Assembly.GetAssembly(typeof(UnityEditor.SceneView));
      var overlayWindowType = unityEditor.GetType("UnityEditor.OverlayWindow");
      var sceneViewOverlayType = unityEditor.GetType("UnityEditor.SceneViewOverlay");
      var windowFuncType = sceneViewOverlayType.GetNestedType("WindowFunction");
      var windowFunc = Delegate.CreateDelegate(windowFuncType, this.GetType().GetMethod(nameof(DoOverlayUI), BindingFlags.Static | BindingFlags.NonPublic));
      var windowDisplayOptionType = sceneViewOverlayType.GetNestedType("WindowDisplayOption");
      _sceneOverlayWindow = Activator.CreateInstance(
        overlayWindowType, 
        EditorGUIUtility.TrTextContent("Block Object Visualiser","Block Object Visualiser", (Texture)null),
        windowFunc,
        int.MaxValue,
        (UnityEngine.Object) target,
        Enum.Parse(windowDisplayOptionType, "OneWindowPerTarget")
      );
      _showSceneViewOverlay = sceneViewOverlayType.GetMethod("ShowWindow", BindingFlags.Static | BindingFlags.Public);
      
      _blockObject = (BlockObject)target;
      _placeableBlockObject = _blockObject.TransformFast.gameObject.GetComponent<PlaceableBlockObject>();
      _placeableBlockObjectExists = _placeableBlockObject != null;
    }
    
    private static void DoOverlayUI(UnityEngine.Object target, SceneView sceneView)
    {
      GUILayout.Label("<color=grey>v0.5.2-alpha</color>", new GUIStyle
      {
        alignment = TextAnchor.UpperRight,
        fontStyle = FontStyle.Italic,
        fontSize = 10,
        richText = true
      });

      _pluginEnabled = GUILayout.Toggle(_pluginEnabled, "Enable Plugin");
      if (!_pluginEnabled)
        return;
      
      _sizeFoldout = EditorGUILayout.Foldout(_sizeFoldout, "Size");
      if (_sizeFoldout)
      {
        _showSize = GUILayout.Toggle(_showSize, "Show Size");
        _sizeColour = EditorGUILayout.ColorField("Indicator Colour", _sizeColour);
        _sizeFillMultiplier = EditorGUILayout.Slider("Indicator Opacity", _sizeFillMultiplier,  0,  1);
        EditorGUILayout.Separator();
      }
      
      _baseZFoldout = EditorGUILayout.Foldout(_baseZFoldout, "Base Z");
      if (_baseZFoldout)
      {
        _showBaseZ = GUILayout.Toggle(_showBaseZ, "Show Base Z");
        _baseZColour = EditorGUILayout.ColorField("Indicator Colour", _baseZColour);
        _baseZFillMultiplier = EditorGUILayout.Slider("Indicator Opacity", _baseZFillMultiplier,  0,  1);
        EditorGUILayout.Separator();
      }
      
      _occupationFoldout = EditorGUILayout.Foldout(_occupationFoldout, "Occupation");
      if (_occupationFoldout)
      {
        _showOccupation = GUILayout.Toggle(_showOccupation, "Show Occupation");
        _occupationColour = EditorGUILayout.ColorField("Indicator Colour", _occupationColour);
        _occupationFillMultiplier = EditorGUILayout.Slider("Indicator Opacity", _occupationFillMultiplier,  0,  1);
        EditorGUILayout.Separator();
      }

      if (!_placeableBlockObjectExists)
        return;
      
      _customPivotFoldout = EditorGUILayout.Foldout(_customPivotFoldout, "Custom Pivot");
      if (_customPivotFoldout)
      {
        _showCustomPivot = GUILayout.Toggle(_showCustomPivot, "Show Custom Pivot");
        _customPivotColour = EditorGUILayout.ColorField("Indicator Colour", _customPivotColour);
        _customPivotFillMultiplier = EditorGUILayout.Slider("Indicator Opacity", _customPivotFillMultiplier,  0,  1);
        EditorGUILayout.Separator();
      }
      
    }

    public void OnSceneGUI()
    {
      _showSceneViewOverlay.Invoke(null, new object[] {_sceneOverlayWindow});
      if (!_pluginEnabled)
        return;
        
      _blockObject = (BlockObject)target;
      var baseZ = _blockObject.BaseZ;
      var specification = _blockObject.BlocksSpecification;
      var size = specification.Size;
      var blockSpecifications = specification.BlockSpecifications;

      if (_showSize)
        DrawSize(size, baseZ);
      if (_showBaseZ)
        DrawBaseZ(size);
      if (_showOccupation)
        DrawOccupation(blockSpecifications, size, baseZ);
      if (_showCustomPivot && _placeableBlockObjectExists)
        DrawCustomPivot();
    }

    void DrawCustomPivot()
    {
      if (!_placeableBlockObject.CustomPivot.HasCustomPivot)
        return;

      var pivotLocation = TimberbornToUnityVector(_placeableBlockObject.CustomPivot.Coordinates);
      DrawAxis(pivotLocation, 1f);
    }

    void DrawAxis(Vector3 pos, float scale)
    {
      Handles.color = GetTransparent(_customPivotColour, _customPivotFillMultiplier);
      Handles.DrawLine(new Vector3(pos.x - scale, pos.y, pos.z), new Vector3(pos.x + scale, pos.y, pos.z));
      Handles.DrawLine(new Vector3(pos.x, pos.y - scale, pos.z), new Vector3(pos.x, pos.y + scale, pos.z));
      Handles.DrawLine(new Vector3(pos.x, pos.y, pos.z - scale), new Vector3(pos.x, pos.y, pos.z + scale));
    }
    
    void DrawBaseZ(Vector3 size)
    {
      var vector = new Vector3(size.x/2, 0f, size.y/2);
      var sizeVector = new Vector3(size.x+0.4f, 0f, size.y+0.4f);
      Handles.color = _baseZColour;
      Handles.DrawWireCube(vector, sizeVector);
    }

    void DrawSize(Vector3 size, float baseZ)
    {
      var vector = TimberbornToUnityVector(size);
      var center = CorrectFromBaseZ(vector / 2, baseZ);
      Handles.color = GetTransparent(_sizeColour, _sizeFillMultiplier);;
      Handles.DrawWireCube(center, vector);
    }

    void DrawOccupation(BlockSpecification[] blockSpecifications, Vector3Int size, float baseZ)
    {
      var vector = TimberbornToUnityVector(size);
      var index = 0;
      for (var y = 0; y < vector.y; y++)
      {
        for (var z = 0; z < vector.z; z++)
        {
          for (var x = 0; x < vector.x; x++)
          {
            var position = CorrectFromBaseZ(new Vector3(x, y, z), baseZ);
            switch (blockSpecifications[index].Occupation)
            {
              case BlockOccupationSpecification.Full:
                DrawFull(position);
                break;
              case BlockOccupationSpecification.TopAndCorners:
                DrawTopAndCorners(position);
                break;
              case BlockOccupationSpecification.BottomAndFloor:
                DrawBottomAndFloor(position);
                break;
              case BlockOccupationSpecification.Floor:
                DrawFloor(position);
                break;
              case BlockOccupationSpecification.TopAndBottom:
                DrawTopAndBottom(position);
                break;
              case BlockOccupationSpecification.Corners:
                DrawCorners(position);
                break;
              case BlockOccupationSpecification.FullExceptCorners:
                DrawFullExceptCorners(position);
                break;
              case BlockOccupationSpecification.FullExceptTop:
                DrawFullExceptTop(position);
                break;
              case BlockOccupationSpecification.Top:
                DrawTop(position);
                break;
              case BlockOccupationSpecification.Bottom:
                DrawBottom(position);
                break;
              case BlockOccupationSpecification.None:
                break;
              default:
                throw new ArgumentOutOfRangeException();
            }
            index++;
          }
        }
      }
    }

    void DrawOccupationMesh(Vector3 center, Vector3 size)
    {
      Handles.color = GetTransparent(_occupationColour, _occupationFillMultiplier);
      Handles.DrawWireCube(TimberbornToUnityVector(center), size);
    }
    void DrawFull(Vector3 position)
    {
      var unityVector = TimberbornToUnityVector(position + new Vector3(0.5f, 0.5f, 0.5f));
      var size = new Vector3(1f, 1f, 1f);
      DrawOccupationMesh(unityVector, size);
    }
    void DrawTop(Vector3 position)
    {
      var unityVector = TimberbornToUnityVector(position + new Vector3(0.5f, 0.75f, 0.5f));
      var size = new Vector3(0.8f, 0.5f, 0.8f);
      DrawOccupationMesh(unityVector, size);
    }
    void DrawBottom(Vector3 position)
    {
      var unityVector = TimberbornToUnityVector(position + new Vector3(0.5f, 0.3f, 0.5f));
      var size = new Vector3(0.8f, 0.4f, 0.8f);
      DrawOccupationMesh(unityVector, size);
    }
    void DrawFloor(Vector3 position)
    {
      var unityVector = TimberbornToUnityVector(position + new Vector3(0.5f, 0.05f, 0.5f));
      var size = new Vector3(0.8f, 0.1f, 0.8f);
      DrawOccupationMesh(unityVector, size);
    }
    void DrawCorners(Vector3 position)
    {
      var size = new Vector3(0.1f, 1f, 0.1f);
      DrawOccupationMesh(TimberbornToUnityVector(position + new Vector3(0.05f, 0.5f, 0.05f)), size);
      DrawOccupationMesh(TimberbornToUnityVector(position + new Vector3(0.95f, 0.5f, 0.05f)), size);
      DrawOccupationMesh(TimberbornToUnityVector(position + new Vector3(0.05f, 0.5f, 0.95f)), size);
      DrawOccupationMesh(TimberbornToUnityVector(position + new Vector3(0.95f, 0.5f, 0.95f)), size);
    }
    void DrawTopAndCorners(Vector3 position)
    {
      DrawTop(position);
      DrawCorners(position);
    }
    void DrawBottomAndFloor(Vector3 position)
    {
      var unityVector = TimberbornToUnityVector(position + new Vector3(0.5f, 0.25f, 0.5f));
      var size = new Vector3(0.8f, 0.5f, 0.8f);
      DrawOccupationMesh(unityVector, size);
    }
    void DrawTopAndBottom(Vector3 position)
    {
      var unityVector = TimberbornToUnityVector(position + new Vector3(0.5f, 0.55f, 0.5f));
      var size = new Vector3(1f, 0.9f, 1f);
      DrawOccupationMesh(unityVector, size);
    }
    void DrawFullExceptCorners(Vector3 position)
    {
      var unityVector = TimberbornToUnityVector(position + new Vector3(0.5f, 0.5f, 0.5f));
      var size = new Vector3(0.8f, 1f, 0.8f);
      DrawOccupationMesh(unityVector, size);
    }
    void DrawFullExceptTop(Vector3 position)
    {
      var unityVector = TimberbornToUnityVector(position + new Vector3(0.5f, 0.25f, 0.5f));
      var size = new Vector3(0.8f, 0.5f, 0.8f);
      DrawOccupationMesh(unityVector, size);
    }

    Color GetTransparent(Color color, float multiplier)
    {
      return new Color(color.r, color.g, color.b, color.a * multiplier);
    }

    Vector3 TimberbornToUnityVector(Vector3 vector)
    {
      return new Vector3(vector.x, vector.z, vector.y);
    }

    Vector3 CorrectFromBaseZ(Vector3 vector, float baseZ)
    {
      return new Vector3(vector.x, vector.y - baseZ, vector.z);
    }
    
  }
}
#endif
