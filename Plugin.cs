using BepInEx;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CosmeticGUI
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private bool guiEnabled = false;
        public Rect rect = new Rect(400, 10, 120, 90);

        private void Update()
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame) guiEnabled = !guiEnabled;
        }

        private void OnGUI()
        { 
            if (guiEnabled)
            {
                rect = GUI.Window(1, rect, CosmeticsWindow, $"Cosmetic GUI");
            }
        }

        private string _cosmetic;
        private void CosmeticsWindow(int windowID)
        {
            _cosmetic = GUI.TextArea(new Rect(10, 20, 100, 20), _cosmetic);
            if (GUI.Button(new Rect(10, 40, 100, 20), "Purchase"))
            {
                foreach (var item in CosmeticsController.instance.allCosmetics)
                {
                    if (item.itemName == _cosmetic)
                    {
                        CosmeticsController.instance.itemToBuy = item;
                        CosmeticsController.instance.PurchaseItem();
                        UpdateCosmetics();
                    }
                }
            }

            if (GUI.Button(new Rect(10, 60, 100, 20), "Try On"))
            {
                var item = CosmeticsController.instance.allCosmeticsDict[_cosmetic];
                
                if (CosmeticsController.instance.currentCart.Contains(item))
                {
                    GorillaTagger.Instance.offlineVRRig.concatStringOfCosmeticsAllowed.Replace(item.itemName, "");
                    CosmeticsController.instance.currentCart.Remove(item);
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.concatStringOfCosmeticsAllowed += item.itemName;
                    CosmeticsController.instance.currentCart.Add(item);
                }
                UpdateCosmetics();
            }
            
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        private void UpdateCosmetics()
        {
            var instance = CosmeticsController.instance;
            instance.UpdateCurrencyBoards();
            instance.UpdateMyCosmetics();
            instance.UpdateShoppingCart();
            instance.UpdateWardrobeModelsAndButtons();
            instance.UpdateWornCosmetics();
        }
    }
}
