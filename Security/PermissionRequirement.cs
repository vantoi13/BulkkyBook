using Microsoft.AspNetCore.Authorization;

namespace BulkkyBook.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {

        public string FunctionId { get; }
        public string CommandId { get; }

      public PermissionRequirement(string functionId, string commandId)
        {
            FunctionId = functionId;
            CommandId = commandId ;
        }
    }
}