namespace TaskTracker.Api.Responses;

public record TaskResponse(
    int Id,

    string Title,

    string Description,

    bool IsCompleted,

    DateTime CreatedAt
);