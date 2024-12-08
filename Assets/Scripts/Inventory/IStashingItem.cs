using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public interface IStashingItem : IEndableItem, IStartableItem
    {
        /// <summary>
        /// внутренний метод для начального создания хранилища
        /// </summary>
        void InitializeStash();
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

        public class Stash : IEnumerable<KeyValuePair<ISlot.Slotable, T>>
        {
            private readonly ConcurrentDictionary<ISlot.Slotable, T> _dictionary;

            public Stash(ISlot.Slotable slotable, T initial) : this() => Add(slotable, initial);
            public Stash() => _dictionary = new ConcurrentDictionary<ISlot.Slotable, T>();

            /// <summary>
            /// получить данные для предмета
            /// </summary>
            /// <param name="slotable">именно слот используется как ключ к данным предмета</param>
            public T this[ISlot.Slotable slotable] => Get(slotable);

            /// <summary>
            /// получить данные для предмета
            /// </summary>
            /// <param name="slotable">именно слот используется как ключ к данным предмета</param>
            /// <returns>данные о предмете</returns>
            /// <exception cref="ArgumentException">если данные к этому слоту отсутствуют</exception>
            /// <exception cref="NullReferenceException">если слот нулёвый</exception>
            public T Get(ISlot.Slotable slotable)
            {
                if (_dictionary.TryGetValue(slotable, out T value)) return value;
                throw new ArgumentException($"Данные к слоту {slotable} отсутствуют");
            }

            /// <summary>
            /// позволяет убрать данные о предмете
            /// </summary>
            /// <param name="slotable">именно слот используется как ключ к данным предмета</param>
            /// <exception cref="ArgumentException">если данные к этому слоту отсутствуют</exception>
            /// <exception cref="NullReferenceException">если слот нулёвый</exception>
            public void Pop(ISlot.Slotable slotable)
            {
                if (_dictionary.TryRemove(slotable, out _)) return;
                throw new ArgumentException($"Данные к слоту {slotable} отсутствуют");
            }

            /// <summary>
            /// записать данные о предмете
            /// </summary>
            /// <param name="slotable">используется как ключ к данным предмета</param>
            /// <param name="current">данные, которые следует записать</param>
            /// <exception cref="ArgumentException">если данные к этому слоту уже существуют</exception>
            /// <exception cref="NullReferenceException">если слот нулёвый</exception>
            public void Add(ISlot.Slotable slotable, T current)
            {
                if (_dictionary.TryAdd(slotable, current)) return;
                throw new ArgumentException("item in this slot already contained in stash");
            }

            public IEnumerator<KeyValuePair<ISlot.Slotable, T>> GetEnumerator() => _dictionary.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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

        public void OnStart(Entity.Entity entity, IInventory inventory, ISlot.Slotable slotable)
        {
            Data.Add(slotable, Initiate(entity, inventory, slotable));
            Started(entity, inventory, slotable);
        }

        public void OnEnded(Entity.Entity entity, IInventory inventory, ISlot.Slotable slotable)
        {
            var c = Data[slotable];
            Data.Pop(slotable);
            End(entity, inventory, slotable, c);
        }

        public void InitializeStash()
        {
            Data ??= new IStashingItem<T>.Stash();
        }

        /// <summary>
        /// нужно для создания нового экземпляра данных
        /// </summary>
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slotable">слот, в котором лежит предмет</param>
        /// <returns>новый экземпляр данных</returns>
        protected abstract T Initiate(Entity.Entity entity, IInventory inventory, ISlot.Slotable slotable);

        /// <summary>
        /// эта кароч замена старта, чтоб заново интерфейс не делать
        /// </summary>
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slotable">слот, в котором лежит предмет</param>
        protected virtual void Started(Entity.Entity entity, IInventory inventory, ISlot.Slotable slotable)
        {
        }

        /// <summary>
        /// эта кароч замена уничтожения, чтоб заново интерфейс не делать
        /// </summary>
        /// <param name="entity">сущность привязки инвентаря</param>
        /// <param name="inventory">инвентарь, в котором находится предмет</param>
        /// <param name="slotable">слот, в котором лежит предмет</param>
        /// <param name="current">данные, которые использовались для этого предмета</param>
        protected virtual void End(Entity.Entity entity, IInventory inventory, ISlot.Slotable slotable, T current)
        {
        }
    }
}