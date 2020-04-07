namespace Tedu.Server.Status.DataAccess.Entities
{
    /// <summary>
    /// An abstract class for the database entity with own ID field as a primary key.
    /// </summary>
    public abstract class EntityWithId
    {
        /// <summary>
        /// Gets or sets an integer value which uniquely identifies the record in the table.
        /// This field is the key field.
        /// </summary>
        public int Id { get; set; }
    }
}
