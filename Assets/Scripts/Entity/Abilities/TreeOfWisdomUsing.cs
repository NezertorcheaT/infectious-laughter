using PropsImpact;
using UnityEngine;

namespace Entity.Abilities
{
    [AddComponentMenu("Entity/Abilities/Tree of Wisdom Using")]
    public class TreeOfWisdomUsing : Ability
    {
        private bool _detectedTree;
        private TreeOfWisdom _lastSavedTree;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var tree = other.GetComponent<TreeOfWisdom>();
            if (!tree) return;
            _lastSavedTree = tree;
            _detectedTree = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.GetComponent<TreeOfWisdom>()) return;
            _detectedTree = false;
        }

        public bool TryUseTree()
        {
            if (!_detectedTree || !Available()) return false;
            _lastSavedTree.UseTreeOfWisdom();
            return true;
        }

        public void UseTree()
        {
            if (!_detectedTree || !Available()) return;
            _lastSavedTree.UseTreeOfWisdom();
        }
    }
}