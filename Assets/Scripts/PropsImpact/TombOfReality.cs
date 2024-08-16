using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Installers;
using UnityEngine.SceneManagement;

namespace PropsImpact
{
    public class TombOfReality : MonoBehaviour
    {
        private string _searchFruitID = "il.fruit_of_the_tree";
        private byte _usedFruitsCount = 0;
        private List<Inventory.ISlot> _slots;
        
        [Inject] private PlayerInstallation _playerInstallation;
        [SerializeField] private int sceneId;

        public void Use()
        {
            _slots = _playerInstallation.Inventory.Slots;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].IsEmpty) continue;
                if(!_slots[i].LastItem.Id.Contains(_searchFruitID)) continue;
                _slots[i].Count--; // Честно я хз почему, но предмет отнимается но в UI Не отображается (обновляется когда поднимаешь какой нибудь предмет)
                _usedFruitsCount++;

                if(_usedFruitsCount >= 3)
                {
                    SceneManager.LoadScene(sceneId);
                }
            }
        }
    }
}