using System;
using System.Threading.Tasks;

namespace Commands
{
    public static class Command
    {
        private class GeneralCommand : ICommand
        {
        }

        private class AsyncRepeatableGeneralCommand : IAsyncRepeatableCommand
        {
            public Task Repeat { get; set; }
        }

        private class AsyncRevertableGeneralCommand : IAsyncRevertableCommand
        {
            public Task Revert { get; set; }
        }

        private class AsyncBothGeneralCommand : IAsyncRevertableCommand, IAsyncRepeatableCommand
        {
            public Task Revert { get; set; }
            public Task Repeat { get; set; }
        }

        private class SyncBothGeneralCommand : ISyncRevertableCommand, ISyncRepeatableCommand
        {
            public Action Revert { get; set; }
            public Action Repeat { get; set; }
        }


        private class AsyncSyncGeneralCommand : IAsyncRevertableCommand, ISyncRepeatableCommand
        {
            public Task Revert { get; set; }
            public Action Repeat { get; set; }
        }

        private class SyncAsyncGeneralCommand : ISyncRevertableCommand, IAsyncRepeatableCommand
        {
            public Action Revert { get; set; }
            public Task Repeat { get; set; }
        }


        private class SyncRepeatableGeneralCommand : ISyncRepeatableCommand
        {
            public Action Repeat { get; set; }
        }

        private class SyncRevertableGeneralCommand : ISyncRevertableCommand
        {
            public Action Revert { get; set; }
        }

        public static IRepeatableCommand FromAction(Action action)
        {
            var com = new SyncRepeatableGeneralCommand();
            com.Repeat = action;
            return com;
        }

        public static IRepeatableCommand FromTask(Task task)
        {
            var com = new AsyncRepeatableGeneralCommand();
            com.Repeat = task;
            return com;
        }


        public static IRevertableCommand RevertingBy(Action action)
        {
            var com = new SyncRevertableGeneralCommand();
            com.Revert = action;
            return com;
        }

        public static IRevertableCommand RevertingBy(Task task)
        {
            var com = new AsyncRevertableGeneralCommand();
            com.Revert = task;
            return com;
        }

        public static ICommand RevertingBy(this ICommand command, Task task)
        {
            if (command is IAsyncRepeatableCommand rep1)
            {
                var com1 = new AsyncBothGeneralCommand();
                com1.Repeat = rep1.Repeat;
                if (command is IAsyncRevertableCommand rev1)
                    com1.Revert = Task.WhenAll(rev1.Revert, task);
                else
                    com1.Revert = task;
                return com1;
            }

            if (command is ISyncRepeatableCommand rep2)
            {
                var com2 = new AsyncSyncGeneralCommand();
                com2.Repeat = rep2.Repeat;
                if (command is IAsyncRevertableCommand rev2)
                    com2.Revert = Task.WhenAll(rev2.Revert, task);
                else
                    com2.Revert = task;
                return com2;
            }

            var com = new AsyncRevertableGeneralCommand();
            if (command is IAsyncRevertableCommand rev)
                com.Revert = Task.WhenAll(rev.Revert, task);
            else
                com.Revert = task;
            return com;
        }

        public static ICommand RevertingBy(this ICommand command, Action action)
        {
            if (command is IAsyncRepeatableCommand rep1)
            {
                var com1 = new SyncAsyncGeneralCommand();
                com1.Repeat = rep1.Repeat;
                com1.Revert = action;
                return com1;
            }

            if (command is ISyncRepeatableCommand rep2)
            {
                var com2 = new SyncBothGeneralCommand();
                com2.Repeat = rep2.Repeat;
                com2.Revert = action;
                return com2;
            }

            var com = new SyncRevertableGeneralCommand();
            com.Revert = action;
            return com;
        }
    }
}