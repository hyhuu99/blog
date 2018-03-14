namespace Database
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MiniBlog : DbContext
    {
        private object _connStr;

        // Your context has been configured to use a 'MiniBlog' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Database.MiniBlog' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MiniBlog' 
        // connection string in the application configuration file.
        public MiniBlog()
            : base("name=MiniBlog")
        {
            //Database.SetInitializer<MiniBlog>(new CreateDatabaseIfNotExists<MiniBlog>());
            //Database.SetInitializer<MiniBlog>(new DropCreateDatabaseIfModelChanges<MiniBlog>());
        }

        public MiniBlog(object connStr)
        {
            _connStr = connStr;
        }

        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Tag> Tag { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.Posts)
            //    .WithRequired(e => e.User);
            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.Comments)
            //    .WithRequired(e => e.User);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Comments)
                .WithRequired()
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Post>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Posts)
                .Map(e => e.ToTable("TagDetails"));

            modelBuilder.Entity<Tag>()
                .HasMany(e => e.Posts);
            modelBuilder.Entity<Comment>();

        }

            // Add a DbSet for each entity type that you want to include in your model. For more information 
            // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

            // public virtual DbSet<MyEntity> MyEntities { get; set; }
        }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}