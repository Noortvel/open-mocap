using OpenMocap;
using OpenMocap.BackgroundServices;
using OpenMocap.CoreServices;
using OpenMocap.CoreServices.Services;
using OpenMocap.ML;
using OpenMocap.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSingleton<IAddressProvider, AddressProvider>();

builder.Services
    .AddMLServices(builder.Configuration.GetSection(nameof(MlOptions)).Bind)
    .AddCoreServices()
    .AddSingleton<CallbacksRepository>()
    .AddSingleton<MocapJobsQueue>()
    .AddSingleton<SplitedEventsQueue>()
    ;

builder.Services.Configure<AddressesOptions>(
    builder.Configuration.GetSection(AddressesOptions.Section));

builder.Services
    .AddHostedService<MocapAsyncRunnner>()
    .AddHostedService<SplitedEventsHandler>()
    ;

builder.Services.AddHttpClient();

var app = builder.Build();
//app.UseHttpLogging();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.AddHttpHandlers();

app.Run();
