namespace EmployeeTraingsTracker.Model
{
    public class Training
    {
        public int Id { get; set; } // Primary Key
        public string Title { get; set; }
        public string Description { get; set; }

        // Navigation property (link to employees through join table)
        public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
    }
}
