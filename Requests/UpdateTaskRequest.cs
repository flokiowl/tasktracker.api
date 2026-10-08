namespace TaskTracker.Api.Requests;

public record UpdateTaskRequest(
    string Title,

    string Description,

    bool IsCompleted
);