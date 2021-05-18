using FundooRepository.Repo.Notes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooManager.Notes
{
    public class NotesManager : INotesManager
    {
        public readonly INotesRepo repository;
        public NotesManager(INotesRepo notesRepo)
        {
            repository = notesRepo;
        }
    }
}
