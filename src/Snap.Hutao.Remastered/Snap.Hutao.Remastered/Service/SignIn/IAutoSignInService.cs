namespace Snap.Hutao.Remastered.Service.SignIn;

public interface IAutoSignInService
{
    ValueTask InitializeAsync(CancellationToken token = default);

    ValueTask RunOnceAsync(CancellationToken token = default);

    bool IsEnabled { get; set; }
}
