/////------------------------------------------------------------------------
////<copyright file="INotesManager.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooManager.NotesManager
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using FundooModel.Notes;

    /// <summary>
    /// Notes manager interface
    /// </summary>
    public interface INotesManager
    {
        /// <summary>
        /// Add notes
        /// </summary>
        /// <param name="notes">parameter notes</param>
        /// <returns>return notes</returns>
        Task<Notes> AddNotes(Notes notes);

        /// <summary>
        /// Delete notes
        /// </summary>
        /// <param name="id">parameter ID</param>
        /// <returns>returns Notes</returns>
        Task<Notes> DeleteNotes(int id);
        
        /// <summary>
        /// Update notes
        /// </summary>
        /// <param name="notes">parameter notes</param>
        /// <returns>returns notes</returns>
        Task<Notes> UpdateNotes(Notes notes);

        /// <summary>
        /// Get all notes
        /// </summary>
        /// <returns>returns all notes</returns>
        Task<IEnumerable<Notes>> GetAllNotes();

        /// <summary>
        /// Get all notes by email address
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns list of notes</returns>
        Task<IEnumerable<Notes>> GetAllNotesByEmail(string email);

        /// <summary>
        /// Delete from trash
        /// </summary>
        /// <param name="id">parameter ID</param>
        /// <returns>Returns Notes</returns>
        Task<Notes> DeleteFromTrash(int id);

        /// <summary>
        /// Add collaborater
        /// </summary>
        /// <param name="collaborater">parameter collaborater</param>
        /// <returns>return collaborater</returns>
        Task<Collaborater> AddCollaborater(Collaborater collaborater);

        /// <summary>
        /// Delete collaborater
        /// </summary>
        /// <param name="collaborater">parameter collaborater</param>
        /// <returns>return collaborater</returns>
        Task<Collaborater> DeleteCollaborater(Collaborater collaborater);

        /// <summary>
        /// Upload image
        /// </summary>
        /// <param name="noteId">parameter note ID</param>
        /// <param name="imagePath">parameter image path</param>
        /// <returns>returns image upload result</returns>
        Task<ImageUploadResult> UploadImage(int noteId, string imagePath);

        Task<bool> ResetIsTrash(int id);
        Task<bool> SetIsTrash(int id);
        Task<bool> ResetArchive(int id);
        Task<bool> SetArchive(int id);
        Task<bool> ResetPin(int id);
        Task<bool> SetPin(int id);
        Task<bool> AddRemainder(int id, string time);
        Task<bool> DeleteRemainder(int id);
    }
}
