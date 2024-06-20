﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saving
{
    [Serializable]
    public class Config : IFileSaver<string>.ISavable<string>
    {
        public string Convert => JsonSerializer.Serialize(this);

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                _saver.Save(this);
            }
        }

        private float _volume = 1;

        private IFileSaver<string> _saver;

        [JsonConstructor]
        private Config(float volume)
        {
            _volume = volume;
        }

        public Config(ConfigFileSaver saver)
        {
            _saver = saver;
        }
    }
}