
using FundooModel.Notes;
using FundooRepository.Repo.NotesRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooManager.NotesManager
{
    public class NotesManager : INotesManager
    {
        public readonly INotesRepo repository;
        public NotesManager(INotesRepo notesRepo)
        {
            repository = notesRepo;
        }

        public Task<Notes> AddNotes(Notes notes)
        {
            try
            {
                return this.repository.AddNotes(notes);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<Notes> DeleteNotes(int id)
        {
            try
            {
                return this.repository.DeleteNotes(id);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<Notes> UpdateNotes(Notes notes)
        {
            try
            {
                return this.repository.UpdateNotes(notes);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<IEnumerable<Notes>> GetAllNotes()
        {
            try
            {
                return this.repository.GetAllNotes();
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<IEnumerable<Notes>> GetAllNotesByEmail(string email)
        {
            try
            {
                return this.repository.GetAllNotesByEmail(email);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<Notes> DeleteFromTrash(int id)
        {
            try
            {
                return this.repository.DeleteFromTrash(id);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
