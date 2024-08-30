using Demo1.Service;
using ITI_Attendance.Models;
using ITI_Attendance.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/LogOut";
    });
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});
string con = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<ITIDbContext>(a =>
{
    a.UseSqlServer(con);
});
builder.Services.AddScoped<IUserServices,UserServices>();
builder.Services.AddScoped<IHrService, HrService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentAttendeceService, StudentAttendeceService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IStudentProgramIntakeService, StudentProgramIntakeService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
