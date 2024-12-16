using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// это предмет (методы без обобщений), который имеет данные, привязанные к конкретному предмету
    /// </summary>
    public interface IStashingItem : IEndableItem, IStartableItem
    {
        /// <summary>
        /// внутренний метод для начального создания хранилища
        /// </summary>
        void InitializeStash();

        /// <summary>
        /// имеет ли данные для предмета
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns></returns>
        bool HasStored(ItemData itemData);
    }

    /// <summary>
    /// это предмет, который имеет данные, привязанные к конкретному предмету
    /// </summary>
    /// <typeparam name="T">тип данных, который будет использоваться для хранения, советую сделать побольше</typeparam>
    public interface IStashingItem<T> : IStashingItem where T : class
    {
        /// <summary>
        /// это данные каждого предмета
        /// </summary>
        Stash Data { get; }

        bool IStashingItem.HasStored(ItemData itemData) => Data.Has(itemData);

        void IStartableItem.OnStart(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            Data.Add(itemData, Initiate(entity, inventory, itemData));
            Started(entity, inventory, itemData);
        }

        void IEndableItem.OnEnded(Entity.Entity entity, IInventory inventory, ItemData itemData)
        {
            var c = Data[itemData];
            Data.Pop(itemData);
            End(entity, inventory, itemData, c);
        }

        T Initiate(Entity.Entity entity, IInventory inventory, ItemData itemData);
        void Started(Entity.Entity entity, IInventory inventory, ItemData itemData);
        void End(Entity.Entity entity, IInventory inventory, ItemData itemData, T c);

        /// <summary>
        /// структура данных, скрывающая данные каждого предмета
        /// </summary>
        public class Stash : IEnumerable<KeyValuePair<ItemData, T>>
        {
            private readonly ConcurrentDictionary<ItemData, T> _dictionary = new();

            /// <summary>
            /// получить данные для предмета
            /// </summary>
            /// <param name="itemData">именно слот используется как ключ к данным предмета</param>
            public T this[ItemData itemData] => Get(itemData);

            /// <summary>
            /// получить данные для предмета
            /// </summary>
            /// <param name="itemData">именно слот используется как ключ к данным предмета</param>
            /// <returns>данные о предмете</returns>
            /// <exception cref="ArgumentException">если данные к этому слоту отсутствуют</exception>
            public T Get(ItemData itemData)
            {
                if (_dictionary.TryGetValue(itemData, out T value)) return value;
                throw new ArgumentException($"Данные к слоту {itemData} отсутствуют");
            }

            /// <summary>
            /// имеет ли данные для предмета
            /// </summary>
            /// <param name="itemData"></param>
            /// <returns></returns>
            public bool Has(ItemData itemData) => _dictionary.ContainsKey(itemData);

            /// <summary>
            /// позволяет убрать данные о предмете
            /// </summary>
            /// <param name="itemData">именно слот используется как ключ к данным предмета</param>
            /// <exception cref="ArgumentException">если данные к этому слоту отсутствуют</exception>
            public void Pop(ItemData itemData)
            {
                if (_dictionary.TryRemove(itemData, out _)) return;
                throw new ArgumentException($"Данные к слоту {itemData} отсутствуют");
            }

            /// <summary>
            /// записать данные о предмете
            /// </summary>
            /// <param name="itemData">используется как ключ к данным предмета</param>
            /// <param name="current">данные, которые следует записать</param>
            /// <exception cref="ArgumentException">если данные к этому слоту уже существуют</exception>
            public void Add(ItemData itemData, T current)
            {
                if (_dictionary.TryAdd(itemData, current)) return;
                throw new ArgumentException("item in this slot already contained in stash");
            }

            public IEnumerator<KeyValuePair<ItemData, T>> GetEnumerator() => _dictionary.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}