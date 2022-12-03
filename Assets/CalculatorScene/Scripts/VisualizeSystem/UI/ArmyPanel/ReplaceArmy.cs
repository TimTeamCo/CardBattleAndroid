namespace TTBattle.UI
{
    public static class ReplaceArmy
    {
        public static void Execute(ArmyPanel Army1, ArmyPanel Army2)
        {
            SwapDropdownValues(Army1, Army2);
            SwapPlayers(Army1, Army2);
            Army1.SetArmyValues();
            Army2.SetArmyValues();
        }

        private static void SwapDropdownValues(ArmyPanel Army1, ArmyPanel Army2)
        {

            var army1DropdownImageColor = Army1.UnitDropdownImage.color;
            var army2DropdownImageColor = Army2.UnitDropdownImage.color;
            var Army1Color = Army1.UnitDropdownTemplateImage.color;
            var Army2Color = Army2.UnitDropdownTemplateImage.color;
            Army1.UnitDropdown.value = 0;
            Army2.UnitDropdown.value = 0;
            Army1.UnitDropdownImage.color = army2DropdownImageColor;
            Army2.UnitDropdownImage.color = army1DropdownImageColor;
            Army1.UnitDropdownTemplateImage.color = Army2Color;
            Army2.UnitDropdownTemplateImage.color = Army1Color;
        }

        private static void SwapPlayers(ArmyPanel Army1, ArmyPanel Army2)
        {
            var player = Army1.playerData;
            Army1.playerData = Army2.playerData;
            Army2.playerData = player;
        }
    }
}