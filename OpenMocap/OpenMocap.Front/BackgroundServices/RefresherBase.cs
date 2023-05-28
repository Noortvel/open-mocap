namespace OpenMocap.Front.BackgroundServices
{
    public abstract class RefresherBase : BackgroundService
    {
        protected readonly IServiceProvider RootProvider;

        protected RefresherBase(IServiceProvider serviceProvider)
        {
            RootProvider = serviceProvider;
        }

        protected virtual TimeSpan SleepTime => TimeSpan.FromSeconds(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = RootProvider.CreateScope();
                var reuslt = await Handle(scope.ServiceProvider, stoppingToken);
                if (reuslt)
                {
                    continue;
                }

                await Task.Delay(SleepTime, stoppingToken);
            }
        }

        protected abstract Task<bool> Handle(
            IServiceProvider provider,
            CancellationToken cancellationToken);
    }
}
