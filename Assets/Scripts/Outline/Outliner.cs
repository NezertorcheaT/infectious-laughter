using System.IO;
using CustomHelper;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Texture2D = UnityEngine.Texture2D;

namespace Outline
{
    public class Outliner
    {
        public static readonly Color OutlineColor = new(1, 1, 1, 1);
        private static int _kernel;
        private static int _threadGroupSize = 3;
        private static ComputeShader _shader;
#if UNITY_EDITOR
        [MenuItem("File/Regenerate outline", false, 3)]
        public static void Regenerate()
        {
            var mainPath = $"{Application.dataPath}/Drive".Replace('/', '\\');
            var newMainPath = $"{Application.dataPath}/Resources/Outlines".Replace('/', '\\');
            if (Directory.Exists(newMainPath))
                Directory.Delete(newMainPath, true);
            var container = Resources.Load<OutlinesContainer>("OutlinesContainer");
            container.Reset();
            _shader = Resources.Load<ComputeShader>("OutlineShader");
            _kernel = _shader.FindKernel("cs_main");
            //_shader.GetKernelThreadGroupSizes(_kernel, out _threadGroupSize, out _, out _);
            foreach (var path in Directory.EnumerateFiles(
                         mainPath,
                         "*.*",
                         new EnumerationOptions { RecurseSubdirectories = true }
                     ))
            {
                if (path.Contains("Concepts"))
                    continue;
                if (!path.EndsWith(".png") && !path.EndsWith(".jpg") && !path.EndsWith(".jpeg"))
                    continue;
                var asset = AssetDatabase.LoadAssetAtPath<Sprite>(path.Replace(mainPath, "Assets/Drive"));
                if (asset?.texture is null)
                    continue;
                byte[] texture = GenerateNewOutline(asset.texture).EncodeToPNG();

                var newPath = path.Replace(mainPath, newMainPath);
                if (!Directory.Exists(newPath.Replace(Path.GetFileName(newPath), "")))
                    Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                if (texture is not null)
                    File.WriteAllBytes(newPath, texture);

                AssetDatabase.ImportAsset(
                    path.Replace(mainPath, "Assets/Resources/Outlines"),
                    ImportAssetOptions.ForceUpdate
                );

                var destinationImporter =
                    AssetImporter.GetAtPath(path.Replace(mainPath, "Assets/Resources/Outlines")) as TextureImporter;
                EditorUtility.CopySerialized(
                    AssetImporter.GetAtPath(path.Replace(mainPath, "Assets/Drive")) as TextureImporter,
                    destinationImporter
                );
                destinationImporter.textureCompression = TextureImporterCompression.CompressedLQ;
                destinationImporter.crunchedCompression = true;
                destinationImporter.compressionQuality = 50;
                destinationImporter.SaveAndReimport();

                AssetDatabase.ImportAsset(
                    path.Replace(mainPath, "Assets/Resources/Outlines"),
                    ImportAssetOptions.ForceUpdate
                );
                container.Cache.Add(new OutlinesContainer.OutlineType
                    { Original = asset, Path = path.Replace(mainPath, "Outlines").Replace(".png", "") });
            }

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(container);
            AssetDatabase.SaveAssetIfDirty(container);
        }

        private static Texture2D GenerateNewOutline(Texture2D original)
        {
            var newTexture = new RenderTexture(original.width, original.height, 16);
            newTexture.enableRandomWrite = true;
            _shader.SetTexture(_kernel, "original", original);
            _shader.SetTexture(_kernel, "result", newTexture);
            _shader.Dispatch(_kernel,
                original.width,
                original.height,
                1
            );
            return newTexture.ToTexture2D();
        }
#endif
        private static bool CheckPosition(Texture texture, int x, int y) =>
            CheckPosition(texture, new Vector2Int(x, y));

        private static bool CheckPosition(Texture texture, Vector2Int pos) =>
            pos.x >= 0 && pos.y >= 0 && pos.x < texture.width && pos.y < texture.height;

        public static Sprite[] GetOutlined(string path)
        {
            path = path
                .Replace('/', '\\')
                .Replace($"{Application.dataPath}\\Drive".Replace('/', '\\'), "Outlines")
                .Replace("Assets\\Drive", "Outlines")
                .Replace("Drive", "Outlines");
            return Resources.LoadAll<Sprite>(path);
        }
    }
#if UNITY_EDITOR
    public class OutlineBeforeBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            Outliner.Regenerate();
        }
    }
#endif
}

namespace CustomHelper
{
    public static partial class Helper
    {
        public static Texture2D ToTexture2D(this RenderTexture rTex)
        {
            var tex = new Texture2D(rTex.width, rTex.height);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}