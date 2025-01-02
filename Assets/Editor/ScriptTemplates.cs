using System.IO;
using System.Text;
using UnityEditor;
using Zenject.Internal;

namespace Editor
{
    public static class ScriptTemplates
    {
        private static readonly string Vowels = "eiouy";

        private static string CreateFile(string friendlyName, string defaultFileName, string folderPath,
            string templateStr, string rejectInName)
        {
            var absolutePath = EditorUtility.SaveFilePanel(
                "Choose name for " + (Vowels.Contains(friendlyName.ToLower()[0]) ? "an " : "a ") + friendlyName,
                folderPath,
                defaultFileName + ".cs",
                "cs");

            if (absolutePath == "") return null;

            if (!absolutePath.ToLower().EndsWith(".cs"))
            {
                absolutePath += ".cs";
            }

            var className = Path.GetFileNameWithoutExtension(absolutePath);
            var rejClassName = className;
            foreach (var rej in rejectInName.Split(';'))
            {
                rejClassName = rejClassName.Replace(rej, "");
            }

            var rejClassNameCamel = new StringBuilder();
            var i = 0;
            foreach (var c in rejClassName)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(c) && i != 0)
                    rejClassNameCamel.Append('_');
                rejClassNameCamel.Append(c.ToString().ToLower());
                i++;
            }

            var rejClassNameSpace = new StringBuilder();
            i = 0;
            foreach (var c in rejClassName)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(c) && i != 0)
                    rejClassNameSpace.Append(' ');
                rejClassNameSpace.Append(c.ToString());
                i++;
            }

            File.WriteAllText(absolutePath, templateStr
                .Replace("#CLASS_NAME#", className)
                .Replace("#REJECTED_CLASS_NAME#", rejClassName)
                .Replace("#REJECTED_CLASS_NAME_CAMEL#", rejClassNameCamel.ToString())
                .Replace("#REJECTED_CLASS_NAME_SPACE#", rejClassNameSpace.ToString())
            );

            AssetDatabase.Refresh();

            var assetPath = ZenUnityEditorUtil.ConvertFullAbsolutePathToAssetPath(absolutePath);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

            return assetPath;
        }

        [MenuItem("Assets/Code Presets/Item", priority = 10000)]
        public static void CreateItemMenu()
        {
            CreateFile("Item", "NewItem", ZenUnityEditorUtil.GetCurrentDirectoryAssetPathFromSelection(),
                @"using UnityEngine;

namespace Inventory.Items
{
    [CreateAssetMenu(fileName = ""New #REJECTED_CLASS_NAME_SPACE#"", menuName = ""Inventory/Items/#REJECTED_CLASS_NAME_SPACE#"", order = 0)]
    public class #CLASS_NAME# : ScriptableObject, IShopItem, ISpriteItem, IStackableClampedItem
    {
        public string Name => ""#REJECTED_CLASS_NAME_SPACE#"";
        public string Id => ""il.#REJECTED_CLASS_NAME_CAMEL#"";
        public ScriptableObject SelfRef => this;
        public Sprite Sprite => sprite;

        [SerializeField] private Sprite sprite;
        [field: SerializeField, Min(1)] public int ItemCost { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; } = 1;
    }
}", "Item");
        }

        [MenuItem("Assets/Code Presets/Ability", priority = 10000)]
        public static void CreateAbilityMenu()
        {
            CreateFile("Ability", "EntityNewAbility", ZenUnityEditorUtil.GetCurrentDirectoryAssetPathFromSelection(),
                @"using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu(""Entity/Abilities/#REJECTED_CLASS_NAME_SPACE#"")]
    public class #CLASS_NAME# : Ability
    {
        
    }
}", "Ability;Entity");
        }

        [MenuItem("Assets/Code Presets/Neurone", priority = 10000)]
        public static void CreateNeuroneMenu()
        {
            CreateFile("Neurone", "NewNeurone", ZenUnityEditorUtil.GetCurrentDirectoryAssetPathFromSelection(),
                @"using UnityEngine;

namespace Entity.AI.Neurones
{
    [AddComponentMenu(""Entity/AI/Neurones/#REJECTED_CLASS_NAME_SPACE#"")]
    public class #CLASS_NAME# : Neurone
    {
        public override void AfterInitialize()
        {
        }
    }
}", "Neurone");
        }

        [MenuItem("Assets/Code Presets/Controller", priority = 10000)]
        public static void CreateControllerMenu()
        {
            CreateFile("Controller", "ControllerNew", ZenUnityEditorUtil.GetCurrentDirectoryAssetPathFromSelection(),
                @"using UnityEngine;

namespace Entity.Controllers
{
    [AddComponentMenu(""Entity/Controllers/#REJECTED_CLASS_NAME_SPACE# Controller"")]
    public class #CLASS_NAME# : Controller
    {
        public override void Initialize()
        {
            base.Initialize();
        }
    }
}", "Controller");
        }
    }
}