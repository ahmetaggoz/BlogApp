using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore;

public static class SeedData
{
    public static void TestVerileriniDoldur(IApplicationBuilder app)
    {
        var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();
        if(context != null)
        {
            if(context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if(!context.Tags.Any())
            {
                context.Tags.AddRange(
                    new Entity.Tag { Text = "Web programlama", Url = "web-programlama", Color = TagColors.danger},
                    new Entity.Tag { Text = "backend", Url = "backend", Color = TagColors.primary},
                    new Entity.Tag { Text = "frontend", Url = "frontend", Color = TagColors.secondary},
                    new Entity.Tag { Text = "fullstack", Url = "fullstack", Color = TagColors.success},
                    new Entity.Tag { Text = "php", Url = "php", Color = TagColors.warning}
                );
                context.SaveChanges();
            }
            if(!context.Users.Any())
            {
                context.Users.AddRange(
                    new Entity.User { UserName = "ahmet ağgöz",Name="Ahmet Ağgöz", Email = "info@ahmetaggoz.com", Password = "123456", Image = "p1.jpg"},
                    new Entity.User { UserName = "mahmut orhan",Name= "Mahmut Orhan", Password = "123456", Email = "info@mahmutorhan.com", Image = "p2.jpg"}
                );
                context.SaveChanges();
            }
            if(!context.Posts.Any())
            {
                context.Posts.AddRange(
                    new Entity.Post { 
                        Title = "Asp.net Core",
                        Url = "aspnet-core",
                        Description = "Asp net core dersleri",
                        Content = "Asp net core dersleri",
                        IsActive = true,
                        PublishedOn = DateTime.Now.AddDays(-10),
                        Tags = context.Tags.Take(3).ToList(),
                        Image = "1.jpg",
                        UserId = 1,
                        Comments = new List<Comment> {
                            new Comment { Text = "iyi bir kurs", PublishedOn = DateTime.Now.AddDays(-10), UserId = 1 },
                            new Comment { Text = "çok faydalı bir kurs", PublishedOn = DateTime.Now.AddDays(-20), UserId = 2 }
                        }
                    },
                    new Entity.Post { 
                        Title = "Php",
                        Url = "php",
                        Description = "Asp net core dersleri",
                        Content = "Php dersleri",
                        IsActive = true,
                        PublishedOn = DateTime.Now.AddDays(-20),
                        Tags = context.Tags.Take(2).ToList(),
                        Image = "2.jpg",
                        UserId = 1
                    },
                    new Entity.Post { 
                        Title = "Django",
                        Url = "django",
                        Description = "Asp net core dersleri",
                        Content = "Django dersleri",
                        IsActive = true,
                        PublishedOn = DateTime.Now.AddDays(-30),
                        Tags = context.Tags.Take(4).ToList(),
                        Image = "3.jpeg",
                        UserId = 2
                    },
                    new Entity.Post { 
                        Title = "React dersleri",
                        Url = "react",
                        Description = "Asp net core dersleri",
                        Content = "React dersleri",
                        IsActive = true,
                        PublishedOn = DateTime.Now.AddDays(-40),
                        Tags = context.Tags.Take(4).ToList(),
                        Image = "3.jpeg",
                        UserId = 2
                    },
                    new Entity.Post { 
                        Title = "Angular",
                        Url = "angular",
                        Description = "Asp net core dersleri",
                        Content = "Angular dersleri",
                        IsActive = true,
                        PublishedOn = DateTime.Now.AddDays(-50),
                        Tags = context.Tags.Take(4).ToList(),
                        Image = "3.jpeg",
                        UserId = 2
                    },
                    new Entity.Post { 
                        Title = "Web Tasarım",
                        Url = "web-tasarim",
                        Description = "Asp net core dersleri",
                        Content = "Web tasarım dersleri",
                        IsActive = true,
                        PublishedOn = DateTime.Now.AddDays(-60),
                        Tags = context.Tags.Take(4).ToList(),
                        Image = "3.jpeg",
                        UserId = 2
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
