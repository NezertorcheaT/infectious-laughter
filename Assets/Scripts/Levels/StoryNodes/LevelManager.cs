namespace Levels.StoryNodes
{
    public class LevelManager
    {
        public int Shop { get; private set; }

        public StoryTree.Node CurrentLevel
        {
            get => _currentLevel;
            private set => _currentLevel = value;
        }

        private StoryTree.Node _currentLevel;
        private readonly StoryTree _tree;

        public LevelManager(StoryTree storyTree, int shop)
        {
            _tree = storyTree;
            Shop = shop;
            Reset();
        }

        public bool HasNextLevelAtMiddle() => string.IsNullOrEmpty(_tree.GetPort2(CurrentLevel.ID));

        public void NextLevelAtMiddle()
        {
            CurrentLevel = _tree.GetState(_tree.GetPort2(CurrentLevel.ID));
        }

        public void NextLevelAtEnd()
        {
            CurrentLevel = _tree.GetState(_tree.GetPort1(CurrentLevel.ID));
        }

        public void SetLevel(string id)
        {
            CurrentLevel = _tree.GetState(id);
        }

        public bool TrySetLevel(string id)
        {
            return _tree.TryGetState(id, ref _currentLevel);
        }

        public void Reset()
        {
            CurrentLevel = _tree.First();
        }
    }
}