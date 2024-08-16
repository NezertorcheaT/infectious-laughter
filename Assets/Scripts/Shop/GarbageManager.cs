using System;

namespace Shop
{
    public class GarbageManager
    {
        public event EventHandler OnBalanceChanged;

        private int _garbageBalance = 100;
        
        public int GarbageBalance
        {
            get => _garbageBalance;
            set
            {
                _garbageBalance = value;
                OnBalanceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public GarbageManager(int garbageBalance)
        {
            GarbageBalance = garbageBalance;
        }

        public bool IfCanAfford(int garbageAmount) => _garbageBalance >= garbageAmount;
    }
}