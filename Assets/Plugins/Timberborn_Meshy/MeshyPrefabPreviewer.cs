using Timberborn.Meshy;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Timberborn.MeshyEditorTools.Editor {
  [InitializeOnLoad]
  internal class MeshyPrefabPreviewer {

    static MeshyPrefabPreviewer() {
      PrefabStage.prefabStageOpened += OnPrefabStageOpened;
      Menu.SetChecked(AutoPreviewTogglePath, true);
    }

    [MenuItem(AutoPreviewTogglePath, false, 201)]
    public static void ToggleAutoPreview() {
      Menu.SetChecked(AutoPreviewTogglePath, !AutoPreviewEnabled);
    }

    private static bool AutoPreviewEnabled => Menu.GetChecked(AutoPreviewTogglePath);

    private static void OnPrefabStageOpened(PrefabStage prefabStage) {
      var root = prefabStage.prefabContentsRoot;
      if (AutoPreviewEnabled && root != null) {
        CreatePreviews(root);
      }
    }

    private static void CreatePreviews(GameObject root) {
      var meshyDescriptions = root.GetComponentsInChildren<MeshyDescription>(true);
      foreach (var meshyDescription in meshyDescriptions) {
        MeshyPreviewFactory.Create(meshyDescription);
      }
    }

    private const string AutoPreviewTogglePath = "Timberborn/Automatically preview meshy models";

  }
}