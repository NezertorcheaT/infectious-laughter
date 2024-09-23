using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace Commands
{
    public class History : IEnumerable<ICommand>, IDisposable
    {
        private List<(TimeSpan delay, ICommand command)> _history;
        private Stopwatch _timer;
        public bool Stopped { get; private set; }

        public static History Create()
        {
            var h = new History();
            h._history = new List<(TimeSpan delay, ICommand command)>();
            h._timer = new Stopwatch();
            return h;
        }

        public static History CreateStarted()
        {
            var h = Create();
            h._timer.Start();
            return h;
        }

        public History StartWatch()
        {
            if (!_timer.IsRunning)
                _timer.Start();
            return this;
        }

        public History Register<T>(T command) where T : ICommand
        {
            if (Stopped) return this;
            if (!_timer.IsRunning)
                _timer.Start();
            _history.Add((_timer.Elapsed, command));

            return this;
        }

        public async Task Revert()
        {
            foreach (var (delay, command) in _history)
            {
                if (command is IRevertableCommand) continue;
                Debug.LogError($"Cannot revert unrevertable command {command}, consider using RevertForced()");
                return;
            }

            await RevertForced();
        }

        public async Task Reapply()
        {
            foreach (var (delay, command) in _history)
            {
                await Task.Delay(delay);
                if (command is not IRepeatableCommand) continue;
                if (command is ISyncRepeatableCommand rev) rev.Repeat();
                if (command is IAsyncRepeatableCommand arev) await arev.Repeat;
            }
        }

        public async Task RevertForced()
        {
            foreach (var (delay, command) in _history)
            {
                await Task.Delay(delay);
                if (command is not IRevertableCommand) continue;
                if (command is ISyncRevertableCommand rev) rev.Revert();
                if (command is IAsyncRevertableCommand arev) await arev.Revert;
            }
        }

        public IEnumerator<ICommand> GetEnumerator() => _history.Select(a => a.command).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            Stopped = true;
            if (_timer.IsRunning)
                _timer.Stop();
        }
    }
}