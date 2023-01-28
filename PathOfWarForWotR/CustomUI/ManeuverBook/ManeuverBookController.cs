using Kingmaker.UI.MVVM._PCView.Party;
using Kingmaker.UI.MVVM._PCView.ServiceWindows.Spellbook.KnownSpells;
using Kingmaker.Utility;
using Owlcat.Runtime.UI.Controls.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;



namespace PathOfWarForWotR.CustomUI.ManeuverBook
{
    class ManeuverBookController : MonoBehaviour
    {
        private ManeuverBookView view;

        public void Create()
        {
            Main.Context.Logger.Log("TIC Spellbook Controller Created");
            view.widgetCache = new();
            view.widgetCache.PrefabGenerator = () => {
                SpellbookKnownSpellPCView spellPrefab = null;
                var listPrefab = UIHelpers.SpellbookScreen.Find("MainContainer/KnownSpells");
                var spellsKnownView = listPrefab.GetComponent<SpellbookKnownSpellsPCView>();

                if (spellsKnownView != null)
                    spellPrefab = listPrefab.GetComponent<SpellbookKnownSpellsPCView>().m_KnownSpellView;
                else
                {
                    foreach (var component in UIHelpers.SpellbookScreen.gameObject.GetComponents<Component>())
                    {
                        if (component.GetType().FullName == "EnhancedInventory.Controllers.SpellbookController")
                        {
                           // Main.Verbose(" ** INSTALLING WORKAROUND FOR ENHANCED INVENTORY **");
                            var fieldHandle = component.GetType().GetField("m_known_spell_prefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                           // Main.Verbose($"Got field handle: {fieldHandle != null}");
                            spellPrefab = (SpellbookKnownSpellPCView)fieldHandle.GetValue(component);
                          //  Main.Verbose($"Found spellPrefab: {spellPrefab != null}");

                            break;
                        }
                    }
                }

                ManeuverRoot = GameObject.Instantiate(spellPrefab.gameObject);

                ManeuverRoot.name = "ManeuverBookView";
                ManeuverRoot.DestroyComponents<SpellbookKnownSpellPCView>();
                ManeuverRoot.DestroyChildrenImmediate("Icon/Decoration", "Icon/Domain", "Icon/MythicArtFrame", "Icon/ArtArrowImage", "RemoveButton", "Level");

                return ManeuverRoot;

            };

        }
        private GameObject MainContainer;
        private GameObject ManeuverRoot;
        private bool WasSpellsShown = false;
        private bool AreSpellsShowing => MainContainer.activeSelf;
        private bool IsNoSpellShowing => NoSpellbooksContainer.activeSelf;
        private GameObject NoSpellbooksContainer;
        private PartyPCView PartyView;
        private GameObject ManeuversToggleButton;
        
        private void Awake()
        {
            Main.Context.Logger.Log($"TIC Spellbook Controller Awake");

            MainContainer = transform.Find("MainContainer").gameObject;
            NoSpellbooksContainer = transform.Find("NoSpellbooksContainer").gameObject;
            PartyView = UIHelpers.StaticRoot.Find("PartyPCView").gameObject.GetComponent<PartyPCView>();

            GameObject.Destroy(transform.Find("maneuversToggleButton")?.gameObject);
            GameObject.Destroy(transform.Find("spellsToggleButton")?.gameObject);
            GameObject.Destroy(transform.Find("bubblebuff-root")?.gameObject);

            ManeuversToggleButton = GameObject.Instantiate(transform.Find("MainContainer/MetamagicButton").gameObject, transform);
            (ManeuversToggleButton.transform as RectTransform).anchoredPosition = new Vector2(1800, 0);
            ManeuversToggleButton.name = "maneuversToggleButton";
            ManeuversToggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Maneuvers";
          
            
           
        }

        /*

            [HarmonyPatch(typeof(SpellbookVM), argumentTypes: new Type[] { typeof(IReactiveProperty<UnitDescriptor>) }, methodType: MethodType.Constructor)]
        public static class TriggerOnMake
        {
            static void Postfix(SpellbookVM __result, IReactiveProperty<UnitDescriptor> unitDescriptor)
            {
                Main.Context.Logger.Log($"In SpellbookVM constuctor override override");
                
                if (__result.CurrentSpellbook.HasValue)
                {
                    var book = __result.CurrentSpellbook.Value.Blueprint.Components.OfType<ManeuverBookComponent>();
                    if (book != null)
                    {
                        Main.Log($"Passing To Controller");
                        GlobalPath.Instance.PotCSpellbookController.Modify(__result.CurrentSpellbook.Value);
                    }
                }
                
            }
        }
            */
    }

    class WidgetCache
    {
        public int Hits;
        public int Misses;
        private GameObject prefab;
        private readonly List<GameObject> cache = new();

        public Func<GameObject> PrefabGenerator;

        public void ResetStats()
        {
            Hits = 0;
            Misses = 0;
        }

        public WidgetCache() { }

        public GameObject Get(Transform parent)
        {
            if (prefab == null)
            {
                prefab = PrefabGenerator.Invoke();
                if (prefab == null)
                    throw new Exception("null prefab in widget cache");
            }
            GameObject ret;
            if (cache.Empty())
            {
                ret = GameObject.Instantiate(prefab, parent);
                Misses++;
            }
            else
            {
                Hits++;
                ret = cache.Last();
                ret.transform.SetParent(parent);
                cache.RemoveLast();
            }
            ret.SetActive(true);
            return ret;
        }

        public void Return(IEnumerable<GameObject> widgets)
        {
            //cache.AddRange(widgets);
        }

    }
}
