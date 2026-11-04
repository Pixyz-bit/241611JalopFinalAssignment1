var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Both GET and POST controllers use the same protected round format.
builder.Services.AddSingleton<_241611JalopFinalAssignment1.Services.ExampleStateTokens>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// The default controller name includes the assignment prefix; public example routes stay explicit.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=JALOPARLANMARTIN_241611_Home}/{action=Index}/{id?}");

app.Run();
