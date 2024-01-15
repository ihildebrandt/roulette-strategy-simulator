namespace RouletteStrategySimulator;
public class RouletteGame : IGame
{
    private class PlayerWagers
    {
        private readonly IEnumerable<Wager> _wagers;
        private readonly TaskCompletionSource<WagerResult> _taskCompletionSource = new();

        public Task<WagerResult> Task => _taskCompletionSource.Task;

        public PlayerWagers(IEnumerable<Wager> wagers) =>
            _wagers = wagers;

        public void Resolve(Number hit)
        {
            foreach (var wager in _wagers)
            {
                wager.Resolve(hit);
            }

            _taskCompletionSource.SetResult(new WagerResult(hit, _wagers));
        }

    }

    private readonly IWheel _wheel;

    private readonly IList<PlayerWagers> _turnWagers = new List<PlayerWagers>();

    public RouletteGame(IWheel wheel) => _wheel = wheel;

    public Task<WagerResult> PlaceBets(IEnumerable<Wager> wagers)
    {
        var playerWagers = new PlayerWagers(wagers);
        _turnWagers.Add(playerWagers);
        return playerWagers.Task;
    }

    public void Turn()
    {
        var hit = _wheel.Spin();

        foreach (var playerWagers in _turnWagers)
        {
            playerWagers.Resolve(hit);
        }

        _turnWagers.Clear();
    }
}
