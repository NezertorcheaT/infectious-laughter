using System;
using System.Threading.Tasks;

namespace Commands
{
    public interface ICommand
    {
    }

    public interface IAsyncRepeatableCommand : IRepeatableCommand
    {
        public Task Repeat { get; }
    }

    public interface IAsyncRevertableCommand : IRevertableCommand
    {
        public Task Revert { get; }
    }

    public interface ISyncRepeatableCommand : IRepeatableCommand
    {
        public Action Repeat { get; }
    }

    public interface ISyncRevertableCommand : IRevertableCommand
    {
        public Action Revert { get; }
    }

    public interface IRepeatableCommand : ICommand
    {
    }

    public interface IRevertableCommand : ICommand
    {
    }
}