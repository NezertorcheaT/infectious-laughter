using System;

namespace Saving
{
    /// <summary>
    /// этот класс должен хранить в себе только и только ключи для сессий
    /// </summary>
    public static partial class SavedKeys
    {
        public static readonly SavingKey Level =
            SavingKey.New("game.current_level", "0");

        public static readonly SavingKey PlayerInventory =
            SavingKey.New("game.player_inventory", "");

        public static readonly SavingKey PlayerGarbage =
            SavingKey.New("game.player_garbage", 0);

        public static readonly SavingKey PlayerHp =
            SavingKey.New("game.player_hp", 5);

        public static readonly SavingKey PlayerAddictiveHp =
            SavingKey.New("game.player_addictive_hp", 0);

        public static readonly SavingKey PlayerMaxHp =
            SavingKey.New("game.player_max_hp", 5);

        public static readonly SavingKey PlayerMaxAddictiveHp =
            SavingKey.New("game.player_max_addictive_hp", 0);

        public static readonly SavingKey Seed =
            SavingKey.New("game.generation_seed", Guid.NewGuid().ToString());
        
        public static readonly SavingKey PlayerInventorySelection =
            SavingKey.New("game.player_inventory_selection", 0);
    }
}