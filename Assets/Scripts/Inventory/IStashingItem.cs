using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

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

        void IStartableItem.OnStart(Entity.Entity entity, IInventory inventory, Slotable slotable)
        {
            Data.Add(slotable, Initiate(entity, inventory, slotable));
            Started(entity, inventory, slotable);
        }

        void IEndableItem.OnEnded(Entity.Entity entity, IInventory inventory, Slotable slotable)
        {
            var c = Data[slotable];
            Data.Pop(slotable);
            End(entity, inventory, slotable, c);
        }

        T Initiate(Entity.Entity entity, IInventory inventory, Slotable slotable);
        void Started(Entity.Entity entity, IInventory inventory, Slotable slotable);
        void End(Entity.Entity entity, IInventory inventory, Slotable slotable, T c);

        /// <summary>
        /// структура данных, скрывающая данные каждого предмета
        /// </summary>
        public class Stash : IEnumerable<KeyValuePair<Slotable, T>>
        {
            private readonly ConcurrentDictionary<Slotable, T> _dictionary;

            public Stash(Slotable slotable, T initial) : this() => Add(slotable, initial);
            public Stash() => _dictionary = new ConcurrentDictionary<Slotable, T>();

            /// <summary>
            /// получить данные для предмета
            /// </summary>
            /// <param name="slotable">именно слот используется как ключ к данным предмета</param>
            public T this[Slotable slotable] => Get(slotable);

            /// <summary>
            /// получить данные для предмета
            /// </summary>
            /// <param name="slotable">именно слот используется как ключ к данным предмета</param>
            /// <returns>данные о предмете</returns>
            /// <exception cref="ArgumentException">если данные к этому слоту отсутствуют</exception>
            /// <exception cref="NullReferenceException">если слот нулёвый</exception>
            public T Get(Slotable slotable)
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
            public void Pop(Slotable slotable)
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
            public void Add(Slotable slotable, T current)
            {
                Debug.Log(slotable);
                if (_dictionary.TryAdd(slotable, current)) return;
                throw new ArgumentException("item in this slot already contained in stash");
            }

            public IEnumerator<KeyValuePair<Slotable, T>> GetEnumerator() => _dictionary.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}