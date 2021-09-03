using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class TodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoDto>> GetAllAsync()
        => (await _context.Todos.ToListAsync())
            .OrderBy(x => x.Id)
            .Select(TodoMappings.ToDto);

    public async Task<TodoDto> GetByIdAsync(string id)
        => TodoMappings.ToDto(await _context.Todos.FirstOrDefaultAsync(x => x.Id == int.Parse(id)));

    public async Task<TodoDto> CreateAsync(TodoDto dto)
    {
        var x = await _context.Todos.AddAsync(TodoMappings.ToModel(dto));
        await _context.SaveChangesAsync();

        return dto;
    }

    public async Task<TodoDto> UpdateAsync(string id, TodoDto dto)
    {
        var todo = await _context.Todos.FindAsync(int.Parse(id));
        todo.Title = dto.Title; todo.Description = dto.Description; todo.IsCompleted = dto.IsCompleted;

        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();

        return dto;
    }

    public async Task RemoveAsync(string id)
    {
        var todo = await _context.Todos.FindAsync(int.Parse(id));

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
    }

    public async Task<TodoDto> MarkAsDoneAsync(string id)
    {
        var todo = await _context.Todos.FindAsync(int.Parse(id)); 
        todo.IsCompleted = true;
        
        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();

        return TodoMappings.ToDto(todo);
    }
}