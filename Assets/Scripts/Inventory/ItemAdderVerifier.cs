using Zenject;

namespace Inventory
{
    public class ItemAdderVerifier
    {
        public DiContainer Container { get; private set; }

        public ItemAdderVerifier(DiContainer container)
        {
            Container = container;
        }

        public void Verify(IItemAdder itemAdder)
        {
            Container.Inject(itemAdder);
        }
    }
}