using Exam.App.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        // Seed Administrators
        await SeedUser(userManager, "john", "john.doe@example.com", "John", "Doe", "John123!", "Administrator");
        await SeedUser(userManager, "jane", "jane.doe@example.com", "Jane", "Doe", "Jane123!", "Administrator");

        // Seed Users
        await SeedUser(userManager, "alice", "alice.smith@example.com", "Alice", "Smith", "Alice123!", "User");
        await SeedUser(userManager, "bob", "bob.jones@example.com", "Bob", "Jones", "Bobj123!", "User");
        await SeedUser(userManager, "charlie", "charlie.brown@example.com", "Charlie", "Brown", "Charlie123!", "User");
        await SeedUser(userManager, "diana", "diana.prince@example.com", "Diana", "Prince", "Diana123!", "User");
        await SeedUser(userManager, "edward", "edward.norton@example.com", "Edward", "Norton", "Edward123!", "User");
        await SeedUser(userManager, "fiona", "fiona.apple@example.com", "Fiona", "Apple", "Fiona123!", "User");
        await SeedUser(userManager, "george", "george.lucas@example.com", "George", "Lucas", "George123!", "User");
        await SeedUser(userManager, "hannah", "hannah.montana@example.com", "Hannah", "Montana", "Hannah123!", "User");
        await SeedUser(userManager, "ivan", "ivan.petrov@example.com", "Ivan", "Petrov", "Ivanp123!", "User");
        await SeedUser(userManager, "julia", "julia.roberts@example.com", "Julia", "Roberts", "Julia123!", "User");
        await SeedUser(userManager, "kevin", "kevin.hart@example.com", "Kevin", "Hart", "Kevin123!", "User");
        await SeedUser(userManager, "laura", "laura.palmer@example.com", "Laura", "Palmer", "Laura123!", "User");
        await SeedUser(userManager, "mike", "mike.tyson@example.com", "Mike", "Tyson", "Miket123!", "User");
        await SeedUser(userManager, "nina", "nina.simone@example.com", "Nina", "Simone", "Ninas123!", "User");
        await SeedUser(userManager, "oscar", "oscar.wilde@example.com", "Oscar", "Wilde", "Oscar123!", "User");

        // Seed Skills
        await SeedSkills(context);

        // Seed Projects
        await SeedProjects(context, userManager);
    }

    private static async Task SeedSkills(AppDbContext context)
    {
        if (await context.Skills.AnyAsync()) return;

        var skills = new List<Skill>
        {
            new Skill { Name = "Mobile App Development" },
            new Skill { Name = "Web Design" },
            new Skill { Name = "REST API Design" },
            new Skill { Name = "Database Migration" },
            new Skill { Name = "Real-Time Communication" },
            new Skill { Name = "Authentication and Authorization" },
            new Skill { Name = "Machine Learning" },
            new Skill { Name = "Data Analytics" },
            new Skill { Name = "CRM Integration" },
            new Skill { Name = "Email Service Development" },
            new Skill { Name = "E-Commerce" },
            new Skill { Name = "UI Component Design" },
            new Skill { Name = "IoT" },
            new Skill { Name = "CI/CD" },
            new Skill { Name = "Full-Text Search" },
            new Skill { Name = "Payment Integration" },
            new Skill { Name = "Push Notifications" },
            new Skill { Name = "Blockchain" },
            new Skill { Name = "Cloud Storage" },
            new Skill { Name = "DevOps Monitoring" },
        };

        context.Skills.AddRange(skills);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProjects(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        if (await context.Projects.AnyAsync()) return;

        var alice = await userManager.FindByNameAsync("alice");
        var bob = await userManager.FindByNameAsync("bob");
        var charlie = await userManager.FindByNameAsync("charlie");
        var diana = await userManager.FindByNameAsync("diana");
        var edward = await userManager.FindByNameAsync("edward");
        var fiona = await userManager.FindByNameAsync("fiona");
        var george = await userManager.FindByNameAsync("george");

        var projects = new List<Project>
        {
            // Alice: 1 draft, 1 published, 2 completed, 0 archived
            new Project { Name = "Alice Draft App", Description = "A draft mobile app concept", StartedAt = new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Draft, CompletedAt = null, UserId = alice!.Id },
            new Project { Name = "Alice Website Redesign", Description = "Redesigning the company website", StartedAt = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Published, CompletedAt = null, UserId = alice.Id },
            new Project { Name = "Alice Inventory System", Description = "Warehouse inventory tracking system", StartedAt = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 5, 20, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = alice.Id },
            new Project { Name = "Alice Data Migration", Description = "Legacy database migration project", StartedAt = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 7, 10, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = alice.Id },

            // Bob: 0 draft, 2 published, 1 completed, 1 archived
            new Project { Name = "Bob API Platform", Description = "REST API platform for partners", StartedAt = new DateTime(2024, 8, 5, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Published, CompletedAt = null, UserId = bob!.Id },
            new Project { Name = "Bob Chat Service", Description = "Real-time messaging service", StartedAt = new DateTime(2024, 9, 12, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Published, CompletedAt = null, UserId = bob.Id },
            new Project { Name = "Bob Auth Module", Description = "Authentication and authorization module", StartedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 4, 15, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = bob.Id },
            new Project { Name = "Bob Legacy Portal", Description = "Old customer portal now archived", StartedAt = new DateTime(2023, 5, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2023, 12, 1, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Archived, UserId = bob.Id },

            // Charlie: 1 draft, 1 published, 3 completed, 1 archived
            new Project { Name = "Charlie ML Pipeline", Description = "Machine learning data pipeline", StartedAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Draft, CompletedAt = null, UserId = charlie!.Id },
            new Project { Name = "Charlie Dashboard", Description = "Analytics dashboard for sales team", StartedAt = new DateTime(2024, 10, 1, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Published, CompletedAt = null, UserId = charlie.Id },
            new Project { Name = "Charlie CRM Integration", Description = "CRM system integration with ERP", StartedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 3, 30, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = charlie.Id },
            new Project { Name = "Charlie Email Service", Description = "Transactional email microservice", StartedAt = new DateTime(2024, 4, 10, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 6, 25, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = charlie.Id },
            new Project { Name = "Charlie Report Generator", Description = "Automated PDF report generation", StartedAt = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 9, 15, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = charlie.Id },
            new Project { Name = "Charlie Old Website", Description = "Previous company website now archived", StartedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Archived, UserId = charlie.Id },

            // Diana: 2 draft, 1 published, 1 completed, 1 archived
            new Project { Name = "Diana Mobile App", Description = "Cross-platform mobile application", StartedAt = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Draft, CompletedAt = null, UserId = diana!.Id },
            new Project { Name = "Diana Design System", Description = "Component library and design tokens", StartedAt = new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Draft, CompletedAt = null, UserId = diana.Id },
            new Project { Name = "Diana E-Commerce Platform", Description = "Online store with payment integration", StartedAt = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Published, CompletedAt = null, UserId = diana.Id },
            new Project { Name = "Diana Newsletter System", Description = "Automated newsletter distribution platform", StartedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 5, 30, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = diana.Id },
            new Project { Name = "Diana Old Blog", Description = "Previous blogging platform now retired", StartedAt = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2023, 9, 15, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Archived, UserId = diana.Id },

            // Edward: 1 draft, 2 published, 0 completed, 2 archived
            new Project { Name = "Edward IoT Gateway", Description = "IoT device management gateway", StartedAt = new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Draft, CompletedAt = null, UserId = edward!.Id },
            new Project { Name = "Edward Monitoring Tool", Description = "Server health monitoring dashboard", StartedAt = new DateTime(2024, 8, 10, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Published, CompletedAt = null, UserId = edward.Id },
            new Project { Name = "Edward CI Pipeline", Description = "Continuous integration and deployment pipeline", StartedAt = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Published, CompletedAt = null, UserId = edward.Id },
            new Project { Name = "Edward Legacy CMS", Description = "Old content management system decommissioned", StartedAt = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2023, 7, 20, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Archived, UserId = edward.Id },
            new Project { Name = "Edward Deprecated API", Description = "V1 API replaced by newer version", StartedAt = new DateTime(2023, 4, 15, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Archived, UserId = edward.Id },

            // Fiona: 0 draft, 1 published, 3 completed, 1 archived
            new Project { Name = "Fiona Search Engine", Description = "Full-text search service with Elasticsearch", StartedAt = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Published, CompletedAt = null, UserId = fiona!.Id },
            new Project { Name = "Fiona Payment Gateway", Description = "Stripe and PayPal payment integration", StartedAt = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 4, 20, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = fiona.Id },
            new Project { Name = "Fiona Notification Service", Description = "Push notification microservice", StartedAt = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 6, 10, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = fiona.Id },
            new Project { Name = "Fiona User Analytics", Description = "User behavior analytics platform", StartedAt = new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 8, 30, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = fiona.Id },
            new Project { Name = "Fiona Old Dashboard", Description = "First-generation analytics dashboard", StartedAt = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2023, 11, 15, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Archived, UserId = fiona.Id },

            // George: 1 draft, 0 published, 2 completed, 2 archived
            new Project { Name = "George Blockchain Wallet", Description = "Cryptocurrency wallet application", StartedAt = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Draft, CompletedAt = null, UserId = george!.Id },
            new Project { Name = "George Task Scheduler", Description = "Background job scheduling service", StartedAt = new DateTime(2024, 2, 15, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 5, 10, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = george.Id },
            new Project { Name = "George File Storage", Description = "Cloud file storage and sharing service", StartedAt = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2024, 9, 20, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Completed, UserId = george.Id },
            new Project { Name = "George Legacy Intranet", Description = "Old company intranet portal", StartedAt = new DateTime(2023, 1, 15, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2023, 5, 30, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Archived, UserId = george.Id },
            new Project { Name = "George Retired Forum", Description = "Community forum no longer in use", StartedAt = new DateTime(2023, 4, 1, 0, 0, 0, DateTimeKind.Utc), CompletedAt = new DateTime(2023, 8, 15, 0, 0, 0, DateTimeKind.Utc), Status = ProjectStatus.Archived, UserId = george.Id },
        };

        context.Projects.AddRange(projects);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUser(UserManager<ApplicationUser> userManager, string username, string email, string name, string surname, string password, string role)
    {
        if (await userManager.FindByNameAsync(username) == null)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                Name = name,
                Surname = surname,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
