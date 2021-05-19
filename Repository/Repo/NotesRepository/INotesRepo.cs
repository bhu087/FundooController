using System;
using System.Collections.Generic;
using System.Text;
using FundooModel.Notes;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace FundooRepository.Repo.NotesRepository
{
    public interface INotesRepo
    {
        Task<Notes> AddNotes(Notes notes);
        Task<Notes> DeleteNotes(int id);
        Task<Notes> UpdateNotes(Notes notes);
        Task<IEnumerable<Notes>> GetAllNotes();
        Task<IEnumerable<Notes>> GetAllNotesByEmail(string email);
        Task<Notes> DeleteFromTrash(int id);
        Task<Collaborater> AddCollaborater(Collaborater collaborater);
        Task<Collaborater> DeleteCollaborater(Collaborater collaborater);
        Task<ImageUploadResult> UploadImage(int noteId, string imagePath);
    }
}
