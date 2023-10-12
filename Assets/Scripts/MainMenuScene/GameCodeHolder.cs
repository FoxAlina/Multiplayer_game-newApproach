public enum PlayerType
{
    HOST, CLIENT
}

public static class GameCodeHolder
{
    private static string _gameCode;
    public static string gameCode { get => _gameCode; set => _gameCode = value; }

    private static PlayerType _playerType;
    public static PlayerType playerType { get => _playerType; set => _playerType = value; }
}
