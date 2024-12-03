using System;
using System.Threading.Tasks;

namespace Commands
{
    public static class Command
    {
        private class GeneralCommand : ICommand
        {
            public Action Action;
            public Action ActionRevert;
            public Task Task;
            public Task TaskRevert;

            public GeneralCommand(Action action, Action revert)
            {
                Action = action;
                ActionRevert = revert;
            }

            public GeneralCommand(Task task, Task revert)
            {
                Task = task;
                TaskRevert = revert;
            }

            public GeneralCommand(Task task, Action revert)
            {
                Task = task;
                ActionRevert = revert;
            }

            public GeneralCommand(Action action, Task revert)
            {
                Action = action;
                TaskRevert = revert;
            }

            public async Task Repeat()
            {
                await Task;
                Action?.Invoke();
            }

            public async Task Revert()
            {
                await TaskRevert;
                ActionRevert?.Invoke();
            }
        }

        public static ICommand FromAction(Action action) => new GeneralCommand(action, delegate { });
        public static ICommand FromTask(Task task) => new GeneralCommand(task, delegate { });

        public static ICommand RepeatsWith(this ICommand command, Action action)
        {
            return new GeneralCommand(
                Action(),
                command.Revert()
            );

            async Task Action()
            {
                await command.Repeat();
                action?.Invoke();
            }
        }

        public static ICommand RepeatsWith(this ICommand command, Task task)
        {
            return new GeneralCommand(
                Action(),
                command.Revert()
            );

            async Task Action()
            {
                await command.Repeat();
                await task;
            }
        }

        public static ICommand RevertsWith(this ICommand command, Task task)
        {
            return new GeneralCommand(
                command.Repeat(),
                Revert()
            );

            async Task Revert()
            {
                await command.Revert();
                await task;
            }
        }

        public static ICommand RevertsWith(this ICommand command, Action action)
        {
            return new GeneralCommand(
                command.Repeat(),
                Revert()
            );

            async Task Revert()
            {
                await command.Revert();
                action?.Invoke();
            }
        }
    }
}