using System;
using UnityEngine;
using Zenject;

namespace Saving
{
    public class SessionTester : MonoBehaviour
    {
        [Inject] private SessionCreator _sessionCreator;
        [Inject] private Config _config;

        private void Start()
        {
            var ses = _sessionCreator.NewSession();
            ses.Add(new Vector2(0, 1), "asss");
            ses.Add("amogus", "random string");
            _sessionCreator.SaveCurrentSession();

            _sessionCreator.LoadSession("0");
            foreach (var (key, content) in _sessionCreator.Current)
            {
                Debug.Log(key);
                Debug.Log(content.Value);
                Debug.Log(content.Value.GetType());
                Debug.Log(content.Type);
            }

            Debug.Log(_config.Volume);
        }
    }
}