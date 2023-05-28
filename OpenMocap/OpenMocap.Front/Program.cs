using OpenMocap.Front;
using OpenMocap.Front.BackgroundServices;
using OpenMocap.Front.Services;
using OpenMocap.Front.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services
    .AddSingleton<OperationConnectionStorage>()
    .AddSingleton<MocapResultsStorage>();

builder.Services.AddHttpClient<OpenMocapClient>();
builder.Services
    .AddSingleton<OpenMocapApiProvider>()
    .AddSingleton<UrlProvider>()
    .AddSingleton<OpenMocapClientBuilder>();

// SignalR
builder.Services.AddScoped<VideoHubClients>();

builder.Services
    .AddHostedService<Warmup>()
    .AddHostedService<MetricsRefresher>()
    .AddHostedService<MocapResultSender>()
    ;

builder.Services.Configure<AddressesOptions>(
    builder.Configuration.GetSection(AddressesOptions.Section));


var app = builder.Build();
//app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<VideoHub>(VideoHub.Url);

OpenMocap.Front.HttpHandlers.ResultReciver.Map(app);
OpenMocap.Front.HttpHandlers.SendToWorker.Map(app);

app.Run();
