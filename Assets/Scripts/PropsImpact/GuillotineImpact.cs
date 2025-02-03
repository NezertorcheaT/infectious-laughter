using System.Linq;
using Cysharp.Threading.Tasks;
using Entity.Abilities;
using UnityEngine;

namespace PropsImpact
{
    public class GuillotineImpact : MonoBehaviour
    {
        private bool _used;

        public async void Impact(float maxYPosition, float radius, float levitationTime)
        {
            await UniTask.WaitUntil(() => gameObject.transform.position.y < maxYPosition && !_used);
            _used = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            (GuillotineResponsive Instance, bool Enabled)[] responsives =
                    Physics2D
                        .OverlapCircleAll(gameObject.transform.position, radius)
                        .Select(a => a.gameObject.GetComponent<GuillotineResponsive>())
                        .Where(a => a is not null && a.Available())
                        .Select(i => (i, i.Movement.enabled))
                        .ToArray()
                ;
            foreach (var responsive in responsives)
            {
                responsive.Instance.Rigidbody.gravityScale = responsive.Instance.NewGravityScale;
                responsive.Instance.Rigidbody.velocity = new Vector2(0f, 0f);
                responsive.Instance.Movement.enabled = false;
            }

            await UniTask.WaitForSeconds(levitationTime);

            foreach (var responsive in responsives)
            {
                responsive.Instance.Rigidbody.gravityScale = 1f;
                responsive.Instance.Movement.enabled = responsive.Enabled;
            }

            Destroy(gameObject);
        }
    }
}