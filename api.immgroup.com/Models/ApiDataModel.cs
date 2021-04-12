using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace api.immgroup.com.Models
{
    public class Base64StringItem
    {
        public string Base64String { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string CustomerPhone { get; set; }
        public string FirebaseToken { get; set; }
    }

    public class ChangePasswordModel
    {
        public int CustomerId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }


    public class FeedbackModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int StaffId { get; set; }
        public string MessageText { get; set; }
        public IList<Base64StringItem> Base64StringItems { get; set; }
    }

    public class ChatModel
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerRowId { get; set; }
        public string FirebaseKey { get; set; }
        public Base64StringItem Base64StringItem { get; set; }
    }

    public class UploadFileModel
    {
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
    }

    public class FirebaseNotificationModel
    {
        public string CustomerId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }


    public class SendMailMobileModel
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
    }


    public class SendMailWebModel
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
    }

    public class NotificationModel
    {
        public int StaffId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string HtmlBody { get; set; }
        public IList<int> CustomerIds { get; set; }
    }

    public class FolderModel
    {
        public string FolderPath { get; set; }
    }

    public class CreateFolderModel
    {
        public string FolderPath { get; set; }
        public string NewFolderName { get; set; }
    }

    public class UploadFilesModel
    {
        public string FolderPath { get; set; }
        public IList<Base64StringItem> Base64StringItems { get; set; }
    }

    public class TodoListModel
    {
        public int Owner { get; set; }
        public string TaskDetails { get; set; }
        public int Rate { get; set; }
        public int Active { get; set; }       
    }

    public class ItemsQuery
    {
        public string Product { get; set; }
        public string ProfileStatus { get; set; }
        public string SeriousRate { get; set; }
        public string Evaluation { get; set; }
    }
}
