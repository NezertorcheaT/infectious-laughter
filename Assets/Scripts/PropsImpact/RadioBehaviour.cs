using System.Threading.Tasks;
using UnityEngine;

namespace PropsImpact
{
    public class RadioBehaviour : MonoBehaviour
    {
        [SerializeField] private float lifeTimeInSeconds;

        private async void Start()
        {
            await Task.Delay((int)(lifeTimeInSeconds * 1000));
            Destroy(gameObject);
        }
    }
}