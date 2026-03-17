public sealed record LoginResponse(
    string Token,
    string RefreshToken);

public sealed record RefreshTokenResponse(string Token);