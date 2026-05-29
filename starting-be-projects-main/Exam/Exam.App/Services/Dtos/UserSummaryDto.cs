namespace Exam.App.Services.Dtos
{
    public class UserSummaryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int CompletedProjectsCount { get; set; }
        public int InProgressProjectsCount { get; set; }
        public DateTime? LastCompletedAt { get; set; }
    }
}
