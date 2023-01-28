using Kingmaker;
using Kingmaker.Utility;
using System.Linq;
using UnityEngine;

namespace PathOfWarForWotR.CustomUI
{
    public static class UIHelpers
    {
        public static Transform StaticRoot => Game.Instance.UI.Canvas.transform;
        public static Transform ServiceWindow => StaticRoot.Find("ServiceWindowsPCView");

        public static Transform SpellbookScreen => ServiceWindow.Find(SpellScreen);
        public static string SpellScreen => "SpellbookPCView/SpellbookScreen";
        public static GameObject[] ChildObjects(this GameObject obj, params string[] paths)
        {
            return paths.Select(p => obj.transform.Find(p)?.gameObject).ToArray();
        }
        public static void DestroyChildrenImmediate(this GameObject obj, params string[] paths)
        {
            obj.ChildObjects(paths).ForEach(GameObject.DestroyImmediate);
        }
        public static void DestroyComponents<T>(this GameObject obj) where T : UnityEngine.Object
        {
            var componentList = obj.GetComponents<T>();
            foreach (var c in componentList)
                GameObject.DestroyImmediate(c);
        }
    }
}
