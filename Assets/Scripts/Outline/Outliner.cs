using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Texture2D = UnityEngine.Texture2D;

namespace Outline
{
    public class Outliner
    {
        public static readonly Color OutlineColor = new(1, 1, 1, 1);
#if UNITY_EDITOR
        [MenuItem("File/Regenerate outline", false, 3)]
        private static void Regenerate()
        {
            var mainPath = $"{Application.dataPath}/Drive".Replace('/', '\\');
            var newMainPath = $"{Application.dataPath}/Resources/Outlines".Replace('/', '\\');
            foreach (var path in Directory.EnumerateFiles(mainPath, "*.*",
                         new EnumerationOptions { RecurseSubdirectories = true }))
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

                EditorUtility.CopySerialized(
                    AssetImporter.GetAtPath(path.Replace(mainPath, "Assets/Drive")) as TextureImporter,
                    AssetImporter.GetAtPath(path.Replace(mainPath, "Assets/Resources/Outlines")) as TextureImporter
                );

                AssetDatabase.ImportAsset(
                    path.Replace(mainPath, "Assets/Resources/Outlines"),
                    ImportAssetOptions.ForceUpdate
                );
            }
        }

        private static Texture2D GenerateNewOutline(Texture2D original)
        {
            var newTexture = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);
            for (var x = 0; x < original.width; x++)
            {
                for (var y = 0; y < original.height; y++)
                {
                    var pixel = original.GetPixel(x, y);
                    newTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                    if (pixel.a != 0) continue;
                    var r = CheckPosition(newTexture, x + 1, y) && original.GetPixel(x + 1, y).a != 0;
                    var l = CheckPosition(newTexture, x - 1, y) && original.GetPixel(x - 1, y).a != 0;
                    var u = CheckPosition(newTexture, x, y + 1) && original.GetPixel(x, y + 1).a != 0;
                    var d = CheckPosition(newTexture, x, y - 1) && original.GetPixel(x, y - 1).a != 0;
                    if (r || l || u || d)
                        newTexture.SetPixel(x, y, OutlineColor);
                }
            }

            newTexture.Apply(false);
            return newTexture;
        }
#endif
        private static bool CheckPosition(Texture texture, int x, int y) =>
            CheckPosition(texture, new Vector2Int(x, y));

        private static bool CheckPosition(Texture texture, Vector2Int pos) =>
            pos.x >= 0 && pos.y >= 0 && pos.x < texture.width && pos.y < texture.height;

        public static Sprite GetOutlined(string path)
        {
            path = path
                .Replace('/', '\\')
                .Replace($"{Application.dataPath}\\Drive".Replace('/', '\\'), "Outlines")
                .Replace("Assets\\Drive", "Outlines")
                .Replace("Drive", "Outlines");
            return Resources.Load<Sprite>(path);
        }
    }
}