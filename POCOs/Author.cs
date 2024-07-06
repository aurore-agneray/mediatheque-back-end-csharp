namespace mediatheque_back_csharp.Pocos
{
    /// <summary>
    /// POCO for the Authors
    /// </summary>
    public class Author
    {
        /// <summary>
        /// ID (primary key)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Custom code
        /// </summary>
        public String? Code { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public String? FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public String? LastName { get; set; }

        /// <summary>
        /// First name + last name (generated into the database)
        /// </summary>
        public String? CompleteName { get; set; }
    }
}
