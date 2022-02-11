namespace PokeParadise
{
    public static class PokemonContextCancel
    {
        public static void Close()
        {
            PlayerData.ClosePanels();
        }
    }
}