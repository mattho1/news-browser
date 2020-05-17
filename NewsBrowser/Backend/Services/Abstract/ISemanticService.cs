using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.Abstract
{
    public interface ISemanticService
    {
        IEnumerable<string> GetBroaderConcepts(string searchQuery);
        IEnumerable<string> GetNarrowerConcepts(string searchQuery);
        IEnumerable<string> GetRelatedConcepts(string searchQuery);
    }
}
