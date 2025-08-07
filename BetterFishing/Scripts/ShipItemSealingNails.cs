namespace BetterFishing
{
    public class ShipItemSealingNails : ShipItem
    {
        public override void UpdateLookText()
        {
            if (!sold)
                return;
            
            lookText = $"sealing nails\n({amount})";
        }
    }
}
