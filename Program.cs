using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadioStationScheduler.Data;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Services
// =======================

// Enable CORS for React frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<RadioStationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// =======================
// Middleware
// =======================

// CORS MUST be first
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Remove HTTPS redirect because React runs on HTTP
// app.UseHttpsRedirection();

// =======================
// Endpoints
// =======================

// Get today's schedule
app.MapGet("/schedule/today", async (RadioStationContext db) =>
{
    var today = DateTime.Today;

    var results = await db.ScheduledEvents
        .Where(e => e.StartTime.Date == today)
        .ToListAsync();

    return Results.Ok(results);
});

// Get next 7 days
app.MapGet("/schedule/next7days", async (RadioStationContext db) =>
{
    var today = DateTime.Today;
    var next = today.AddDays(6);

    var results = await db.ScheduledEvents
        .Where(e => e.StartTime.Date >= today && e.StartTime.Date <= next)
        .ToListAsync();

    return Results.Ok(results);
});

// Get event by ID
app.MapGet("/schedule/{id}", async ([FromRoute] int id, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    return ev is not null ? Results.Ok(ev) : Results.NotFound();
});

// Create new event
app.MapPost("/schedule", async (ScheduledEvent newEvent, RadioStationContext db) =>
{
    if (string.IsNullOrWhiteSpace(newEvent.Title) || newEvent.EndTime <= newEvent.StartTime)
        return Results.BadRequest("Invalid event data.");

    db.ScheduledEvents.Add(newEvent);
    await db.SaveChangesAsync();

    return Results.Created($"/schedule/{newEvent.Id}", newEvent);
});

// Update event time
app.MapPut("/schedule/{id}/reschedule", async ([FromRoute] int id, RescheduleRequest req, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    ev.StartTime = req.NewStartTime;
    ev.EndTime = req.NewEndTime;

    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Add host
app.MapPut("/schedule/{id}/addHost", async ([FromRoute] int id, HostRequest req, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    if (!ev.Hosts.Contains(req.Host))
        ev.Hosts.Add(req.Host);

    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Remove host
app.MapPut("/schedule/{id}/removeHost", async ([FromRoute] int id, HostRequest req, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    ev.Hosts.Remove(req.Host);

    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Add guest
app.MapPut("/schedule/{id}/addGuest", async ([FromRoute] int id, GuestRequest req, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    if (!ev.Guests.Contains(req.Guest))
        ev.Guests.Add(req.Guest);

    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Remove guest
app.MapPut("/schedule/{id}/removeGuest", async ([FromRoute] int id, GuestRequest req, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    ev.Guests.Remove(req.Guest);

    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Delete event
app.MapDelete("/schedule/{id}", async ([FromRoute] int id, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    db.ScheduledEvents.Remove(ev);
    await db.SaveChangesAsync();

    return Results.NoContent();
});
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RadioStationContext>();
    if (!db.ScheduledEvents.Any())
    {
        db.ScheduledEvents.Add(new ScheduledEvent
        {
            Title = "Morning Mix",
            StartTime = DateTime.Today.AddHours(10),
            EndTime = DateTime.Today.AddHours(11),
            Hosts = new List<string> { "DJ Test" },
            Guests = new List<string> { "Guest A" }
        });
        db.SaveChanges();
    }
}

// =======================
// Run app
// =======================
app.Run();


