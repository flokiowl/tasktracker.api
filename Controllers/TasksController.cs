using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Models;
using TaskTracker.Api.Requests;
using TaskTracker.Api.Responses;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    // Тимчасова "БД" в пам'яті
    private static readonly List<TaskItem> Tasks = new()
    {
        new TaskItem { Id = 1, Title = "Вивчити базу C#", Description = "Розібратися з Requests та Responses", IsCompleted = true },
        new TaskItem { Id = 2, Title = "Підключити MS SQL Server", Description = "Налаштувати Entity Framework Core", IsCompleted = false }
    };

    // GET: api/tasks
    [HttpGet]
    public IActionResult GetAll()
    {
        // Перетворюємо внутрішні моделі (TaskItem) у вихідні DTO (TaskResponse)
        var response = Tasks.Select(t => new TaskResponse(
            t.Id,
            t.Title,
            t.Description,
            t.IsCompleted,
            t.CreatedAt
        ));

        return Ok(response);
    }

    // GET: api/tasks/1
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound(new { message = $"Задачу з ID {id} не знайдено" });

        var response = new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.CreatedAt
        );

        return Ok(response);
    }

    // POST: api/tasks
    [HttpPost]
    public IActionResult Create([FromBody] CreateTaskRequest request)
    {
        var newTask = new TaskItem
        {
            Id = Tasks.Max(t => t.Id) + 1,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false
        };

        Tasks.Add(newTask);

        var response = new TaskResponse(
            newTask.Id,
            newTask.Title,
            newTask.Description,
            newTask.IsCompleted,
            newTask.CreatedAt
        );

        return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, response);
    }

    // PUT: api/tasks/1
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] UpdateTaskRequest request)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            return NotFound();

        task.Title = request.Title;
        task.Description = request.Description;
        task.IsCompleted = request.IsCompleted;

        return NoContent(); // HTTP 204
    }

    // DELETE: api/tasks/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            return NotFound();

        Tasks.Remove(task);
        return NoContent();
    }
}