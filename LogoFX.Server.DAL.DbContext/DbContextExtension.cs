using System;
using System.Data.Entity;
using LogoFX.Server.DAL.Entities;

namespace LogoFX.Server.DAL.DbContext
{
    public static class DbContextExtension
    {
        public static void ApplyStateChanges(this System.Data.Entity.DbContext dbContext)
        {
            foreach (var dbEntityEntry in dbContext.ChangeTracker.Entries())
            {
                var entityState = dbEntityEntry.Entity as IObjectState;
                if (entityState == null)
                    throw new InvalidCastException(
                        "All entites must implement " +
                        "the IObjectState interface, this interface " +
                        "must be implemented so each entites state" +
                        "can be explicitly determined when updating graphs.");

                dbEntityEntry.State = ConvertState(entityState.State);
            }
        }

        private static EntityState ConvertState(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Added:
                    return EntityState.Added;
                case ObjectState.Modified:
                    return EntityState.Modified;
                case ObjectState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }
    }
}