using System.Text.Json;
using mangas.Domain.Entities;

namespace mangas.Infrastructure.Repositories;

public class MangaRepository
{
    private List<Manga> _mangas;

    private string _filePath;

    public MangaRepository(IConfiguration configuration)
    {
       _filePath = configuration.GetValue<string>("dataBank") ?? string.Empty;
        _mangas = LoadData();
    }

    private string GetCurrentFilePath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var CurrentFilePath = Path.Combine(currentDirectory, _filePath);

        return CurrentFilePath;
    }

    private List<Manga> LoadData()
    {
        var CurrentFilePath = GetCurrentFilePath();

        if (File.Exists(CurrentFilePath))
        {
            var jsonData = File.ReadAllText(CurrentFilePath);
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return JsonSerializer.Deserialize<List<Manga>>(jsonData);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        return new List<Manga>();
    }

    public IEnumerable<Manga> GetAll()
    {
        return _mangas;
    }

    public Manga GetById(int id)
    {
        var manga = _mangas.FirstOrDefault(manga => manga.Id == id);
        return manga ?? new Manga
        {
            Title = string.Empty,
            Author = string.Empty,
        };
    }



    public void Add(Manga manga)
    {
        var CurrentFilePath = GetCurrentFilePath();
        if (!File.Exists(CurrentFilePath))
            return;
        _mangas.Add(manga);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_mangas));
    }

    public void Update(Manga UpdatedManga)
    {
        var CurrentFilePath = GetCurrentFilePath();
        if (!File.Exists(CurrentFilePath))
            return;


        var index = _mangas.FindIndex(m => m.Id == UpdatedManga.Id);

        if (index != -1)
        {
            _mangas[index] = UpdatedManga;
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_mangas));
        }
    }

    public void Delete(int id)
    {
        var CurrentFilePath = GetCurrentFilePath();
        if (!File.Exists(CurrentFilePath))
            return;

        _mangas.RemoveAll(m => m.Id == id);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_mangas));
    }


}