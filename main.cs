using System;
using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;

namespace SolastaNewMerchantTest
{
    public class Main
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg)
        {
            if (logger != null) logger.Log(msg);
        }
        public static void Error(Exception ex)
        {
            if (logger != null) logger.Error(ex.ToString());
        }
        public static void Error(string msg)
        {
            if (logger != null) logger.Error(msg);
        }

        public static UnityModManager.ModEntry.ModLogger logger;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                logger = modEntry.Logger;
                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
            return true;
        }
        [HarmonyPatch(typeof(MainMenuScreen), "RuntimeLoaded")]
        static class MainMenuScreen_RuntimeLoaded_Patch
        {
            static void Postfix()
            {
                try
                {
                    var gorims = DatabaseRepository.GetDatabase<MerchantDefinition>().GetElement("Store_Merchant_Gorim_Ironsoot_Cyflen_GeneralStore");
                    var summers = DatabaseRepository.GetDatabase<MerchantDefinition>().GetElement("Store_Merchant_Antiquarians_Halman_Summer");
                    var rosa = DatabaseRepository.GetDatabase<ItemDefinition>().GetElement("EnchantingTool");
                    var GS1p = DatabaseRepository.GetDatabase<ItemDefinition>().GetElement("Greatsword+1");
                    var LS1p = DatabaseRepository.GetDatabase<ItemDefinition>().GetElement("Longsword+1");
                    var rosastock = new StockUnitDescription();
                    var GSstock = new StockUnitDescription();
                    var LSstock = new StockUnitDescription();
                    AccessTools.Field(rosastock.GetType(), "itemDefinition").SetValue(rosastock, rosa);
                    AccessTools.Field(GSstock.GetType(), "itemDefinition").SetValue(GSstock, GS1p);
                    AccessTools.Field(LSstock.GetType(), "itemDefinition").SetValue(LSstock, LS1p);
                    gorims.StockUnitDescriptions.Add(rosastock);
                    gorims.StockUnitDescriptions.Add(GSstock);
                    gorims.StockUnitDescriptions.Add(LSstock);
                    summers.StockUnitDescriptions.Add(rosastock);
                    summers.StockUnitDescriptions.Add(GSstock);
                    summers.StockUnitDescriptions.Add(LSstock);
                }
                catch (Exception ex)
                {
                    Error(ex);
                    throw;
                }
            }
        }
    }
}
