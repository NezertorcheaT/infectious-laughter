using System;

namespace Saving
{
    /// <summary>
    /// этот класс должен хранить в себе только и только ключи для сессий
    /// </summary>
    public static partial class SavedKeys
    {
        public static readonly SavingKey Level =
            SavingKey.New("game.saved_level_key", "0");

        public static readonly SavingKey PlayerInventory =
            SavingKey.New("game.saved_player_inventory_key", "");

        public static readonly SavingKey PlayerGarbage =
            SavingKey.New("game.saved_player_garbage_key", 0);

        public static readonly SavingKey PlayerHp =
            SavingKey.New("game.saved_player_hp_key", 5);

        public static readonly SavingKey PlayerAddictiveHp =
            SavingKey.New("game.saved_player_addictive_hp_key", 0);

        public static readonly SavingKey PlayerMaxHp =
            SavingKey.New("game.saved_player_max_hp_key", 5);

        public static readonly SavingKey PlayerMaxAddictiveHp =
            SavingKey.New("game.saved_player_max_addictive_hp_key", 0);

        public static readonly SavingKey Seed =
            SavingKey.New("game.saved_seed", Guid.NewGuid().ToString());
        
        public static readonly SavingKey PlayerInventorySelection =
            SavingKey.New("game.player_inventory_selection", 0);
    }

    public class SavingKey
    {
        public string Key { get; private set; }
        public Type Type { get; private set; }
        public object Default { get; private set; }

        public static SavingKey New<T>(string key, T value)
        {
            return new SavingKey
            {
                Key = key,
                Type = typeof(T),
                Default = value
            };
        }
    }
}