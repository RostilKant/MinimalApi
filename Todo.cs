public record Todo
{
    public int Id { get; set; }
    public string? Title { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public bool? IsCompleted { get; set; } = null!;
}

public record TodoDto(string? Title, string? Description, bool? IsCompleted);


public static class TodoMappings
{
    public static Todo ToModel(TodoDto dto) 
        => new Todo 
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        };

        public static TodoDto ToDto(Todo? model) 
            => new TodoDto(model?.Title, model?.Description, model?.IsCompleted);
}