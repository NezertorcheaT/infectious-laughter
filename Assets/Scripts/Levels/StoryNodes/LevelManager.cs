namespace Levels.StoryNodes
{
    public class LevelManager
    {
        public int Shop { get; private set; }

        public int LevelsPassCount { get; private set; }

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
            LevelsPassCount++;
            CurrentLevel = _tree.GetState(_tree.GetPort2(CurrentLevel.ID));
        }

        public void NextLevelAtEnd()
        {
            LevelsPassCount++;
            CurrentLevel = _tree.GetState(_tree.GetPort1(CurrentLevel.ID));
        }

        public void SetLevel(string id)
        {
            LevelsPassCount++;
            CurrentLevel = _tree.GetState(id);
        }

        public bool TrySetLevel(string id)
        {
            return _tree.TryGetState(id, out _currentLevel);
        }

        public void Reset()
        {
            LevelsPassCount = 0;
            CurrentLevel = _tree.First();
        }
    }
}