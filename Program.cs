using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadioStationScheduler.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database connection (SQLite example)
builder.Services.AddDbContext<RadioStationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ================== ENDPOINTS ==================

// Get today's schedule
app.MapGet("/schedule/today", async (RadioStationContext db) =>
{
    var today = DateTime.Today;
    return await db.ScheduledEvents
        .Where(e => e.StartTime.Date == today)
        .ToListAsync();
});

// Get next 7 days
app.MapGet("/schedule/next7days", async (RadioStationContext db) =>
{
    var today = DateTime.Today;
    var next7 = today.AddDays(6);
    return await db.ScheduledEvents
        .Where(e => e.StartTime.Date >= today && e.StartTime.Date <= next7)
        .ToListAsync();
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

// Reschedule event
app.MapPut("/schedule/{id}/reschedule", async ([FromRoute] int id, RescheduleRequest request, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    ev.StartTime = request.NewStartTime;
    ev.EndTime = request.NewEndTime;
    await db.SaveChangesAsync();

    return Results.Ok(ev);
});

// Add host
app.MapPut("/schedule/{id}/addHost", async ([FromRoute] int id, HostRequest request, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    if (!ev.Hosts.Contains(request.Host))
        ev.Hosts.Add(request.Host);

    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Remove host
app.MapPut("/schedule/{id}/removeHost", async ([FromRoute] int id, HostRequest request, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    ev.Hosts.Remove(request.Host);
    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Add guest
app.MapPut("/schedule/{id}/addGuest", async ([FromRoute] int id, GuestRequest request, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    if (!ev.Guests.Contains(request.Guest))
        ev.Guests.Add(request.Guest);

    await db.SaveChangesAsync();
    return Results.Ok(ev);
});

// Remove guest
app.MapPut("/schedule/{id}/removeGuest", async ([FromRoute] int id, GuestRequest request, RadioStationContext db) =>
{
    var ev = await db.ScheduledEvents.FindAsync(id);
    if (ev is null) return Results.NotFound();

    ev.Guests.Remove(request.Guest);
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

app.Run();
