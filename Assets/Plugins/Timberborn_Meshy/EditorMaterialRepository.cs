using System.Collections.Generic;
using Timberborn.Meshy;
using UnityEditor;
using UnityEngine;

namespace Timberborn.MeshyEditorTools.Editor {
  internal class EditorMaterialRepository : IMaterialRepository {

    private readonly Dictionary<string, Material> _materials;

    private EditorMaterialRepository(Dictionary<string, Material> materials) {
      _materials = materials;
    }

    public static EditorMaterialRepository Create() {
      return new(GetAllMaterials());
    }

    public Material GetMaterial(string materialName) {
      if (string.IsNullOrWhiteSpace(materialName)) {
        return null;
      }
      if (_materials.TryGetValue(materialName, out var material)) {
        return material;
      }
      Debug.LogError($"Material {materialName} not found");
      return null;
    }

    private static Dictionary<string, Material> GetAllMaterials() {
      var materials = new Dictionary<string, Material>();
      var guids = AssetDatabase.FindAssets("t:Material");
      for (var i = 0; i < guids.Length; i++) {
        var materialPath = AssetDatabase.GUIDToAssetPath(guids[i]);
        var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        if (material != null) {
          materials.TryAdd(material.name, material);
        }
      }
      return materials;
    }

  }
}