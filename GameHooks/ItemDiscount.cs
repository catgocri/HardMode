
using System;
using HarmonyLib;

namespace catgocrihxpmods.HardMode.PotionCraft.GameHooks
{
    [HarmonyPatch(typeof(TradeManager), "GetDiscountForItem")]
    public static class ItemDiscount
    {
        public static EventHandler<ItemDiscountEventArgs> OnCalculateItemDiscount;

        static bool Postfix(ref float __result, InventoryItem item)
        {
            var e = new ItemDiscountEventArgs(item);
            OnCalculateItemDiscount?.Invoke(null, e);
            if (e.CostMultiplier.HasValue)
            {
                __result = e.CostMultiplier.Value;
                return false;
            }

            return true;
        }
    }

    public class ItemDiscountEventArgs : EventArgs
    {
        public ItemDiscountEventArgs(InventoryItem item)
        {
            Item = item;
        }
        public InventoryItem Item { get; }
        public float? CostMultiplier { get; set; }
    }
}
