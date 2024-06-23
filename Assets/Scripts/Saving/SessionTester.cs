using System;
using UnityEngine;
using Zenject;

namespace Saving
{
    public class SessionTester : MonoBehaviour
    {
        //эту штуку инжектить, чтоб был доступ к сохранениям
        [Inject] private SessionCreator _sessionCreator;

        //ахуеть конфиг
        [Inject] private Config _config;

        private void Start()
        {
            /*
            //Читаем тестовое поле с конфига
            Debug.Log(_config.Volume);
            
            //Это путь, по которому лежат сохранения
            Debug.Log(SessionFileSaver.CreatePath("0"));

            //здесь типа мы создаём новую игру
            var ses = _sessionCreator.NewSession();

            //здесь записываем в сохранение данные, чтоб типа сохранить их
            ses.Add(new Vector2(0, 1), "asss");
            ses.Add("amogus", "random string");
            ses.Add(new[] {"a0", "b2"}, "pen");

            //Сохранит созданную сессию на диске, после тестов почистите папку сохранений
            _sessionCreator.SaveCurrentSession();

            //Прочитаеет сессию 0.session с диска, у вас её может не быть
            _sessionCreator.LoadSession("0");
            foreach (var (key, content) in _sessionCreator.Current)
            {
                Debug.Log(key);
                Debug.Log(content.Value);
                Debug.Log(content.Value.GetType());
                Debug.Log(content.Type);
            }
            */
        }
    }
}