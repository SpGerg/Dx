using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dx.AdminUtilities.Features.Admin.Interfaces;
using Newtonsoft.Json;

namespace Dx.AdminUtilities.Features.Admin;

public class AdminRepository : IAdminRepository
{
    public AdminRepository(string filePath)
    {
        _filePath = filePath;
    }
    
    public IEnumerable<AdminProfile> Entities => _entities;
    
    private readonly string _filePath;

    private List<AdminProfile> _entities = new();
    
    public void Add(AdminProfile entity)
    {
        _entities.Add(entity);
    }

    public AdminProfile Get(AdminProfile entity)
    {
        var profile = _entities.FirstOrDefault(profile => profile.UserId == entity.UserId);

        if (profile is null)
        {
            return new AdminProfile
            {
                Username = entity.Username,
                UserId = entity.UserId,
                ModeratedTime = new DateTime()
            };
        }

        return profile;
    }

    public void Update(AdminProfile entity)
    {
        var index = _entities.FindIndex(profile => profile.UserId == entity.UserId);

        if (index < 0)
        {
            _entities.Add(entity);
            
            return;
        }
        
        _entities[index] = entity;
    }

    public bool Remove(AdminProfile entity)
    {
        return _entities.Remove(entity);
    }

    public void Load()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }
        
        _entities = JsonConvert.DeserializeObject<List<AdminProfile>>(File.ReadAllText(_filePath));
    }

    public void Clear()
    {
        _entities.Clear();
    }

    public void Save()
    {
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(_entities));
    }
}