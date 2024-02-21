using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Timberborn.AssetSystem.Editor {
  public abstract class BinaryFileImporter : ScriptedImporter {

    public override void OnImportAsset(AssetImportContext context) {
      var mainObject = new GameObject("Binary file");
      var binaryData = mainObject.AddComponent<BinaryData>();
      var fileData = File.ReadAllBytes(context.assetPath);
      binaryData.SetData(fileData);

      PostprocessAsset(context, mainObject);
      if (Icon) {
        context.AddObjectToAsset(Identifier, mainObject, Icon);
      } else {
        context.AddObjectToAsset(Identifier, mainObject);
      }

      context.SetMainObject(mainObject);
    }

    protected abstract string Identifier { get; }

    protected abstract Texture2D Icon { get; }

    protected virtual void PostprocessAsset(AssetImportContext context, GameObject mainObject) {
    }

  }
}