using System.Linq;
using Cysharp.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace Inventory.ImpactWorldItems
{
    public class GuillotineImpact : MonoBehaviour
    {
        private bool _used;

        public async void Impact(float maxYPosition, float radius, float levitationTime)
        {
            await UniTask.WaitUntil(() => gameObject.transform.position.y < maxYPosition && !_used);
            _used = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            var responsives =
                    Physics2D
                        .OverlapCircleAll(gameObject.transform.position, radius)
                        .Select(a => a.gameObject.GetComponent<GuillotineResponsive>())
                        .Where(a => a is not null && a.Available())
                        .ToArray()
                ;
            foreach (var responsive in responsives)
            {
                responsive.Rigidbody.gravityScale = responsive.NewGravityScale;
            }

            await UniTask.WaitForSeconds(levitationTime);

            foreach (var responsive in responsives)
            {
                responsive.Rigidbody.gravityScale = 1f;
            }

            Destroy(gameObject);
        }
    }
}