using Testcontainers.MsSql;

namespace Integration.Tests.Library.TestContainers;

public sealed class ContainerizedSqlServerDatabase : IAsyncDisposable
{
    public string ConnectionString => _connectionString
        ?? throw new InvalidOperationException(
            "Start the container first to get the connection string.");

    private string? _connectionString;

    private readonly MsSqlContainer _msSqlContainer;

    public ContainerizedSqlServerDatabase()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04")
            .Build();
    }

    public async Task Start(CancellationToken cancellation = default)
    {
        await _msSqlContainer.StartAsync(cancellation);

        _connectionString = _msSqlContainer.GetConnectionString();
    }

    public async ValueTask DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }
}