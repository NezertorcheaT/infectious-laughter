using System.IO;
using System.Linq;
using CustomHelper;
using UnityEditor;
using UnityEditor.U2D;
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
        private static ComputeShader _shader;
        public static readonly string MainPath = $"{Application.dataPath}/Drive".Replace('/', '\\');
        public static readonly string NewMainPath = $"{Application.dataPath}/Outlines".Replace('/', '\\');
#if UNITY_EDITOR

        public static Sprite Regenerate(Sprite original, string path)
        {
            Debug.Log(path);
            _shader ??= Resources.Load<ComputeShader>("OutlineShader");
            _kernel = _shader.FindKernel("cs_main");
            if (original?.texture is null)
                return null;
            byte[] texture = GenerateNewOutline(original.texture).EncodeToPNG();

            var newPath = path.Replace(MainPath, NewMainPath);
            var relativeNewPath = path.Replace(MainPath, "Assets/Outlines");
            var relativeMainPath = path.Replace(MainPath, "Assets/Drive");

            if (!Directory.Exists(newPath.Replace(Path.GetFileName(newPath), "")))
                Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
            if (texture is not null)
                File.WriteAllBytes(newPath, texture);

            AssetDatabase.ImportAsset(
                relativeNewPath,
                ImportAssetOptions.ForceUpdate
            );

            var destinationImporter =
                AssetImporter.GetAtPath(relativeNewPath) as TextureImporter;
            EditorUtility.CopySerialized(
                AssetImporter.GetAtPath(relativeMainPath) as TextureImporter,
                destinationImporter
            );
            destinationImporter.textureCompression = TextureImporterCompression.CompressedLQ;
            destinationImporter.crunchedCompression = true;
            destinationImporter.compressionQuality = 50;
            destinationImporter.SaveAndReimport();

            AssetDatabase.ImportAsset(
                relativeNewPath,
                ImportAssetOptions.ForceUpdate
            );

            return AssetDatabase
                .LoadAllAssetsAtPath(relativeNewPath)
                .AsType<Sprite>()
                .FirstOrDefault(i => i.GetSpriteID() == original.GetSpriteID());
        }

        [MenuItem("File/Regenerate outlines", false, 3)]
        public static void Regenerate()
        {
            if (Directory.Exists(NewMainPath))
                Directory.Delete(NewMainPath, true);
            var container = Resources.Load<OutlinesContainer>("OutlinesContainer");
            container.Reset();
            _shader = Resources.Load<ComputeShader>("OutlineShader");
            _kernel = _shader.FindKernel("cs_main");
            foreach (var path in Directory.EnumerateFiles(
                         MainPath,
                         "*.*",
                         new EnumerationOptions { RecurseSubdirectories = true }
                     ))
            {
                if (path.Contains("Concepts"))
                    continue;
                if (!path.EndsWith(".png") && !path.EndsWith(".jpg") && !path.EndsWith(".jpeg"))
                    continue;

                foreach (var orig in AssetDatabase.LoadAllAssetsAtPath(path.Replace(MainPath, "Assets/Drive"))
                             .AsType<Sprite>())
                {
                    var outlineType = new OutlinesContainer.OutlineType
                        { Original = orig, New = Regenerate(orig, path) };
                    if (outlineType.New is null || outlineType.Original is null) continue;
                    container.Cache.Add(outlineType);
                }
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