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
            .Select(TodoMappings.ToDto);

    public async Task<TodoDto> CreateAsync(TodoDto dto)
    {
        var x = await _context.Todos.AddAsync(TodoMappings.ToModel(dto));
        await _context.SaveChangesAsync();

        return dto; 
    }

    public async Task<TodoDto> GetByIdAsync(int id)
        => TodoMappings.ToDto(await _context.Todos.FirstOrDefaultAsync(x => x.Id == id));

}