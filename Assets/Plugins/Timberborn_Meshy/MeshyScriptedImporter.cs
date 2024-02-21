using System;
using System.IO;
using Timberborn.AssetSystem;
using Timberborn.AssetSystem.Editor;
using Timberborn.Meshy;
using Timberborn.MeshyDTO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Timberborn.MeshyEditorTools.Editor {
  [ScriptedImporter(1, "meshy")]
  public class MeshyScriptedImporter : BinaryFileImporter {

    protected override string Identifier => "MeshyModel";
    protected override Texture2D Icon => Resources.Load<Texture2D>("Editor/Meshy");

    protected override void PostprocessAsset(AssetImportContext context, GameObject mainObject) {
      var binaryData = mainObject.GetComponent<BinaryData>();
      using var memoryStream = new MemoryStream(binaryData.Bytes);
      var meshyModel = MeshyReader.ReadFromStream(memoryStream);

      CreateAssetContent(context, mainObject, meshyModel);
      ModelMetadata.Create(mainObject, meshyModel);
    }

    private static void CreateAssetContent(AssetImportContext context, GameObject mainObject,
                                           Model meshyModel) {
      var staticMeshBuilder = new StaticMeshBuilder(EditorMaterialRepository.Create());
      var meshyImporter = new MeshyImporter(staticMeshBuilder, Array.Empty<IModelPostprocessor>());
      var importDetails = meshyImporter.Import(meshyModel, mainObject.transform);

      foreach (var (node, createdObject) in importDetails.CreatedObjectsMap) {
        if (node.VertexCount > 0) {
          var mesh = createdObject.GetComponent<MeshFilter>().sharedMesh;
          context.AddObjectToAsset($"{node.Name} Mesh", mesh);
        }
      }
    }

  }
}