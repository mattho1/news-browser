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
        IEnumerable<string> GetBroaderConcepts(string searchQuery, int n = 10);
        IEnumerable<string> GetNarrowerConcepts(string searchQuery, int n = 10);
        IEnumerable<string> GetRelatedConcepts(string searchQuery, int n = 10);
        bool UseIndexFrequency();
        int GetIndexConceptFrequency(string concept);
    }
}
