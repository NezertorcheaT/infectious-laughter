using System.Threading.Tasks;

namespace Commands
{
    public interface ICommand
    {
        public Task Repeat();
        public Task Revert();
    }
}