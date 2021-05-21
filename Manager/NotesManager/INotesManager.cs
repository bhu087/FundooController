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

        /// <summary>
        /// Reset the trash
        /// </summary>
        /// <param name="id">Note id</param>
        /// <returns>Returns boolean result</returns>
        Task<bool> ResetIsTrash(int id);

        /// <summary>
        /// Set the trash
        /// </summary>
        /// <param name="id">Note id</param>
        /// <returns>Returns boolean result</returns>
        Task<bool> SetIsTrash(int id);

        /// <summary>
        /// Reset the Archive
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> ResetArchive(int id);

        /// <summary>
        /// Set the Archive
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> SetArchive(int id);

        /// <summary>
        /// Reset the Pin
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> ResetPin(int id);

        /// <summary>
        /// Set the Pin
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> SetPin(int id);

        /// <summary>
        /// Add remainder
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> AddRemainder(int id, string time);

        /// <summary>
        /// Delete remainder
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>boolean result</returns>
        Task<bool> DeleteRemainder(int id);
    }
}
