using System;
using System.IO;
using Timberborn.AssetSystem;
using Timberborn.Meshy;
using UnityEngine;

namespace Timberborn.MeshyEditorTools.Editor {
  internal static class MeshyPreviewFactory {

    public static void Create(MeshyDescription meshyDescription) {
      try {
        var staticMeshBuilder = new StaticMeshBuilder(EditorMaterialRepository.Create());
        var importer = new MeshyImporter(staticMeshBuilder, Array.Empty<IModelPostprocessor>());
        var modelMetadata = Resources.Load<ModelMetadata>(meshyDescription.ModelName);
        var modelBytes = modelMetadata.GetComponent<BinaryData>().Bytes;
        using var memoryStream = new MemoryStream(modelBytes);
        importer.ImportAsPreview(memoryStream, meshyDescription.transform);
      } catch (Exception e) {
        Debug.LogError($"Failed to create preview for {meshyDescription.name}: {e}");
      }
    }

  }
}