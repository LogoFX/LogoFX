﻿using System;
using System.Threading.Tasks;
using LogoFX.Client.Mvvm.ViewModel.Shared;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{    
    /// <summary>
    /// Available message buttons.
    /// </summary>
    public enum MessageButton
    {
        /// <summary>
        /// OK button.
        /// </summary>
        OK = 0,

        /// <summary>
        /// OK and Cancel buttons.
        /// </summary>
        OKCancel = 1,

        /// <summary>
        /// Yes, No and Cancel buttons.
        /// </summary>
        YesNoCancel = 3,

        /// <summary>
        /// Yes and No buttons.
        /// </summary>
        YesNo = 4,
    }

    /// <summary>
    /// Available message images.
    /// </summary>
    public enum MessageImage
    {
        /// <summary>
        /// Show no image.
        /// </summary>
        None = 0,

        /// <summary>
        /// Error image.
        /// </summary>
        Error = 16,

        /// <summary>
        /// Question image.
        /// </summary>
        Question = 32,

        /// <summary>
        /// Warning image.
        /// </summary>
        Warning = 48,

        /// <summary>
        /// Information image.
        /// </summary>
        Information = 64,

        /// <summary>
        /// Success image.
        /// </summary>
        Success = 82,
    }

    /// <summary>
    /// Represents the service that allows displaying messages.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Shows the specified message and returns the result.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The button.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>The <see cref="MessageResult"/>.</returns>
        MessageResult Show(string message, string caption = "", MessageButton button = MessageButton.OK,
                           MessageImage icon = MessageImage.None);

        /// <summary>
        /// Shows the specified message and allows to await for the message to complete.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The button.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>A Task containing the <see cref="MessageResult"/>.</returns>
        Task<MessageResult> ShowAsync(string message, string caption = "", MessageButton button = MessageButton.OK, MessageImage icon = MessageImage.None);

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        MessageResult ShowError(Exception error, string caption = "");
    }
}