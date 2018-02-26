namespace LocalTheatre.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LocalTheatre.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LocalTheatre.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            var passwordHash = new PasswordHasher();
            String password = passwordHash.HashPassword("Secure123!");
            var author1 = new ApplicationUser
            {
                UserName = "rhendry185@gmail.com",
                PasswordHash = "letmein",
                Email = "rhendry185@gmail.com"
            };
            context.Users.AddOrUpdate(u => u.UserName,
                author1);

            var post1 = new BlogPost();
            post1.PostID = 1;
            post1.Author = author1.UserName;
            post1.PostTitle = "Reminder";
            post1.PostText = "A friendly reminder to those who signed up that the acting classes we will be hosting are beginning tonight";
            post1.PostDate = DateTime.Now;

            try
            {
                context.BlogPosts.Add(post1);
            }
            catch (SqlException ex)
            {
                var error = ex.InnerException;
            }

            var comment1 = new Comment();
            comment1.CommentID = 1;
            comment1.PostID = 1;
            comment1.CommentText = "Excellent!";
            comment1.Author = author1.UserName;
            comment1.CommentDate = DateTime.Now;

            try
            {
                context.Comments.Add(comment1);
            }
            catch (Exception ex)
            {
                var error = ex.InnerException;
            }

            var comment2 = new Comment();
            comment2.CommentID = 2;
            comment2.PostID = 1;
            comment2.CommentText = "Looking forward to it.";
            comment2.Author = author1.UserName;
            comment2.CommentDate = DateTime.Now;

            try
            {
                context.Comments.Add(comment2);
            }
            catch (SqlException ex)
            {
                var error = ex.InnerException;
            }

            var post2 = new BlogPost();
            post2.PostID = 2;
            post2.Author = author1.UserName;
            post2.PostTitle = "Welcome";
            post2.PostText = "We would like to offer a warm welcome to our new members of staff.";
            post2.PostDate = DateTime.Now;

            try
            {
                context.BlogPosts.Add(post2);
            }
            catch (SqlException ex)
            {
                var error = ex.InnerException;
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            //default user created   

            var user = new ApplicationUser();
            user.UserName = "rhendry185@gmail.com";
            user.Email = "rhendry185@gmail.com";

            string userPWD = "letmein";

            UserManager.Create(user, userPWD);
            // creation an admin role 

            if (!roleManager.RoleExists("Admin"))
            {

                  
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            //Then create an admin user                

            var adminuser = new ApplicationUser();
            adminuser.UserName = "Admin";
            adminuser.Email = "admin@localtheatre.com";

            userPWD = "Admin_123!";

            var chkUser = UserManager.Create(adminuser, userPWD);

            //Add admin user to their role   
            if (chkUser.Succeeded)
            {
                var result1 = UserManager.AddToRole(adminuser.Id, "Admin");

            }


            // create a second role  
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Restricted";
                roleManager.Create(role);

            }



        }
    }
}
