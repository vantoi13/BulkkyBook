using Microsoft.AspNetCore.Authorization;

namespace  BulkkyBook.Security;


public class PermissionAttribute : AuthorizeAttribute
{
   public  PermissionAttribute(string functionId, string commandId)
   : base($"Permission:{functionId}.{commandId}")
    {
      
    }



   
}