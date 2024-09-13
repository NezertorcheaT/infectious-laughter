using System.Threading.Tasks;
using UnityEngine;

namespace PropsImpact
{
    public class RadioBehaviour : MonoBehaviour
    {
        [SerializeField] private float lifeTimeInSeconds;

        private async void Start()
        {
            StartCooldown(lifeTimeInSeconds * 1000);
        }

        private async void StartCooldown(float lifeTimeInMiliseconds)
        {
            await Task.Delay((int)lifeTimeInMiliseconds);
            Destroy(gameObject);
        }
    }
}

