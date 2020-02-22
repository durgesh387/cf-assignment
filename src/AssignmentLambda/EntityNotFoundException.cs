using System;

namespace AssignmentLambda
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string entityName, int entityId) : base(GetMessage(entityName, entityId))
        {
        }

        private static string GetMessage(string entityName, int entityId)
        {
            return $"Requested Entity : {entityName} with Id: {entityId} is not found.";
        }
    }
}