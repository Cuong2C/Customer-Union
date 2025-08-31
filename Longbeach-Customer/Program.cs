var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWhen(context =>
{
    var isClientSecretsPost = context.Request.Path.StartsWithSegments("/api/v1/longbeach/auth/client-secrets");
    var isClientSource = context.Request.Path.StartsWithSegments("/api/v1/longbeach/auth/client-sources") && context.Request.Method != "GET";

    return isClientSecretsPost || isClientSource;
}, appBuilder =>
{
    appBuilder.UseMiddleware<IPWhiteListMiddleware>(builder.Configuration["IPWhiteList"]);
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapLongbeachApi();
app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }


