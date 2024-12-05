using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// это предмет, который имеет данные, привязанные к конкретному предмету
    /// </summary>
    /// <typeparam name="T">тип данных, который будет использоваться для хранения, советую сделать побольше</typeparam>
    public interface IStashingItem<T> : IEndableItem, IStartableItem where T : class
    {
        /// <summary>
        /// это данные каждого предмета
        /// </summary>
        Stash Data { get; }

        public class Stash
        {
            private readonly ConcurrentDictionary<ISlot, T> _dictionary;

            public Stash(ISlot slot, T initial) : this() => Add(slot, initial);
            public Stash() => _dictionary = new ConcurrentDictionary<ISlot, T>();

            /// <summary>
            /// получить данные для предмета
            /// </summary>
            /// <param name="slot">именно слот используется как ключ к данным предмета</param>
            public T this[[NotNull] ISlot slot] => Get(slot);

            /// <summary>
            /// получить данные для предмета
            /// </summary>
            /// <param name="slot">именно слот используется как ключ к данным предмета</param>
            /// <returns>данные о предмете</returns>
            /// <exception cref="ArgumentException">если данные к этому слоту отсутствуют</exception>
            /// <exception cref="NullReferenceException">если слот нулёвый</exception>
            public T Get([NotNull] ISlot slot)
            {
                if (slot is null) throw new NullReferenceException($"Слоту {slot} нулёвый, идиот");
                if (_dictionary.TryGetValue(slot, out T value)) return value;
                throw new ArgumentException($"Данные к слоту {slot} отсутствуют");
            }

            /// <summary>
            /// позволяет убрать данные о предмете
            /// </summary>
            /// <param name="slot">именно слот используется как ключ к данным предмета</param>
            /// <exception cref="ArgumentException">если данные к этому слоту отсутствуют</exception>
            /// <exception cref="NullReferenceException">если слот нулёвый</exception>
            public void Pop([NotNull] ISlot slot)
            {
                if (slot is null) throw new NullReferenceException($"Слоту {slot} нулёвый, идиот");
                if (_dictionary.TryRemove(slot, out _)) return;
                throw new ArgumentException($"Данные к слоту {slot} отсутствуют");
            }

            /// <summary>
            /// записать данные о предмете
            /// </summary>
            /// <param name="slot">именно слот используется как ключ к данным предмета</param>
            /// <param name="current">данные, которые следует записать</param>
            /// <exception cref="ArgumentException">если данные к этому слоту уже существуют</exception>
            /// <exception cref="NullReferenceException">если слот нулёвый</exception>
            public void Add([NotNull] ISlot slot, T current)
            {
                if (slot is null) throw new NullReferenceException($"Слоту {slot} нулёвый, идиот");
                if (_dictionary.TryAdd(slot, current)) return;
                throw new ArgumentException("item in this slot already contained in stash");
            }
        }
    }

    /// <summary>
    /// это предмет, который имеет данные, привязанные к конкретному предмету
    /// </summary>
    /// <typeparam name="T">тип данных, который будет использоваться для хранения, советую сделать побольше</typeparam>
    public abstract class StashingItem<T> : ScriptableObject, IStashingItem<T> where T : class
    {
        public abstract ScriptableObject SelfRef { get; }
        public abstract string Name { get; }
        public abstract string Id { get; }
        public abstract int MaxStackSize { get; }
        public abstract Sprite Sprite { get; }

        public IStashingItem<T>.Stash Data { get; private set; }

        public void OnStart(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            Data ??= new IStashingItem<T>.Stash();
            Data.Add(slot, Initiate(entity, inventory, slot));
            Started(entity, inventory, slot);
        }

        public void OnEnded(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
            var c = Data[slot];
            Data.Pop(slot);
            End(entity, inventory, slot, c);
        }

        /// <summary>
        /// нужно для создания нового экземпляра данных
        /// </summary>
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slot">слот, в котором лежит предмет</param>
        /// <returns>новый экземпляр данных</returns>
        protected abstract T Initiate(Entity.Entity entity, IInventory inventory, ISlot slot);

        /// <summary>
        /// эта кароч замена старта, чтоб заново интерфейс не делать
        /// </summary>
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slot">слот, в котором лежит предмет</param>
        protected virtual void Started(Entity.Entity entity, IInventory inventory, ISlot slot)
        {
        }

        /// <summary>
        /// эта кароч замена уничтожения, чтоб заново интерфейс не делать
        /// </summary>
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slot">слот, в котором лежит предмет</param>
        /// <param name="current">данные, которые использовались для этого предмета</param>
        protected virtual void End(Entity.Entity entity, IInventory inventory, ISlot slot, T current)
        {
        }
    }
}