namespace BetterFishing
{
    public class ShipItemSealingNails : ShipItem
    {
        public override void UpdateLookText()
        {
            lookText = $"sealing nails\n({amount})";
        }
    }
}
