namespace EmployeeTraingsTracker.Model
{
    public class Employee
    {
         
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        // Navigation property (link to trainings through join table)
        public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
    }
}
