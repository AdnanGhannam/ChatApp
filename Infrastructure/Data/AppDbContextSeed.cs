using ChatVia.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContextSeed
    {
        // TEST USERS' names
        const string firstUserName = "AdnanGhannam1";
        const string secondUserName = "MohammadKh";

        public static async Task Seed(AppDbContext? context, 
            UserManager<AppUser>? userManager,
            ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<AppDbContextSeed>();

            try
            {
                if(context is null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                if (userManager is null)
                {
                    throw new ArgumentNullException(nameof(userManager));
                }

                // Users Seeding
                if(!context.Users.Any())
                {
                    // Create the TEST USERS
                    var user1 = new AppUser(firstUserName, "Software developer", "Damascus, SYRIA");
                    var user2 = new AppUser(secondUserName, "", "Damascus, SYRIA");

                    await userManager.CreateAsync(user1, "123");
                    await userManager.CreateAsync(user2, "123");
                }
                // Save changes
                await context.SaveChangesAsync();


                // Chats Seeding
                if(!context.Chats.Any())
                {
                    // Get The TEST USERS 
                    var user1 = context.Users.FirstOrDefault(e => e.UserName == firstUserName);
                    var user2 = context.Users.FirstOrDefault(e => e.UserName == secondUserName);

                    if(user1 is null || user2 is null)
                    {
                        throw new Exception("One of the TEST USERS or both are null, Not expected!!");
                    }

                    // Create the TEST CHAT
                    var chat = new Chat(user1, user2);

                    context.Chats.Add(chat);
                }
                // Save changes
                context.SaveChanges();


                // Messages Seeding
                if(!context.Messages.Any())
                {
                    // Get the first chat that has the first TEST USER as a member in it
                    // (Get the TEST CHAT)
                    var chat = context.Chats.FirstOrDefault(
                        e => e.Members.FirstOrDefault(m => m.UserName == firstUserName) != null);

                    if(chat == null)
                    {
                        throw new Exception("TEST CHAT is not expected to be null!!");
                    }

                    // Get one of the TEST USERS
                    var sender = chat.Members.FirstOrDefault();

                    if(sender == null)
                    {
                        throw new Exception("Sender in TEST CHAT is not expected to be null!!");
                    }

                    var message = new Message(sender, chat, "A test message");

                    context.Messages.Add(message);
                }
                // Save changes
                context.SaveChanges();

                logger.LogInformation("Seeding has been succeeded");
            }
            catch (Exception exp)
            {
                logger.LogError("Somethine went wrong while seeding!");
                logger.LogError("ErrorMessage is: " + exp.Message);
            }
        }

        [Obsolete("Not working properly")]
        public static async Task SeedAsync(AppDbContext? context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var a = await context.Users.AnyAsync();
                var u = context.Users;

                // Users Seeding
                if(a)
                {
                    // Create the TEST USERS
                    var users = new List<AppUser>()
                    {
                        new AppUser(firstUserName, "Software developer", "Damascus, SYRIA"),
                        new AppUser(secondUserName, "", "Damascus, SYRIA"),
                    };
                    
                    await context.Users.AddRangeAsync(users, cancellationToken);
                }
                // Save changes
                await context.SaveChangesAsync(cancellationToken);


                // Chats Seeding
                if(!await context.Chats.AnyAsync(cancellationToken))
                {
                    // Get The TEST USERS 
                    var user1 = await context.Users.FirstOrDefaultAsync(e => e.UserName == firstUserName);
                    var user2 = await context.Users.FirstOrDefaultAsync(e => e.UserName == secondUserName);

                    if(user1 is null || user2 is null)
                    {
                        throw new Exception("One of the TEST USERS or both are null, Not expected!!");
                    }

                    // Create the TEST CHAT
                    var chat = new Chat(user1, user2);

                    await context.Chats.AddAsync(chat);
                }
                // Save changes
                await context.SaveChangesAsync(cancellationToken);


                // Messages Seeding
                if(!await context.Messages.AnyAsync(cancellationToken))
                {
                    // Get the first chat that has the first TEST USER as a member in it
                    // (Get the TEST CHAT)
                    var chat = await context.Chats.FirstOrDefaultAsync(
                        e => e.Members.FirstOrDefault(m => m.UserName == firstUserName) != null, 
                        cancellationToken);

                    if(chat == null)
                    {
                        throw new Exception("TEST CHAT is not expected to be null!!");
                    }

                    // Get one of the TEST USERS
                    var sender = chat.Members.FirstOrDefault();

                    if(sender == null)
                    {
                        throw new Exception("Sender in TEST CHAT is not expected to be null!!");
                    }

                    var message = new Message(sender, chat, "A test message");

                    await context.Messages.AddAsync(message, cancellationToken);
                }
                // Save changes
                await context.SaveChangesAsync(cancellationToken);

                Console.WriteLine("Seeding has been succeeded");
            }
            catch (Exception exp)
            {
                Console.WriteLine("Somethine went wrong while seeding!");
                Console.WriteLine("ErrorMessage is: " + exp.Message);
            }
        }
    }
}
