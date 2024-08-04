using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    public class TreeOfWisdomUseAbility : Ability
    {
        private bool _detectedTree;
        private TreeOfWisdomEntity _lastSavedTree;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.GetComponent<TreeOfWisdomEntity>()) return;
            _lastSavedTree = other.GetComponent<TreeOfWisdomEntity>();
            _detectedTree = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!other.GetComponent<TreeOfWisdomEntity>()) return;
            _detectedTree = false;
        }

        public void UseTree()
        {
            if(!_detectedTree) return;
            _lastSavedTree.UseTreeOfWisdom();
        }
    }
}
