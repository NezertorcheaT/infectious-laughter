using System.IO;
using Saving;
using UnityEngine;
using Zenject;

namespace GameFlow
{
    public class SaveLoaderMenuUI : MonoBehaviour
    {
        [SerializeField] private SaveLoaderButtonUI buttonPrefab;
        [SerializeField] private Transform spawnPoint;
        [Inject] private SessionFactory sessionFactory;
        [Inject] private MenuSaveLoader saveLoader;

        private void Start()
        {
            foreach (var sessionID in sessionFactory.GetAvailableSessionIDs())
            {
                var button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, spawnPoint);
                button.Enter.onClick.AddListener(() => { saveLoader.LoadSave(sessionID); });
                button.Remove.onClick.AddListener(() =>
                {
                    File.Delete(SessionFileSaver.CreatePath(sessionID));
                    Destroy(button.gameObject);
                });
                button.TextMesh.SetText(sessionID);
            }
        }
    }
}